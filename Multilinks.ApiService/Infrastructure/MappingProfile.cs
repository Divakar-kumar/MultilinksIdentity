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
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.OwnerName))
            .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
               Link.To(nameof(Controllers.EndpointsController.GetEndpointByIdAsync), new { endpointId = src.EndpointId })));

         CreateMap<EndpointViewModel, UpdateEndpointForm>().ReverseMap();

         CreateMap<EndpointLinkEntity, EndpointLinkViewModel>()
            .ForMember(dest => dest.AssociatedDeviceName, opt => opt.MapFrom(src => src.AssociatedEndpoint.Name))
            .ForMember(dest => dest.AssociatedDeviceOwnerName, opt => opt.MapFrom(src => src.AssociatedEndpoint.Owner.OwnerName))
            .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
               Link.To(nameof(Controllers.EndpointLinksController.GetEndpointLinkByIdAsync), new { linkId = src.LinkId })));
      }
   }
}
