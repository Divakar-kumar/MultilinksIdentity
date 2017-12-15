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
            .AddAspNetIdentity<UserEntity>();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {
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
   }
}
