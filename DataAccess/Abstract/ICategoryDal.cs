using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ICategoryDal:IEntityRepository<Category>
    //işte burada da interface ile IEntity repository içindeki operasyonları kullandık ve
    //değişken T yi burada Category olarak değiştirdik.
    {

    }
}
