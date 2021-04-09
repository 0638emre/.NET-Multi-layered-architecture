using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        //burası conteinerda oluşacak ve istenilen classların verildiği bir araç olarak düşünülebilir
        //autofac bu araçlardan biridir.
        //Module yapısı buraya Autofac tarafından implement edilmiştir. 
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();
            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();

            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().SingleInstance();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
            //Autofac bize interceptor imkanıda veriyor. burada çalışan conteiner içerisinde implement edilmiş interfaceleri
            //bul. onlar için aspectInterceptorSelector u çalıştır diyor.

        }
        //burada IProductService istediğim zaman bana ProductManager i getir demek istiyoruz
        //sonundaki SingleInstance ise: 1 adet örnek oluştur ve nerde ne zaman istersem bana onu verirsin yeniden newleme diyoruz
    }
}
