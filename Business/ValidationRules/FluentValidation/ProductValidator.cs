using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    //burası bizim DOĞRULAMA validation yazdığımız yer. burası bizim product için yazdığımız yerdir.
    //AbstractValidator bize fluentValidation dan gelir.
    public class ProductValidator:AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p=>p.ProductName).MinimumLength(2);
            //buradak diyoruz ki. verdiğimiz productname in minimum uzunluğu 2 karakter olmalıdır.
            RuleFor(p => p.ProductName).NotEmpty();
            //name i boş olamaz.
            RuleFor(p => p.UnitPrice).NotEmpty();
            //fiyatı boş olamaz
            RuleFor(p => p.UnitPrice).GreaterThan(0);
            //fiyatı sıfırdan büyük olmalı.
            RuleFor(p => p.UnitPrice).GreaterThanOrEqualTo(10).When(p => p.CategoryId == 1);
            //p nin kategoriId si 1 oldğu zaman fiyatı 10a eşit veya 10dan büyük olduğu zaman.
            RuleFor(p => p.ProductName).Must(StartWithA).WithMessage("Ürünler A harfi ile başlamalıdır.");
        }

        private bool StartWithA(string arg)
        {
            return arg.StartsWith("A");
        }
    }
}
