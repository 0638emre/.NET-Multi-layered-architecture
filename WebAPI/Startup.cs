using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Autofac, Ninject, CastleWindsor, StructureMap, LightInject, DryInject --> bunlar IoC Conteiner i�ini yap�yorlar.
            //AOP yap�caz biz ileride. bir metodun �n�nde sonunda hata verdi�inde �al��an kod par�ac�klar�n� yaz�yoruz.
            //bu y�zden buras� �orba olacakt�r. yukar�daki yazd���m�z uygulamalar� kullanarak i�imizi kolayla�t�r�caz.*
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });
            //singleton t�m bellekte 1 adet IProductService tutuyorsa ne zaman istenirse onu verir.
            //services.AddSingleton<IProductService, ProductManager>();
            //burada diyoruz ki. e�er sen IProductService g�r�rsen bir yerde onun ba��ml�l��� 2.parametre olan ProductManagerdir.
            //fakat yine hata verecektir. ��nk� ProductManager de ba��ml�. �imdi onu da ��zd�relim.
            //services.AddSingleton<IProductDal, EfProductDal>();
            //ARTIK BUNU KULLANMAYACA�IZ. YAN� M�CROSOFTUN B�ZE SUNDU�UNU KULLANMAYACA�IZ. B�Z AUTOFAC KULLANACA�IZ.
            //��NK� AUTOFAC B�ZE AOP DESTE�� VER�YOR.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
