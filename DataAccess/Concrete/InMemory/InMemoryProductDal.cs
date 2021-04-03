using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.InMemory
{
    public class InMemoryProductDal : IProductDal
    {
        List<Product> _products;
        //burada bellekte bize product listesini getir diyoruz. c# gibi dillerde alttan tire ile global değişkene atanır
        //ctor yazıp tab tab yaptığımızda consructor oluşturma bloğu oluşur.
        public InMemoryProductDal()
        {
            //bu sanki bize oracle, sqlserver, postgres, mongodb gibi veritabanlarından geliyormuş gibi simüle ediyoruz
            _products = new List<Product> {
                new Product{ProductId=1, CategoryId=1, ProductName="Bardak", UnitPrice=15, UnitsInStock=15},
                new Product{ProductId=2, CategoryId=1, ProductName="Kamera", UnitPrice=500, UnitsInStock=3},
                new Product{ProductId=3, CategoryId=2, ProductName="Telefon", UnitPrice=1500, UnitsInStock=2},
                new Product{ProductId=4, CategoryId=2, ProductName="Klavye", UnitPrice=150, UnitsInStock=65},
                new Product{ProductId=5, CategoryId=2, ProductName="Fare", UnitPrice=85, UnitsInStock=1}
            };
            //burada product(ürün) oluşturuyorum. bellekte newliyoruz çünkü.
        }


        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Delete(Product product)
        {
            //NOT:  _products.Remove(product); şeklinde bir ürünü silemeyiz. çünkü referans tipi silemeyiz.
            //bu yüzden şöyle sileriz:

            //burada silinecek ürün diye bir değişken yazıyoruz ve o değişkenin değeri boş(null)
            //Product productToDelete = null;
            //foreach (var p in _products)
            //{
            //    if (product.ProductId == p.ProductId)
            //    {
            //        productToDelete = p;
            //    }
            //}
            //ardından geçiçi bir p oluşturup bütün _products listesini dolaşıyor ve
            //eğer bizim silinmesini istediğimiz id yi görürsen onu başta boş değişken olarak atatığımız
            //productToDelete ye sabitliyoruz
            //_products.Remove(productToDelete);
            //ve ardından onu kaldırıyoruz.

            //Bu daha kısa yazılabilir.Language Integrated Query [LINQ]
            //şimdi linq ile nasıl yazılır onu görelim:
            Product productToDelete = _products.SingleOrDefault(p=>p.ProductId==product.ProductId);
            //burada parantez içindeki kısım yukarıdaki foreach gibi gezmeyi ve aramayı yapıyor.
            //NOT: "  =>  " bu işaretin adı lambda dır. 
            _products.Remove(productToDelete);
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            return _products;
            //burada productları(ürünlerin) hepsini getir diyor. bu yüzden return ile döndürüyoruz
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllByCategory(int categoryId)
        {
            return _products.Where(p => p.CategoryId == categoryId).ToList();
            //burada isteğimiz: categoryid yi bir liste olarak görmek.
            //return ile döndürüyoruz şunu : 
            //where ile içerisindeki şarta uyan bütün durumları .tolist ile yeni bir liste yapar

        }

        public List<ProductDetailDto> GetProductDetails()
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            //yine linq kullanarak bir döngü araması ile güncellenecek ürünü buluyoruz.
            Product productToUpdate = _products.SingleOrDefault(p => p.ProductId == product.ProductId);
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.CategoryId = product.CategoryId;
            productToUpdate.UnitPrice = product.UnitPrice;
            productToUpdate.UnitsInStock = product.UnitsInStock;        }
    }
}
