using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class ErrorResult:Result
    {
        //İŞTE BURASI DA TRUE DEĞİL DE FALSE DÖNDÜRÜYOR. YANİ HATAYI KARŞILAYAN CLASS BURASIDIR.
        //buradaki base => result un kendisidir.
        //bu fonksiyonda true değeri ve mesajı döndürür.
        public ErrorResult(string message) : base(false, message)
        {

        }

        //bu fonksiyonda ise sadece true döndürür. mesajı içermez. çünkü ben belki ileride mesaj göstermek istemeyeceğim.
        public ErrorResult() : base(false)
        {

        }
    }
}
