using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IProductDal : IEntityRepository<Product>
        //işte burada interface ile IEntity repository içindeki operasyonları kullandık ve
        //değişken T yi burada product olarak değiştirdik.
    {
       
    }
}
