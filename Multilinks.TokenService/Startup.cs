using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Multilinks.TokenService.Data;
using Multilinks.TokenService.Models;
using Multilinks.TokenService.Services;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using System.Linq;

namespace Multilinks.TokenService
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
         services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("MultilinksConnectionString")));

         services.AddIdentity<ApplicationUser, ApplicationRole>()
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

         services.AddMvc();

         var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

         services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddConfigurationStore(options =>
            {
               options.ConfigureDbContext = builder =>
                   builder.UseSqlServer(Configuration.GetConnectionString("MultilinksConnectionString"),
                       sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
               options.ConfigureDbContext = builder =>
                   builder.UseSqlServer(Configuration.GetConnectionString("MultilinksConnectionString"),
                       sql => sql.MigrationsAssembly(migrationsAssembly));

               // this enables automatic token cleanup. this is optional.
               options.EnableTokenCleanup = true;
               options.TokenCleanupInterval = 30;
            })
            .AddAspNetIdentity<ApplicationUser>();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {
         // This will do the initial DB population for identity server configuration data store.
         InitializeDatabase(app);

         if(env.IsDevelopment())
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

         app.UseMvcWithDefaultRoute();
      }

      private void InitializeDatabase(IApplicationBuilder app)
      {
         using(var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
         {
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            if(!context.Clients.Any())
            {
               foreach(var client in Config.GetClients())
               {
                  context.Clients.Add(client.ToEntity());
               }
               context.SaveChanges();
            }

            if(!context.IdentityResources.Any())
            {
               foreach(var resource in Config.GetIdentityResources())
               {
                  context.IdentityResources.Add(resource.ToEntity());
               }
               context.SaveChanges();
            }

            if(!context.ApiResources.Any())
            {
               foreach(var resource in Config.GetApiResources())
               {
                  context.ApiResources.Add(resource.ToEntity());
               }
               context.SaveChanges();
            }
         }
      }
   }
}
