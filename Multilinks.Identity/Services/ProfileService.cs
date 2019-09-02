using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Multilinks.Identity.Entities;

namespace Multilinks.Identity.Services
{
   public class ProfileService : IProfileService
   {
      private readonly IUserClaimsPrincipalFactory<UserEntity> _claimsFactory;
      private readonly UserManager<UserEntity> _userManager;

      public ProfileService(UserManager<UserEntity> userManager, IUserClaimsPrincipalFactory<UserEntity> claimsFactory)
      {
         _userManager = userManager;
         _claimsFactory = claimsFactory;
      }

      public async Task GetProfileDataAsync(ProfileDataRequestContext context)
      {
         var user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());
         var principal = await _claimsFactory.CreateAsync(user);

         var claims = principal.Claims.ToList();
         claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

         claims.Add(new Claim(JwtClaimTypes.Name, principal.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value));

         context.IssuedClaims = claims;
      }

      public async Task IsActiveAsync(IsActiveContext context)
      {
         var user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());
         context.IsActive = user != null;
      }
   }
}
