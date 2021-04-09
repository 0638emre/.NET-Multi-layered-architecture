using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductTest();
            //CategoryTest();
            //DTO = Data Transformation Object
        }

        private static void CategoryTest()
        {
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
            foreach (var category in categoryManager.GetAll().Data)
            {
                Console.WriteLine(category.CategoryName);
            }
        }

        private static void ProductTest()
        {
            //ProductManager productManager = new ProductManager(new InMemoryProductDal());
            //burada product managerimizi newledik. yani kullanacağımızı belirttik ve parantez içinde
            //ben inmemoryproduct üzerinde çalışıcam diye onu da newledik.
            ProductManager productManager = new ProductManager(new EfProductDal(), new CategoryManager(new EfCategoryDal()));
            //fakat şimdi biz artık database bağlantımızı EntityFramework ile kullanacağız. Enkatmanlı mimarinin faydası budur.

            var result = productManager.GetProductDetails();

            if (result.Success == true)
            {
                foreach (var product in result.Data)
                {
                    Console.WriteLine(product.ProductName + " / " + product.CategoryName + " / " + product.UnitInStock);
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            //DTO katmanını kullanarak. ürün detaylarını artık istediğimiz gibi alabiliyoruz.
        }
    } 
}
