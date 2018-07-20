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
using System;
using IdentityServer4;
using Microsoft.IdentityModel.Tokens;

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
         var connectionString = _configuration.GetConnectionString("MultilinksConnectionString");
         var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

         services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(connectionString));

         services.AddIdentity<UserEntity, UserRoleEntity>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

         // Modify default password validation options.
         services.Configure<IdentityOptions>(o =>
         {
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequireUppercase = false;
            o.Password.RequiredLength = 8;

            o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            o.Lockout.MaxFailedAccessAttempts = 5;
         });

         // Add application services.
         services.AddTransient<IEmailSender, EmailSender>();

         services.AddMvc(opt =>
         {
            if(_env.IsDevelopment())
            {
               var launchJsonConfig = new ConfigurationBuilder()
                     .SetBasePath(_env.ContentRootPath)
                     .AddJsonFile("Properties\\launchSettings.json", optional: true)
                     .Build();
               opt.SslPort = launchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");

            }
            opt.Filters.Add(new RequireHttpsAttribute());
         });

         var identityServerbuilder = services.AddIdentityServer(options =>
            {
               options.Events.RaiseErrorEvents = true;
               options.Events.RaiseInformationEvents = true;
               options.Events.RaiseFailureEvents = true;
               options.Events.RaiseSuccessEvents = true;
            })
            .AddConfigurationStore(options =>
            {
               options.ConfigureDbContext = builder =>
                   builder.UseSqlServer(connectionString,
                       sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
               options.ConfigureDbContext = builder =>
                   builder.UseSqlServer(connectionString,
                       sql => sql.MigrationsAssembly(migrationsAssembly));

               /* TODO: This is optional, it enables automatic token cleanup.*/
               options.EnableTokenCleanup = true;
               options.TokenCleanupInterval = 30;
            })
            .AddAspNetIdentity<UserEntity>();

         if(_env.IsDevelopment())
         {
            identityServerbuilder.AddDeveloperSigningCredential();
         }
         else
         {
            throw new Exception("need to configure key material");
         }

         /* TODO: CORS policy will need to be updated before deployment. */
         services.AddCors(options => options.AddPolicy("CorsAny", builder =>
         {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowCredentials()
            .AllowAnyHeader();
         }));
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

         app.UseCors("CorsAny");

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
