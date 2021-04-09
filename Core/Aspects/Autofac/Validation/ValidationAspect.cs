using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;
        //burada Typle ValidatorType dedğimiz şey bizim iş kurallarımızı isteyeceğiz mesela PrdocutValidator.
        //buna göre burayı kullan diyoruz yani.
        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değildir.");
            }
            //burada diyoruz ki eğer bizim gönderdiğimiz validator, IValidator değilse. bu hatayı yolla diyoruz 
            _validatorType = validatorType;
        }


        //biz loglama gibi işlemlerimizi onbefore olarak seçtik. yani önce yap diyoruz.
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            //bir üst satır reflection dur. yani çalışma anında devreye girer.
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            //validatorType yani bizim product validatorun tipini bul. sonra onun generic argümanının ilkini bul. yani product.
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);
            //ardından incovation=method demek. methodun parametlerine bak. validatorün tipine eşit olan parametreleri bul.
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
            //ve her birini tek tek gez ve validation tool u kullanarak validate et.
        }
    }
}
