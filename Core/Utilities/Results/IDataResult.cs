using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public interface IDataResult<T>:IResult
    {
        //IDATARESULT da IRESULT gibi mesaj içersin, boolean içersin AMA AYNI ZAMANDA DATA da içersin.
        //ve burada IResult un da içerdiklerini içereceği için onu burada implement ediyoruz. ek olarak :
        //T ile ne olacağı önemli değil hangi tip ile çalışacağını biz söyleyeceğiz. 
        T Data { get; }
    }
}
