using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        //params yazdığımızda istediğimiz kadar  içerisinde iş kuralı(parametre olarak) gönderebiliriz. logics iş kuralı demektir
        //çünkü iş kurallarımızda IResult cinsindendir.
        public static IResult Run(params IResult[] logics)
        {
            //buradaki logic iş kuralına denk gelir. logics ise bütün iş kurallarımız.
            //bütün iş kurallarımızı gezecek başarılı değilse
            foreach (var logic in logics)
            {
                if (!logic.Success)
                {
                    return logic;
                    //burada kurala uymayanları döndürür.
                }
            }
            return null;
            //kurala uyuyorsa devam.
        }
    }
}
