using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    //kendimiddleware lerimizi yazıyoruz. aslınad bizim middlewarelerimiz api de startup ın içindedir. ama biz onların içerisine butayı da ekleyebiliriz.
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            //startup codelarının içerisindeki middleware lere burayı da ekliyor 
        }
    }
}
