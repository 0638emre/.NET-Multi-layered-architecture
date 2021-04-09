using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Utilities.Security.JWT
//bu class token üretilmesini sağlayan classtır.
{
    //IConfiguration bizim appsettings.json u okumamıza yarıyor.
    //tokenoptions ise bir nesneye atıyoruz. _tokenoptions diye.
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;
        //accesstoken ne zaman geçersiz hale gelecek bunu veriyoruz.
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            //TODO: .net 5.0 için araştırma yapılacak
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            //tokenoptions demek. configuration u git getsection bölümü al demektir. yani
            //TokenOptions olarak girdiğimiz class ile appsettings te girdiğimiz section u eşle diyoruz
        }
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            //bana user bilgisi ve claimleri ver. ben onlara göre bi token oluşturucam diyen fonksiyon burasıdır.
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            //bu token in ne zaman biteceği de elimizde artık.
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            //bizim yazdığımız SecurityKeyHelper i kullanarak oluştur. artık elimizde bu token i oluşturacak güvenlik anahtarı da var
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            //bu securitykey ile giriş bilgilerimizi oluşturmuş olacağız.
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }

        //artık son olarak yukarıda kullanacağımız fonksiyonu kendimiz yazıyoruz. bütün parametreleri kullanarak 
        //issuer, audience, expires, notBefore, claims, signingCredentials gibi bilgileri oluşturuyoruz kullanıcı için.
        //fakat claimler bizim için önemli onun içinde en altta bir fonkiyon metot yazdık.
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

            return claims;
            //bir JWT içerisinde sadece yetkiler olmaz başka bilgiler de olabilir.
            //kullanıcı id si, emaili, adı soyadı, rolünü.
            //claim yapısı bize dotnet içerisinde gelen bir yapıdır fakat
            //AddNameIdentifier  AddEmail AddName  AddRoles gibi yapılar orada yoktur.
            //ÖNEMLİ ! biz bunları EXTENSİON yazarak tamamlarız.
            //yani dotnet de bir nesneye yeni metotlar ekleyebiliriz. Extension genişletmek demektir.
        }
    }
}
