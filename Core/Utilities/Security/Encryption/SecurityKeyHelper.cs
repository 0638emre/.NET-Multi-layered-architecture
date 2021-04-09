using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Encryption
{
    //şifreleme olan sistemlerde herşeyi byte[] formatında vermemiz gerekir.basit bir string ile olmaz.
    //bizim verdiğimiz WebApi içerisindeki appsettingsteki security key i byte[] haline getiren yer burasıdır.
    //JWT sisteminin anlayacağı bir yapıya çeviriyoruz burada.
    public class SecurityKeyHelper
    {
        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
        //simetrik bir security key oluşturmamıza yarayacak. UTF8 formatına encoding ediyor.
        //önemli WebApi klasörümüzdeki appsettingste biz security keyimizi verdik. ona göre bir securutiy key oluşturuyor.
    }
}
