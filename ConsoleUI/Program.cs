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
            //ProductManager productManager = new ProductManager(new InMemoryProductDal());
            //burada product managerimizi newledik. yani kullanacağımızı belirttik ve parantez içinde
            //ben inmemoryproduct üzerinde çalışıcam diye onu da newledik.
            ProductManager productManager = new ProductManager(new EfProductDal());
            //fakat şimdi biz artık database bağlantımızı EntityFramework ile kullanacağız. Enkatmanlı mimarinin faydası budur.
            foreach (var product in productManager.GetByUnitPrice(30, 100)) 
            {
                Console.WriteLine(product.ProductName);
            }  
        }
    }
}
