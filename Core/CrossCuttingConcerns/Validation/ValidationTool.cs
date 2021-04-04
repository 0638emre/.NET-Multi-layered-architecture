using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//CrossCuttingConcerns nedir ?
//Bir metodu ele alalım. Her business işleminde ortak olarak yaptığımız bazı işlemler vardır.
//Bunlar, metoda ilk istek atıldığında önce yetki kontrolü yap,
//ardından eğer yapılması gereken validation işlemleri varsa onları yap, logla, cache ekle ya da cache den sil,
//bir hata aldığımızda exception handling işlemlerini gerçekleştir.
//İşte tekrarlı kullandığımız bu işlemler bizim için birer Cross Cutting Concern oluyor. 


namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        //bu sınıfın amacı. doğrulama aracı olmasıdır. Yani tekrar tekrar yazmamak için burada onu tanımlıyoruz ve
        //istediğimiz yerde onu ValidationTool.Validate(new ProductValidator(), product); şeklinde çağıroyoruz
        //mesela burada ProductValidator[içerisinde kendi kurallarımızı yazmıştık] çağırıp obje olarak da product veriyoruz.
        public static void Validate(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);
            var result = validator.Validate(context);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
