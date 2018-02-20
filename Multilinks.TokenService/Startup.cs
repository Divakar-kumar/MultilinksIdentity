using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Multilinks.TokenService.Services;
using System.Reflection;
using Multilinks.DataService.Entities;
using Multilinks.DataService;
using Microsoft.AspNetCore.Mvc;

namespace Multilinks.TokenService
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

         // Modify default password validation options.
         services.Configure<IdentityOptions>(o =>
         {
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequireUppercase = false;
            o.Password.RequiredLength = 8;
         });

         // Add application services.
         services.AddTransient<IEmailSender, EmailSender>();

         services.AddMvc(opt =>
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

         var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

         services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddConfigurationStore(options =>
            {
               options.ConfigureDbContext = builder =>
                   builder.UseSqlServer(_configuration.GetConnectionString("MultilinksConnectionString"),
                       sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
               options.ConfigureDbContext = builder =>
                   builder.UseSqlServer(_configuration.GetConnectionString("MultilinksConnectionString"),
                       sql => sql.MigrationsAssembly(migrationsAssembly));

               // this enables automatic token cleanup. this is optional.
               options.EnableTokenCleanup = true;
               options.TokenCleanupInterval = 30;
            })
            .AddAspNetIdentity<UserEntity>();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app)
      {
         if(_env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();
            app.UseDatabaseErrorPage();
         }
         else
         {
            app.UseExceptionHandler("/Home/Error");
         }

         app.UseStaticFiles();

         app.UseIdentityServer();

         app.UseHsts(opt =>
         {
            opt.MaxAge(days: 365);
            opt.IncludeSubdomains();
            opt.Preload();
         });

         app.UseMvcWithDefaultRoute();
      }
   }
}
