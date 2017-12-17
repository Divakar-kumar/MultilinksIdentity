/* Database initialisation steps:
 * 1. Comment out {InitializeDatabase(app)}
 * 2. Open PM Console
 * 3. Select DataService as default project
 * 4. Run {add-migration -context ApplicationDbContext "InitialDatabaseCreation"}
 * 5. Run {add-migration -context PersistedGrantDbContext "InitialIdentityServerPersistedGrantDbMigration" -OutputDir Migrations/IdentityServer/PersistedGrantDb}
 * 6. Run {add-migration -context ConfigurationDbContext "InitialIdentityServerConfigurationDbMigration" -OutputDir Migrations/IdentityServer/ConfigurationDb}
 * 7. Run {update-database -context ApplicationDbContext}
 * 8. Uncomment out {InitializeDatabase(app)}
 * 9. Run {DataService project once to complete database initialisation}
 */

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Multilinks.DataService.Entities;
using System;

namespace Multilinks.DataService
{
   public class ApplicationDbContext : IdentityDbContext<UserEntity, UserRoleEntity, string>
   {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
      {
      }

      public DbSet<EndpointEntity> Endpoints { get; set; }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         base.OnModelCreating(builder);
         // Customize the ASP.NET Identity model and override the defaults if needed.
         // For example, you can rename the ASP.NET Identity table names and more.
         // Add your customizations after calling base.OnModelCreating(builder);
      }
   }
}
