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
using Multilinks.ApiService.Data;
using Multilinks.ApiService.Services;
using AutoMapper;

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

         services.AddAutoMapper();

         services.AddMvcCore()
            .AddAuthorization()
            .AddJsonFormatters()
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
                        .AddJsonFile("Properties\\launchSettings.json")
                        .Build();
                  opt.SslPort = launchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
               }
               opt.Filters.Add(new RequireHttpsAttribute());
            });

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

         services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
               options.Authority = "http://localhost:5000";
               options.RequireHttpsMetadata = false;

               options.ApiName = "api1";
            });

         services.AddScoped<IEndpointService, EndpointService>();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app)
      {
         if(_env.IsDevelopment())
         {
            AddTestData(app);
            app.UseDeveloperExceptionPage();
         }

         app.UseAuthentication();

         app.UseHsts(opt =>
         {
            opt.MaxAge(days: 365);
            opt.IncludeSubdomains();
            opt.Preload();
         });

         app.UseMvc();
      }

      private static void AddTestData(IApplicationBuilder app)
      {
         using(var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
         {
            var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            /* Add test endpoints if table is empty. */
            if(context.Endpoints.Count() == 0)
            {
               Guid creatorId = Guid.NewGuid();
               Guid serviceAreaId = Guid.NewGuid();

               context.Endpoints.Add(new EndpointEntity
               {
                  EndpointId = Guid.NewGuid(),
                  ServiceAreaId = serviceAreaId,
                  CreatorId = creatorId,
                  IsCloudConnected = false,
                  IsGateway = false,
                  DirectionCapability = EndpointEntity.CommsDirectionCapabilities.receiveOnly,
                  Name = "Arduino TV Remote",
                  Description = "Receive command from the gateway and action the command on the TV"
               });

               context.Endpoints.Add(new EndpointEntity
               {
                  EndpointId = Guid.NewGuid(),
                  ServiceAreaId = serviceAreaId,
                  CreatorId = creatorId,
                  IsCloudConnected = true,
                  IsGateway = true,
                  DirectionCapability = EndpointEntity.CommsDirectionCapabilities.transmitAndReceive,
                  Name = "Arduino TV Remote Gateway",
                  Description = "Manage communications between Arduino TV Remote and other endpoints"
               });

               context.SaveChanges();
            }
         }
      }
   }
}
