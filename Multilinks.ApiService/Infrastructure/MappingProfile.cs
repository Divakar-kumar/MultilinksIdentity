using AutoMapper;
using Multilinks.ApiService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Multilinks.ApiService.Infrastructure
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<EndpointEntity, EndpointViewModel>()
            .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
               Link.To(nameof(Controllers.EndpointsController.GetEndpointByIdAsync), new { endpointId = src.EndpointId })));
      }
   }
}
