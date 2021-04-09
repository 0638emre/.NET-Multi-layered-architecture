using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Encryption
{
    public class SigningCredentialsHelper
    {
        //json web token servislerinin oluşturabilmesi için elimizde olanlardır. id şifre gibi.
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
        {
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        //burada diyoruz ki anahtar olarak 1.parametreyi kullan şifreleme olarak 2yi parametreyi (algoritmayı)
        //kullan diyoruz
        }
        //credential giriş bilgileri demektir. signing imzalama demektir.
    }
}
