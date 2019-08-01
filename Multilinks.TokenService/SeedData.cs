using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Multilinks.TokenService.Entities;
using Multilinks.TokenService.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Multilinks.TokenService
{
   public class SeedData
   {
      public static void EnsureSeedData(IServiceProvider serviceProvider)
      {
         Console.WriteLine("Seeding database...");

         using (var scope = serviceProvider.CreateScope())
         {
            var services = scope.ServiceProvider;
            var config = services.GetRequiredService<Config>();

            InitialiseIdentityServerDbData(services, config);
            InitialiseIdpDbData(services, config);
         }

         Console.WriteLine("Done seeding database.");
         Console.WriteLine();
      }

      private static void InitialiseIdentityServerDbData(IServiceProvider services, Config config)
      {
         services.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

         var context = services.GetRequiredService<ConfigurationDbContext>();
         context.Database.Migrate();

         try
         {
            SeedClients(context, config).Wait();
            SeedIdentityResources(context, config).Wait();
            SeedApiResources(context, config).Wait();
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
         }
      }

      private static async Task SeedClients(ConfigurationDbContext context, Config config)
      {
         var clients = await context.Clients.CountAsync();

         if (clients != 0)
         {
            return;
         }

         foreach (var client in config.GetClients().ToList())
         {
            context.Clients.Add(client.ToEntity());
         }

         var result = await context.SaveChangesAsync();

         if (result < 1)
         {
            throw new ApplicationException("Failed to seed clients.");
         }
      }

      private static async Task SeedIdentityResources(ConfigurationDbContext context, Config config)
      {
         var resources = await context.IdentityResources.CountAsync();

         if (resources != 0)
         {
            return;
         }

         foreach (var resource in config.GetIdentityResources().ToList())
         {
            context.IdentityResources.Add(resource.ToEntity());
         }

         var result = await context.SaveChangesAsync();

         if (result < 1)
         {
            throw new ApplicationException("Failed to seed identity resources.");
         }
      }

      private static async Task SeedApiResources(ConfigurationDbContext context, Config config)
      {
         var resources = await context.ApiResources.CountAsync();

         if (resources != 0)
         {
            return;
         }

         foreach (var resource in config.GetApiResources().ToList())
         {
            context.ApiResources.Add(resource.ToEntity());
         }

         var result = await context.SaveChangesAsync();

         if (result < 1)
         {
            throw new ApplicationException("Failed to seed api resources.");
         }
      }

      private static void InitialiseIdpDbData(IServiceProvider services, Config config)
      {
         var context = services.GetRequiredService<TokenServiceDbContext>();
         var userManager = services.GetRequiredService<UserManager<UserEntity>>();
         context.Database.Migrate();

         try
         {
            SeedSystemOwner(context, config, userManager).Wait();
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
         }
      }

      private static async Task SeedSystemOwner(TokenServiceDbContext context,
         Config config,
         UserManager<UserEntity> userManager)
      {
         var users = await context.Users.CountAsync();

         if (users != 0)
         {
            return;
         }

         var systemOwnerOptions = config.GetSystemOwnerOptions();

         var user = new UserEntity
         {
            UserName = systemOwnerOptions.Email,
            Email = systemOwnerOptions.Email
         };

         var result = await userManager.CreateAsync(user);

         if (!result.Succeeded)
         {
            throw new ApplicationException("Failed to seed system owner.");
         }

         result = userManager.AddClaimsAsync(user, new Claim[] {
            new Claim(JwtClaimTypes.Email, systemOwnerOptions.Email),
            new Claim(JwtClaimTypes.Role, "System Owner"),
            new Claim("RegisteredDateTimeOffsetUtc", DateTimeOffset.UtcNow.ToString())
         }).Result;

         if (!result.Succeeded)
         {
            throw new ApplicationException("Failed to seed system owner.");
         }

         user.PasswordHash = userManager.PasswordHasher.HashPassword(user, systemOwnerOptions.DefaultPassword);
         result = await userManager.UpdateAsync(user);

         if (!result.Succeeded)
         {
            throw new ApplicationException("Failed to seed system owner.");
         }

         result = userManager.AddClaimsAsync(user, new Claim[] {
            new Claim(JwtClaimTypes.GivenName, systemOwnerOptions.FirstName),
            new Claim(JwtClaimTypes.FamilyName, systemOwnerOptions.LastName),
            new Claim(JwtClaimTypes.Name, systemOwnerOptions.FirstName + " " + systemOwnerOptions.LastName),
            new Claim("RegisterConfirmationDateTimeOffsetUtc", DateTimeOffset.UtcNow.ToString())
         }).Result;

         if (!result.Succeeded)
         {
            throw new ApplicationException("Failed to seed system owner.");
         }

         var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
         var confirmationResult = await userManager.ConfirmEmailAsync(user, code);

         if (!confirmationResult.Succeeded)
         {
            throw new ApplicationException("Failed to seed system owner.");
         }
      }
   }
}
