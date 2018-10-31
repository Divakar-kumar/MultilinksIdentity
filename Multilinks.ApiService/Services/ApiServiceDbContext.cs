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

      public DbSet<HubConnectionsEntity> HubConnections { get; set; }
      public DbSet<EndpointEntity> Endpoints { get; set; }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         builder.Entity<EndpointEntity>()
               .HasIndex(u => u.EndpointId)
               .IsUnique();
         base.OnModelCreating(builder);
      }
   }
}
