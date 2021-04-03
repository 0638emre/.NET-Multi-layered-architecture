using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
       
        public Result(bool success, string message):this(success)
        {
            Message = message;
            //burası consructor ettiğimiz yer. burada set ettik. ama hani aşağıda get var sadece set edemezdik ???
            //işte önemli olan nokta bu. consructor içerisinde set edebiliriz. (!)
            //THİS konusu: consructor bloğumuz içerisinde this kullandık ve amaç 
        }

        public Result(bool success)
        {
            Success = success;
            //bu şekilde iki adet Result fonksiyonu yazmamızın sebebi: gerçek hayatta belki ben mesaj vermek istemiyorum
            //sadece başarılı başarısız boolean döndürmek istiyorum. bu sebepledir. sanki iki farklı method varmış gibi.
            //overloading aşırı yükleme        
        }

        public bool Success { get; }
        public string Message { get; }
    }
}
