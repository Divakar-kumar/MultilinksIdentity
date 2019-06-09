/* 1. Add-Migration -Name InitialApiDbMigration -Context ApiServiceDbContext -OutputDir Services/Migrations
 */

using Microsoft.EntityFrameworkCore;
using Multilinks.ApiService.Entities;

namespace Multilinks.ApiService.Services
{
   public class ApiServiceDbContext : DbContext
   {
      public ApiServiceDbContext(DbContextOptions<ApiServiceDbContext> options)
          : base(options)
      {
      }

      public DbSet<EndpointClientEntity> Clients { get; set; }
      public DbSet<EndpointOwnerEntity> Owners { get; set; }
      public DbSet<EndpointEntity> Endpoints { get; set; }

      public DbSet<HubConnectionEntity> HubConnections { get; set; }
      public DbSet<EndpointLinkEntity> Links { get; set; }

      public DbSet<NotificationEntity> Notifications { get; set; }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         base.OnModelCreating(builder);
      }
   }
}
