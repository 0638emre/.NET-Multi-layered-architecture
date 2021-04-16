using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constans;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.TransactionScopeAspect;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;
        ////BİR ENTİTYMANAGER KENDİSİ HARİÇ BAŞKA BİR DAL I ENJEKTE EDEMZ. !! bu yüzden burada categoryDal ı çağıramayız.
        //fakat bir kural hem product ve hem category ile ilgili ise onu ancak bu şekilde çağırıp kullanabiliriz.

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }


        //Hashing herhangi bir şifreyi , MD5 , SHA1 gibi şifrleme fonksiyonları ile saçma bir hale getirmek ve gizlemek.
        //JWT = Json Web Token
        //claim = bu alttaki satır bir claimdir.

        //bir alt satır aspect tir. bizim validatorumuzu çalıştıracak kısımdır. elbette bunu add fonksiyonunun içinde de yazabilirdik
        //ama o zaman kodumuz spagetti olur.amacımız temiz solid kod yazmaktır. validator işlemlerimizi aspect AOP teknikleriyle yaptık
        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            //iş kodları burada yazılacak.
            //iş kuralı.
            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
                CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfCategoryLimitExceded());

            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }


        [CacheAspect] //key, value şeklinde tutulur.
        //cache : Önbellek olarak bilinen cache, isminden de anlaşılacağı üzere, internette yaptığınız
        //işlemlerin geçici bir süre boyunca bilgisayarınızın belleğinde kalması anlamına gelir.
        //Böylelikle daha önce giriş yapmış olduğunuz bir sayfaya yeniden girdiğinizde,
        //bu sayfa daha hızlı ve kolay bir şekilde yüklenecektir.
        //biz burada Getall için cache yazacağız.
        public IDataResult<List<Product>> GetAll()
        {
            //iş kodları burada yazılır
            //InMemoryProductDal inMemoryProductDal = new InMemoryProductDal();
            //böyle yaparsak çok yanlış olur. çünkü bir şeyi değiştirirsek bütün newlediğimiz yerlerden değiştirmek zorunda klaırız.
            //KURAL !! BİR İŞ SINIFI BAŞKA BİR SINIFI ASLA NEW LEMEZ ! YUKARIDA İNJECTİON YAPACAĞIZ. 
            //return new DataResult<List<Product>>(_productDal.GetAll(), true, "ürünler listelendi.");
            //yani burada _productDal.GetAll() datamızdır, true başarılı cevap verir, string ise message mizi iletir.
            //aynı zamanda tıpkı add de yaptığımız gibi utilities yani araçlar klasörümüzden bir class çağırabilirz.

            //if (DateTime.Now.Hour == 00)
            //{
            //    return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            //}
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        [CacheAspect]
        [PerformanceAspect(5)] //bu metot 5 sn den fazla sürerse çalışması beni uyar.
        public IDataResult<Product> GeyById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }


        //kurallarımızı her yerde uygulayabiliriz.
        [ValidationAspect(typeof(ProductValidator))]
        //[CacheRemoveAspect("Get")] //eğer böyle verirsek bellekteki tüm içerisinde Get barındıranları iptal et demektir.(remove olduğu için)
        [CacheRemoveAspect("IProductService.Get")] //şeklinde verirsem Product üzerinden getleri sil demiş olurum
        public IResult Update(Product product)
        {
            //buraya yazılan kural Update e özel kural yazdık demektir.
            var result = _productDal.GetAll(p => p.CategoryId == product.CategoryId).Count;
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
            throw new NotImplementedException();
        }

        //aşağıda parametre bölümün de int categoryId gönderdik aynı zamanda Product product da gönderebilirdik
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            //burası arkada planda Select count(*) from products where categoryId=1 yi çalıştırır.
            //Bir kategoride en fazla 10 ürün olabilir iş kuralını yazalım.
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 15)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            //aynı ürün ismi kullanılamaz iş kuralı. 
            //Any = true ya da false döndürür.
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result == true)
            {
                //eğer başka bir ürün varsa aynı isimde hata mesajı gönder.
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }

        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            Add(product);
            if (product.UnitPrice < 10 )
            {
                throw new Exception("");
            }
            Add(product);

            return null;
        }

    }
}










//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!ADD fonksiyonu içerisindeki bilgi satıları. !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//_productDal.Add(product);
//return new Result();
//IResult Result un interfacei olduğu için burada Result u direkt kullanabiliyoruz.
//önemli: biz result içerisinde iki şey koymuştuk biri success() diğeri message() bunları vermemiz gerek ÇÜNKÜ:
//ürünü ekleme işlemini bitirdik ve sonuç vermemiz gerek.
//return new Result(true, "Ürün eklendi.");
//fakat Result şimdi bize kızar.çünkü Result u consructor etmemiz gerek.
//return ile döndürme işlemini true ve false olarak ikiye ayıyoruz. yani başarılı ve başarısız.
//bu yüzden hem true hem false için iki ayrı sınıf yazacağız. bunları base de yani resultta tutacağız.

//if (product.ProductName.Length<2)
//{
//    //MAGİC STRİNGS ?
//    return new ErrorResult(Messages.ProductNameInvalid);
//}//biz bu validation u? yani doğrulama işlemini ayrı bir klasörde(FluentValidation) yapıyoruz. bu yüzden bunu yorumluyorum.
//şimdi o VALİDATİON u çağıralım.

//!!!!!!!ÖNEMLİİİİİİİİİİİİİİİİİİİ!!!!!!!
//ValidationTool.Validate(new ProductValidator(), product); ARTIK BUNA GEREK YOK ÇÜNKÜ BİZ ASPECTS KULLANIYORUZ.
//[] ŞEKLİNDE YUKARIDA ATTRİBUTE EKLEYEREK VALİDATİON İŞLEMLERİMİZİ ORADAN YAPACAĞIZ BURADA İŞ KODLARIMIZ OLACAK.

//yani eğer false döndürecekse bi sıkıntı var ve ürünü henüz ekleme diyoruz. hata mesajımızı gösteriyoruz


//aslında burada TRY EXCEPT de kullanılabilir.Sektörde bir çok yerde o şekilde de kullanıldığını görürüz.
//buradaki messages i business katmanındaki sabitler bölümünden aldık. yani constans.

//core katmanında biz kendimize ait iş kurallarını yazdık. bu yüzden bu aşağıdaki karmaşık kodlara gerek yoktur.

//if (CheckIfProductCountOfCategoryCorrect(product.CategoryId).Success) //aynı şekilde 2. iş kuralımız NameExists burada && ile de yazılabilirdi.
//{
//    if (CheckIfProductNameExists(product.ProductName).Success)
//    {
//        _productDal.Add(product);
//        return new SuccessResult(Messages.ProductAdded);
//    }
//}
//return new ErrorResult();