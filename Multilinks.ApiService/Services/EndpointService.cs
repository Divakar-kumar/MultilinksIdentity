using System;
using System.Threading;
using System.Threading.Tasks;
using Multilinks.ApiService.Models;
using Multilinks.ApiService.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Collections.Generic;
using AutoMapper.QueryableExtensions;

namespace Multilinks.ApiService.Services
{
   public class EndpointService : IEndpointService
   {
      private readonly ApplicationDbContext _context;

      public EndpointService(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<EndpointViewModel> GetEndpointAsync(Guid id, CancellationToken ct)
      {
         var entity = await _context.Endpoints.SingleOrDefaultAsync(r => r.EndpointId == id, ct);
         if(entity == null) return null;

         return Mapper.Map<EndpointViewModel>(entity);
      }

      public async Task<IEnumerable<EndpointViewModel>> GetEndpointsAsync(CancellationToken ct)
      {
         var query = _context.Endpoints
            .ProjectTo<EndpointViewModel>();

         return await query.ToArrayAsync();
      }
   }
}
