using AutoMapper;
using Multilinks.ApiService.Entities;
using Multilinks.ApiService.Models;

namespace Multilinks.ApiService.Infrastructure
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<EndpointEntity, EndpointViewModel>()
            .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
               Link.To(nameof(Controllers.EndpointsController.GetEndpointByIdAsync), new { endpointId = src.EndpointId })));

         CreateMap<EndpointViewModel, UpdateEndpointForm>().ReverseMap();
      }
   }
}
