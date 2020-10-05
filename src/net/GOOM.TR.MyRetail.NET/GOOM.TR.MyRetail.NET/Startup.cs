using GOOM.TR.MyRetail.NET.Repos;
using GOOM.TR.MyRetail.NET.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GOOM.TR.MyRetail.NET
{
    public class Startup
    {
        public static readonly IContractResolver ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };

        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        { 
            ContractResolver = ContractResolver
        };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PriceStoreDatabaseSettings>(
                Configuration.GetSection(nameof(PriceStoreDatabaseSettings)));
            services.AddSingleton<IPriceStoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<PriceStoreDatabaseSettings>>().Value);

            services.AddMemoryCache();

            services.AddControllers()
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.ContractResolver = ContractResolver;
                });

            services.AddSwaggerGen();
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSingleton<IPriceRepo, PriceRepo>();
            services.AddSingleton<IProductRepo, ProductRepo>();
            services.AddSingleton<IProductService, ProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(); app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "My API V1");
            });

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
