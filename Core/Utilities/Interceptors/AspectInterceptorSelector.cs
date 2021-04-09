using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Interceptors
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>
                (true).ToList();
            var methodAttributes = type.GetMethod(method.Name)
                .GetCustomAttributes<MethodInterceptionBaseAttribute>(true);
            classAttributes.AddRange(methodAttributes);
            //classAttributes.Add(new ExceptionLogAspect(typeof(FileLogger)));
            //bir üstteki satırda diyor ki. otomatik olarak bütün sistemdeki kodları loglamaya davet et.
            //fakat loglama alt yapımız şuan hazır değil o yüzden onu yorum satırı yapıyorum
            //eğer burada bu loglama işlemini ya da performance işlemini çağırırsan bütün metotları kapsar ve unutulmamış olur.
            return classAttributes.OrderBy(x => x.Priority).ToArray();
            //yukarıda classın methodların attributelerini oku onları bul ve onları bir listeye koy. ardından da
            //onları öncelik değerine göre sıralayarak döndür diyoruz.
        }
    }

}
