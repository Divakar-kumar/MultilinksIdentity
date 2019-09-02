/* 1. Add-Migration -Name InitialIdentityDbMigration -Context IdentityDbContext -OutputDir Services/Migrations
 * 2. Add-Migration -Name InitialIdentityServerPersistedGrantDbMigration -Context PersistedGrantDbContext -OutputDir Services/Migrations/IdentityServer/PersistedGrantDb
 * 3. Add-Migration -Name InitialIdentityServerConfigurationDbMigration -Context ConfigurationDbContext -OutputDir Services/Migrations/IdentityServer/ConfigurationDb
 */

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Multilinks.Identity.Entities;

namespace Multilinks.Identity.Services
{
   public class IdentityDbContext : IdentityDbContext<UserEntity>
   {
      public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
          : base(options)
      {
      }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         base.OnModelCreating(builder);
      }
   }
}
