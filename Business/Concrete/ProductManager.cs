using Business.Abstract;
using Business.Constans;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
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

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        //bir alt satır aspect tir. bizim validatorumuzu çalıştıracak kısımdır. elbette bunu add fonksiyonunun içinde de yazabilirdik
        //ama o zaman kodumuz spagetti olur.amacımız temiz solid kod yazmaktır. validator işlemlerimizi aspect AOP teknikleriyle yaptık
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            //iş kodları
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
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
            //aslında burada TRY EXCEPT de kullanılabilir.Sektörde bir çok yerde o şekilde de kullanıldığını görürüz.
            //buradaki messages i business katmanındaki sabitler bölümünden aldık. yani constans.
        }

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
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p=>p.CategoryId==id));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        public IDataResult<Product> GeyById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p=>p.ProductId == productId));
        }
    }
}
