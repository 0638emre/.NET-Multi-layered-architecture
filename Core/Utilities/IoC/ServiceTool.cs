using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.IoC
{
    //IoC = Injections of control demektir.
    //dotnet için servislerini kullanarak kendin build et.
    //dolayısıyla web abi ve ya autofac de oluşturduuğmuz injectionları oluşturabilmemize yarıyor.
    public static class ServiceTool
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static IServiceCollection Create(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}
