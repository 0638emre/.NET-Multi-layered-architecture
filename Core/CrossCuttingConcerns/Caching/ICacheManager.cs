using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching
{
    //bir çok Cache yazma türü vardır biz Microsoft dotnet in mekanizmasını kullanacağız.
    public interface ICacheManager
    {
        T Get<T>(string key); //bu generic versiyonudur
        object Get(string key); 
        //bu şekilde yapmamızın sebebi nerede istersek ona uygun bir parametreye sahip olsun.
        void Add(string key, object value, int duration); //key value ve kalacak süresi ni parametre olarak tutuyoruz
        bool IsAdd(string key); //cache de var mı parametresi eklemek zorundayız. çünkü varsa getirecek.
        //database den alacak fakat eğer cache de varsa oradan getirecek.
        void Remove(string key); //cache den uçuracak
        void RemoveByPattern(string key); //remove dan farkı. mesela başında get olanlardan uçur diyebiliriz koşullu silme gibi
        //ve ya işte içinde category olanları uçur(sil) gibi senaryolarda bunu kullanıcaz.
    }
}