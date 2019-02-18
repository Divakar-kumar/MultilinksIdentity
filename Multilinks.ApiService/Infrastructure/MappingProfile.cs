using AutoMapper;
using Multilinks.ApiService.Entities;
using Multilinks.ApiService.Models;

namespace Multilinks.ApiService.Infrastructure
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<EndpointEntity, EndpointViewModel>();

         CreateMap<EndpointViewModel, UpdateEndpointForm>().ReverseMap();

         CreateMap<EndpointLinkEntity, EndpointLinkViewModel>();
      }
   }
}
