using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    //NuGet = > c# ın kullandığı başkalarının yazdığı paketlerin olduğu yer
    //.net core un içerisinde EntityFramework bir paketle geliyor
    //nuget içerisinde entityframeworkcore u indirdik ve artık biz dataaccess içerisinde ef kodu yazabiliriz
    //EF (entityframework) kodlarımız ile database arasındaki iletişimi sağlayan bir köprüdür.
    //kısacası bizim varlıklar(entities) içerisindeki somutlarımız(concrete)lerimizi veri tabanı ile
    //ilişkilendirmemiz için bir bağ bir köprüdür EF.
    public class EfProductDal : IProductDal
    {
        public void Add(Product entity)
        {
            //IDisposable pattern implementation of C#
            //yani buradaki using en üstteki using ile aynı değildir. bu c# a özgüdür.
            //context pahalı bir yapı olduğu için garbage collector biz bunu kullandıktan sonra bellekten siler.
            using (NorthwindContext context = new NorthwindContext())
            {
                var addedEntity = context.Entry(entity); //1
                addedEntity.State = EntityState.Added;  //2
                context.SaveChanges();  //3
                //1 : addedEntity adından bir değişken ürettik. ef ye özgü yapı ile parantez içerisindeki bizim
                //kendi entitymiz deki varlıkları buraya eşleştirdik, girişini gerçekleştirdik
                //2 : State yapısı da EF den geliyor. ve Added fonksiyonu ile ekleme işlemini gerçekleştirdik.
                //3 : son olarak bu context deki değişiklikleri kayıt ettik.
            }
        }

        public void Delete(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                return context.Set<Product>().SingleOrDefault(filter);
            }
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                return filter == null 
                    ? context.Set<Product>().ToList() 
                    : context.Set<Product>().Where(filter).ToList();
                //burada eğer filter null ise git context i product a set et ve onu bana getir
                //değilse verdiğim filtre ile getir.
                //yani DB sorgularını burada kullanıyoruz. select.
            }
        }

        public void Update(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
