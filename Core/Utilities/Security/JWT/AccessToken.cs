using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.JWT
{
    //erişim anahtarı.
    public class AccessToken
    {
        public string Token { get; set; }
        //anlamsız karakterlerden oluşan bir yapıdır token. jeton da diyebiliriz.
        //kullanıcı bir id ve şifre verecek ve biz ona bir token vericez.
        public DateTime Expiration { get; set; }
        //expiration tokenin bitiş zamanını veren değerdir.
    }
}
