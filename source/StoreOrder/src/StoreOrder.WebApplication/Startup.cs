using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.Wrappers;
using StoreOrder.WebApplication.Extensions;
using StoreOrder.WebApplication.Middlewares;
using StoreOrder.WebApplication.Middlewares.Swagger;
using StoreOrder.WebApplication.Validations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            // Add ApplicationDbContext.
            services.AddDbContext<StoreOrderDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("StoreOrderDbContext")
                    )
            );

            // Add JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    // Validate the JWT Issuer (iss) claim  
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetSection("AppSettings:Issuer").Value,
                    // Validate the JWT Audience (aud) claim  
                    ValidateAudience = true,
                    ValidAudience = Configuration.GetSection("AppSettings:Audience").Value,
                    // Validate the token expiry  
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
                // token expired
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // Add REPOSITORY
            services.AddScoped(typeof(IAuthRepository), typeof(AuthRepository));

            ConfigureSwagger(services); // contains the services.AddSwaggerGen(options => {...} ) code see method definition below

            services.AddApiVersioning(o => {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                o.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddControllers(option => {
                option.UseGeneralRoutePrefix("api/v{version:apiVersion}");
            }).AddJsonOptions(options => {
                // set this option to TRUE to indent the JSON output
                options.JsonSerializerOptions.WriteIndented = true;
                // options.JsonSerializerOptions.PropertyNamingPolicy = null;
                // set this option to NULL to use PascalCase instead of CamelCase (default)
            });

            // Custom InvalidModelStateResponseFactory
            services.Configure<ApiBehaviorOptions>(a =>
            {
                a.InvalidModelStateResponseFactory = context =>
                {
                    throw new ApiException(context.ModelState.AllErrors());
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //Add our new middleware to the pipeline
            ApiResponseOptions apiOptions = new ApiResponseOptions
            {
                ApiVersion = "1.0.0.0"
            };
            app.UseMiddleware<ApiResponseMiddleware>(apiOptions);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.  
            app.UseSwaggerUI(c =>
            {
                //c.InjectJavascript("/swagger/ui/ngnam-auth.js");
                //c.InjectJavascript("/swagger/ui/on-complete.js");
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
                foreach (var apiVersion in provider.ApiVersionDescriptions.OrderBy(version => version.ToString()))
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{apiVersion.GroupName}/swagger.json",
                         $"StoreOrder.WebApplication {apiVersion.GroupName}"
                    );
                    c.DocExpansion(DocExpansion.None);
                    c.RoutePrefix = "docs";
                    c.EnableFilter();
                    c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head, SubmitMethod.Patch, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete, SubmitMethod.Options);
                }
                
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var apiVersion in provider.ApiVersionDescriptions)
                {
                    ConfigureVersionedDescription(options, apiVersion);
                }
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer myToken\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = JwtBearerDefaults.AuthenticationScheme, //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme,
                            }
                        },new List<string>()
                    }
                });

                var xmlCommentsPath = Assembly.GetExecutingAssembly().Location.Replace("dll", "xml");
                options.IncludeXmlComments(xmlCommentsPath);
                // Register the Swagger generator, defining 1 or more Swagger documents
                options.OperationFilter<SwaggerOperationFilter>();
                options.DocumentFilter<SwaggerDocumentFilter>();
                options.OperationFilter<AuthResponsesOperationFilter>();
            });
        }

        private void ConfigureVersionedDescription(SwaggerGenOptions options, ApiVersionDescription apiVersion)
        {
            //In production code you should probably use a seperate class to get these version descriptions
            var dictionairy = new Dictionary<string, string>
            {
                { "1", "This API features several endpoints showing different API features for API version V1" },
                { "2", "This API features several endpoints showing different API features for API version V2" }
            };

            var apiVersionName = apiVersion.ApiVersion.ToString();
            options.SwaggerDoc(apiVersion.GroupName,
                new OpenApiInfo()
                {
                    Title = "StoreOrder.WebApplication",
                    Version = apiVersionName,
                    Description = dictionairy[apiVersionName],
                });
        }
    }
}
