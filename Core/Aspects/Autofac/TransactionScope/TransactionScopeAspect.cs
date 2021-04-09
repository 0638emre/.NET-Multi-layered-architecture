using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Aspects.Autofac.TransactionScopeAspect
{
    public class TransactionScopeAspect : MethodInterception
    {
        //invocation senin methodun demektir.
        //intercept demek bu methodu çalıştır demektir
        public override void Intercept(IInvocation invocation)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                //buradaki transaction scope sistemden using edilir ki aynı anda veritabanında iki işlem uygulanacaksa kullanılır.
                try
                {
                    invocation.Proceed();
                    //invocation (benim metodum) onu çalıştır.
                    transactionScope.Complete();
                }
                catch (System.Exception e)
                {
                    //eğer bir hata varsa  onu dispose et.elden çıkar.
                    transactionScope.Dispose();
                    throw;
                }
            }
        }
    }
}