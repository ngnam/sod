using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Extensions;
using StoreOrder.WebApplication.Middlewares;

namespace StoreOrder.WebApplication
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
            services.AddControllers(option => {
                option.UseGeneralRoutePrefix("api/v{version:apiVersion}");
                }).AddJsonOptions(options => {
                   // set this option to TRUE to indent the JSON output
                   options.JsonSerializerOptions.WriteIndented = true;
                   // options.JsonSerializerOptions.PropertyNamingPolicy = null;
                }); // set this option to NULL to use PascalCase instead of CamelCase (default)

            services.AddApiVersioning(o => o.ReportApiVersions = true);

            // Add ApplicationDbContext.
            services.AddDbContext<StoreOrderDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("StoreOrderDbContext")
                    )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //Add our new middleware to the pipeline
            ApiResponseOptions apiOptions = new ApiResponseOptions
            {
                ApiVersion = "1.0.0.0"
            };
;            app.UseMiddleware<ApiResponseMiddleware>(apiOptions);

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
