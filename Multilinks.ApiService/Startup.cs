using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Formatters;
using Multilinks.ApiService.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Multilinks.ApiService.Filters;
using Multilinks.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Multilinks.ApiService.Services;
using AutoMapper;
using Multilinks.DataService.Entities;
using Multilinks.DataService;
using Microsoft.AspNetCore.Identity;

namespace Multilinks.ApiService
{
   public class Startup
   {
      private IConfiguration _configuration { get; }
      private IHostingEnvironment _env { get; }

      public Startup(IConfiguration configuration, IHostingEnvironment env)
      {
         _configuration = configuration;
         _env = env;
      }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(_configuration.GetConnectionString("MultilinksConnectionString")));

         services.AddIdentity<UserEntity, UserRoleEntity>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

         services.AddAutoMapper();

         services.AddMvcCore()
            .AddAuthorization()
            .AddJsonFormatters()
            .AddDataAnnotations()
            .AddMvcOptions(opt =>
            {
               var jsonFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single();
               opt.OutputFormatters.Remove(jsonFormatter);
               opt.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));

               opt.Filters.Add(typeof(JsonExceptionFilter));
               opt.Filters.Add(typeof(LinkRewritingFilter));

               if(!_env.IsProduction())
               {
                  var launchJsonConfig = new ConfigurationBuilder()
                        .SetBasePath(_env.ContentRootPath)
                        .AddJsonFile("Properties\\launchSettings.json", optional: true)
                        .Build();
                  opt.SslPort = launchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
               }
               opt.Filters.Add(new RequireHttpsAttribute());

               opt.CacheProfiles.Add("Static", new CacheProfile { Duration = 86400 });
               opt.CacheProfiles.Add("Collection", new CacheProfile { Duration = 30 });
               opt.CacheProfiles.Add("Resource", new CacheProfile { Duration = 60 });
            });

         /* TODO: CORS policy will need to be updated before deployment. */
         services.AddCors(options => options.AddPolicy("CorsAny", builder =>
         {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowCredentials()
            .AllowAnyHeader();
         }));

         services.AddRouting(opt => opt.LowercaseUrls = true);

         services.AddApiVersioning(opt =>
         {
            opt.ApiVersionReader = new MediaTypeApiVersionReader();
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opt);
         });

         services.Configure<MultilinksInfoViewModel>(_configuration.GetSection("Info"));
         services.Configure<PagingOptions>(_configuration.GetSection("DefaultPagingOptions"));

         services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
               options.Authority = "http://localhost:5000";

               /* TODO: Remove when deployed. */
               options.RequireHttpsMetadata = false;

               options.ApiName = "api1";
            });

         services.AddScoped<IEndpointService, EndpointService>();
         services.AddScoped<IUserService, UserService>();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app)
      {
         if(_env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseAuthentication();

         app.UseHsts(opt =>
         {
            opt.MaxAge(days: 365);
            opt.IncludeSubdomains();
            opt.Preload();
         });

         app.UseCors("CorsAny");

         app.UseMvc();
      }
   }
}
