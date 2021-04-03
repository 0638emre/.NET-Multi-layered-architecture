using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

//NuGet = > c# ın kullandığı başkalarının yazdığı paketlerin olduğu yer
//.net core un içerisinde EntityFramework bir paketle geliyor
//nuget içerisinde entityframeworkcore u indirdik ve artık biz dataaccess içerisinde ef kodu yazabiliriz
//EF (entityframework) kodlarımız ile database arasındaki iletişimi sağlayan bir köprüdür.
//kısacası bizim varlıklar(entities) içerisindeki somutlarımız(concrete)lerimizi veri tabanı ile
//ilişkilendirmemiz için bir bağ bir köprüdür EF.

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity,TContext>:IEntityRepository<TEntity>
        where TEntity: class, IEntity, new()
        where TContext: DbContext, new()
    {
        public void Add(TEntity entity)
        {
            //IDisposable pattern implementation of C#
            //yani buradaki using en üstteki using ile aynı değildir. bu c# a özgüdür.
            //context pahalı bir yapı olduğu için garbage collector biz bunu kullandıktan sonra bellekten siler.
            using (TContext context = new TContext())
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

        public void Delete(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
                //
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
                //burada eğer filter null ise git context i product a set et ve onu bana getir
                //değilse verdiğim filtre ile getir.
                //yani DB sorgularını burada kullanıyoruz. select.
            }
        }

        public void Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
