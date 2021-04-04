using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
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

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
            //Autofac bize interceptor imkanıda veriyor. burada çalışan conteiner içerisinde implement edilmiş interfaceleri
            //bul. onlar için aspectInterceptorSelector u çalıştır diyor.

        }
        //burada ProductManager istediğim zaman bana IProductService yi getir demek istiyoruz
        //sonundaki SingleInstance ise: 1 adet örnek oluştur ve nerde ne zaman istersem bana onu verirsin yeniden newleme diyoruz
    }
}
