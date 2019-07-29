using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Multilinks.TokenService.Services;
using System;
using System.Linq;

namespace Multilinks.TokenService
{
   public class SeedData
   {
      public static void EnsureSeedData(IServiceProvider serviceProvider)
      {
         Console.WriteLine("Seeding database...");

         using(var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
         {
            scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            {
               var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
               var idpConfigContext = scope.ServiceProvider.GetRequiredService<Config>();

               context.Database.Migrate();
               EnsureSeedData(context, idpConfigContext);
            }

            {
               var context = scope.ServiceProvider.GetRequiredService<TokenServiceDbContext>();
               context.Database.Migrate();
            }
         }

         Console.WriteLine("Done seeding database.");
         Console.WriteLine();
      }

      private static void EnsureSeedData(ConfigurationDbContext context, Config idpConfigContext)
      {
         if(!context.Clients.Any())
         {
            Console.WriteLine("Clients being populated");

            foreach(var client in idpConfigContext.GetClients().ToList())
            {
               context.Clients.Add(client.ToEntity());
            }

            context.SaveChanges();
         }
         else
         {
            Console.WriteLine("Clients already populated");
         }

         if(!context.IdentityResources.Any())
         {
            Console.WriteLine("IdentityResources being populated");

            foreach(var resource in idpConfigContext.GetIdentityResources().ToList())
            {
               context.IdentityResources.Add(resource.ToEntity());
            }

            context.SaveChanges();
         }
         else
         {
            Console.WriteLine("IdentityResources already populated");
         }

         if(!context.ApiResources.Any())
         {
            Console.WriteLine("ApiResources being populated");

            foreach(var resource in idpConfigContext.GetApiResources().ToList())
            {
               context.ApiResources.Add(resource.ToEntity());
            }

            context.SaveChanges();
         }
         else
         {
            Console.WriteLine("ApiResources already populated");
         }
      }
   }
}
