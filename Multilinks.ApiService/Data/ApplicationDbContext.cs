using Microsoft.EntityFrameworkCore;
using Multilinks.ApiService.Models;

namespace Multilinks.ApiService.Data
{
   public class ApplicationDbContext : DbContext
   {
      public ApplicationDbContext(DbContextOptions options)
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
