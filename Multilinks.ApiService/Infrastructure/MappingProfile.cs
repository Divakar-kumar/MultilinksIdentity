using AutoMapper;
using Multilinks.ApiService.Controllers;
using Multilinks.ApiService.Models;
using Multilinks.DataService.Entities;

namespace Multilinks.ApiService.Infrastructure
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<EndpointEntity, EndpointViewModel>()
            .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
               Link.To(nameof(Controllers.EndpointsController.GetEndpointByIdAsync), new { endpointId = src.EndpointId })));

         CreateMap<UserEntity, UserViewModel>();
      }
   }
}
