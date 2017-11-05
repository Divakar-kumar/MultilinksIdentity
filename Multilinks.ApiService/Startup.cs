using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Formatters;
using Multilinks.ApiService.Infrastructure;

namespace Multilinks.ApiService
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
         services.AddMvcCore()
            .AddAuthorization()
            .AddJsonFormatters()
            .AddMvcOptions(opt =>
            {
               var jsonFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single();
               opt.OutputFormatters.Remove(jsonFormatter);
               opt.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));
            });

         services.AddRouting(opt => opt.LowercaseUrls = true);

         services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
               options.Authority = "http://localhost:5000";
               options.RequireHttpsMetadata = false;

               options.ApiName = "api1";
            });
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {
         if(env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseAuthentication();

         app.UseMvc();
      }
   }
}
