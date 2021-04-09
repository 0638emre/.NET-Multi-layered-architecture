using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
            //singleton t�m bellekte 1 adet IProductService tutuyorsa ne zaman istenirse onu verir.
            //services.AddSingleton<IProductService, ProductManager>();
            //burada diyoruz ki. e�er sen IProductService g�r�rsen bir yerde onun ba��ml�l��� 2.parametre olan ProductManagerdir.
            //fakat yine hata verecektir. ��nk� ProductManager de ba��ml�. �imdi onu da ��zd�relim.
            //services.AddSingleton<IProductDal, EfProductDal>();
            //ARTIK BUNU KULLANMAYACA�IZ. YAN� M�CROSOFTUN B�ZE SUNDU�UNU KULLANMAYACA�IZ. B�Z AUTOFAC KULLANACA�IZ.
            //��NK� AUTOFAC B�ZE AOP DESTE�� VER�YOR.

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });

            //burada diyoruz ki. sistemimizde biz JWT kullanca��z haberin olsun.
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
                });
            services.AddDependencyResolvers(new ICoreModule[] {
                new CoreModule()
            });
            //burada coremodule gibi farkl� moduller olu�turup burada new diyerek ekleyebiliriz.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //burada asp.net ya�am d�ng�s�nde hangi yap�lar�n s�ras�yla devreye girdi�ini s�yly�oruz.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
