using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Core.Extensions;
using Business.Constans;

namespace Business.BusinessAspects.Autofac
{
    //burası JWT içindir.
    //ProductManager da yetkilendirme gibi işlemleri çalıştırırken array olarak örneğin add fonksiyonunun üzerinde veririrz.
    //bir attribute olarak veriyoruz yani. [SecuredOperation(yetkilendirme)]
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        //array string ile rolleri oluştur.
        private IHttpContextAccessor _httpContextAccessor;
        //jwt ya istek gönderdiğimizde her gönderen için httpcontextoluşur.

        public SecuredOperation(string roles)
        {
            //burada rollerimiz virgül ile ayrılarak geliyor. çünkü attribute.
            _roles = roles.Split(',');
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            //servicetool bizim injection altyapımımızı aspect için okuyabilmemize yarayan bir araçtır.
            //aspecte inject edemediğmiiz için service tool kullandık onu da core katmanında IoC (injection of controlls) da yazdık
        }

        //onBefore yani öncesinde bunu çalıştır diyoruz. örneğin ekleme metodunun önünde çalıştır.
        protected override void OnBefore(IInvocation invocation)
        {
            //bu kullanıcının claimlerini gez ve ilgili rol varsa metodu çalıştır diyoruz yoksa yetkin yok mesajınnı ver. 
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))
                {
                    return;
                }
            }
            throw new Exception(Messages.AuthorizationDenied);
        }
    }
}
