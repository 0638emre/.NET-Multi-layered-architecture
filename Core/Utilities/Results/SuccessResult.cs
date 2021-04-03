using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class SuccessResult:Result
    {
        //buradaki base => result un kendisidir.
        //bu fonksiyonda true değeri ve mesajı döndürür.
        public SuccessResult(string message) : base(true, message)
        {

        }

        //bu fonksiyonda ise sadece true döndürür. mesajı içermez. çünkü ben belki ileride mesaj göstermek istemeyeceğim.
        public SuccessResult() : base(true)
        {

        }
    }
}
