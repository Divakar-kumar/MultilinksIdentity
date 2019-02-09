using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Services
{
   public class LinkService : ILinkService
   {
      private readonly ApiServiceDbContext _context;

      public LinkService(ApiServiceDbContext context)
      {
         _context = context;
      }
   }
}
