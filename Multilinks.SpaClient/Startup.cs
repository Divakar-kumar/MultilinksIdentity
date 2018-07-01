using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Multilinks.SpaClient
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

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddMvcCore()
            .AddAuthorization()
            .AddRazorViewEngine()
            .AddJsonFormatters()
            .AddMvcOptions(opt =>
            {
               if(!_env.IsProduction())
               {
                  var launchJsonConfig = new ConfigurationBuilder()
                        .SetBasePath(_env.ContentRootPath)
                        .AddJsonFile("Properties\\launchSettings.json", optional: true)
                        .Build();
                  opt.SslPort = launchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
               }
               opt.Filters.Add(new RequireHttpsAttribute());
            });

         services.AddRouting(opt => opt.LowercaseUrls = true);

         services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
               options.Authority = "http://localhost:5000";

               /* TODO: Remove when deployed. */
               options.RequireHttpsMetadata = false;

               options.ApiName = "SpaClientBackend";
            });
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app)
      {
         if(_env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
            app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
            {
               HotModuleReplacement = true
            });
         }
         else
         {
            app.UseExceptionHandler("/Home/Error");
         }

         app.UseAuthentication();

         app.UseStaticFiles();

         app.UseMvc(routes =>
         {
            routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");

            routes.MapSpaFallbackRoute(
                   name: "spa-fallback",
                   defaults: new { controller = "Home", action = "Index" });
         });
      }
   }
}
