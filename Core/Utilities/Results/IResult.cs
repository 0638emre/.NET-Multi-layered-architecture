using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//UTİLİTİES => araçlar klasörümüzdür. projelerimizde araç olarak kullanacağımız bir katmandır.
namespace Core.Utilities.Results
{
    //Temel voidler için başlangıç.
    //burada true false değeri döndüren bir succes kurduk. başaralı mı sonu. evet hayır döndürecek
    //aynı zamanda bir string mesaj döndürecek. neden başarısız gibi.
    public interface IResult
    {
        bool Success { get; }
        //get sadece okunabilir demektir. //set ise yazılabilir demektir
        string Message { get; }

    }
}
