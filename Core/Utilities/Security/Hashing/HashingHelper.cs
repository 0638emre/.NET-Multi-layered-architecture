using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Hashing
{
    public class HashingHelper
    {
        //burada şifrelerimizi hashliyoruz ve HMAC teknolojisini kullanacağız.
        //dotnet in kriptopgrafisini kullanıyoruz.SHA512 algoritmasını kullanacğaız.  
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                //buradaki key ilgili algoritmanın oluştuduğu key dir. her user için ayrı oluşturur.
            }
        }

            //üstteki yazdığımız kod. Create.... hash ile şifremizi oluşturmaya yarar.
            //alttaki yazdığımız kod. Verify.... sisteme girmeye çalışan birinin yazdığı şifre ile aslolan şifreyi 
            //karşılaştırmaya doğru ise true yanlış ise false döndürmeye yarar.
       
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            //burada parametre olarak out kullanmadık çünkü bu değerleri biz vereceğiz.
            //buradaki amacımız şifreyi doğrulama. yukarıdaki fonksiyondan gelen hashing şifresi ile
            //girdiğimiz hashing şifresi karşılaştırılacak ve sonucu boolean dönecek.
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i]!=passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
