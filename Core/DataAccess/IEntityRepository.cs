using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    //hem category hem product içerisinde zaten kullancağımız operasyonlar var ve burada onları tutacağız.
    //by yüzden burada tutacağız onları. T ile de değişken atıyoruz. ne verirsek onu alsın diye.
    //generic constraint = generic kısıt demektir. yani biz T yi de kısıtlıyoruz.
    //yani diyor ki class olabilir(referans tip olarak)
    //veya IEntity olabilir veya IEntity implemente eden bir nesne olabilir
    //fakat biz IEntity olmasını istemiyor ve IEntity newlenemediğin için biz new() de ekledik ki bize
    //IEntity yi getirmesin.
    public interface IEntityRepository<T> where T:class,IEntity,new()
    {
        List<T> GetAll(Expression<Func<T,bool>> filter=null);
        //buradaki amaç; get all ile bütün istekleri getirecektir. fakat filtre kullanmamız lazım.
        //filtreyi null ediyoruz ki aslında bize herşeyi getirsin. çünkü getall. :)
        T Get(Expression<Func<T, bool>> filter);
        //işte burada da filtre null değil. filtre vermek zorundayız çünkü sınırlı bir şey isteyeceğiz.
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        //List<T> GetAllByCategory(int categoryId);
        //işte yukarıda yaptğımız filtreleme işleminden dolayı bu satırı yazmamıza ve uygulamımıza gerek yoktur.
    }
}
