using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        //BURADA DA ERROR BÖLÜMÜNÜ YAZIYORUZ.
        public ErrorDataResult(T data, string message) : base(data, false, message)
        {
            //burada hem data, hem işlem sonucu hem de mesaj
        }

        public ErrorDataResult(T data) : base(data, false)
        {
            //mesaj olayına girmek istemediği zaman bunu alır.
        }

        public ErrorDataResult(string message) : base(default, false, message)
        {
            //data yı default haliyle döndürmek ister yani sadece mesaj gidecek.
        }

        public ErrorDataResult() : base(default, false)
        {
            //hiç bir şey verme sadece işlem sonucu fgelsin.
        }
    }
}
