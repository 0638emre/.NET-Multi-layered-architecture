using Castle.DynamicProxy;
using System;

namespace Core.Utilities.Interceptors
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
    {
        //IInterceptor: kodlamanın her yerinde loglamak gibi kodları kullanmak yerine 
        //IInterceptoru kullanırız.Interceptor’lar belirli noktalarda metot çağrımları sırasında araya girerek
        //bizlerin çakışan ilgilerimizi işletmemizi ve yönetmemizi sağlamakta.
        //Böylece metotların çalışmasından önce veya sonra bir takım işlemleri gerçekleştirebilmekteyiz.
        //bu classtaki IInterceptor bize Autofactan geliyor. bu paket içerisinden core a da ekledik.
        public int Priority { get; set; }
        //Priority öncelik demektir. hangi attribute önce çalışsın. bir metottan önce ki satırda verilen
        //[log] attributedir.
        public virtual void Intercept(IInvocation invocation)
        {

        }
    }

}
