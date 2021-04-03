using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class SuccessDataResult<T>:DataResult<T>
    {
        public SuccessDataResult(T data, string message):base(data,true,message)
        {
            //burada hem data, hem işlem sonucu hem de mesaj
        }

        public SuccessDataResult(T data):base(data,true)
        {
            //mesaj olayına girmek istemediği zaman bunu alır.
        }

        public SuccessDataResult(string message):base(default, true,message)
        {
            //data yı default haliyle döndürmek ister yani sadece mesaj gidecek.
        }

        public SuccessDataResult():base(default,true)
        {
             //hiç bir şey verme sadece işlem sonucu fgelsin.
        }
    }
}
