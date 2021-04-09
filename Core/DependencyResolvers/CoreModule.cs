using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        //servis bağımlılıklarımızı startup da yapmak yerine her zaman kullanmak için burada yazıyoruz.
        public void Load(IServiceCollection serviceCollection)
        {
            //ayrıca MemoryCacheManager classında IMemoryCache _memoryCache; ın çalışması için bunu eklememiz lazım: ->
            serviceCollection.AddMemoryCache(); //dotnet artık kendisi injection etmiş oluyor.
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();
            //birisi senden ICacheManager istersen ona MemoryCacheManager ver diyoruz.
            //bunun amacı: yarın bir gün redis ile çalışmak istersek gelip buraya Memory yerine Redis yazacağız.
            serviceCollection.AddSingleton<Stopwatch>();
        }
    }
}
