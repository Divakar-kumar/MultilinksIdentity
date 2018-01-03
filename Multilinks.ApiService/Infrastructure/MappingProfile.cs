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
               Link.To(nameof(Controllers.EndpointsController.GetEndpointByIdAsync), new { endpointId = src.EndpointId })))
            .ForMember(dest => dest.SubmitForm, opt => opt.MapFrom(src =>
                  FormMetadata.FromModel(
                     new NewEndpointForm(),
                     Link.ToForm(
                           nameof(EndpointsController.CreateEndpointAsync),
                           null,
                           Link.PostMethod,
                           Form.CreateRelation))));

         CreateMap<UserEntity, UserViewModel>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                    Link.To(nameof(Controllers.UsersController.GetUserByIdAsync), new { userId = src.ApplicationUserId })))
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.ApplicationUserId));
      }
   }
}
