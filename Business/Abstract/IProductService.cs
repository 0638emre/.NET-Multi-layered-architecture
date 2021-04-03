using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService
    {
        IDataResult<List<Product>> GetAll();
        IDataResult<List<Product>> GetAllByCategoryId(int id);
        IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max);
        IDataResult<List<ProductDetailDto>> GetProductDetails();
        IDataResult<Product> GeyById(int productId);
        IResult Add(Product product);
        //işte burada önceden void Add(Product product); var idi. biz artık Utilities klasörümüzden(araçlar) kendi yazdığımız
        //result u getirdik. (string döndüden message ve bool döndüren succesi)
        //aynı şekilde diğer kodlarımızı da IDataResult ile çağırdık. yani message, boolean ve data ile birlikte.
    }
}
