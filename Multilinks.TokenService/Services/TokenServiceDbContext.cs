/* 1. Add-Migration -Name InitialIdentityDbMigration -Context TokenServiceDbContext -OutputDir Services/Migrations
 * 2. Add-Migration -Name InitialIdentityServerPersistedGrantDbMigration -Context PersistedGrantDbContext -OutputDir Services/Migrations/IdentityServer/PersistedGrantDb
 * 3. Add-Migration -Name InitialIdentityServerConfigurationDbMigration -Context ConfigurationDbContext -OutputDir Services/Migrations/IdentityServer/ConfigurationDb
 */

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Multilinks.TokenService.Entities;

namespace Multilinks.TokenService.Services
{
   public class TokenServiceDbContext : IdentityDbContext<UserEntity>
   {
      public TokenServiceDbContext(DbContextOptions<TokenServiceDbContext> options)
          : base(options)
      {
      }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         base.OnModelCreating(builder);
      }
   }
}
