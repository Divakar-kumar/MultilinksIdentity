using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Multilinks.TokenService
{
   public class Config
   {
      public static IEnumerable<IdentityResource> GetIdentityResources()
      {
         return new List<IdentityResource>
         {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", "User role(s)", new List<string> { "role" })
         };
      }

      public static IEnumerable<ApiResource> GetApiResources()
      {
         return new List<ApiResource>
         {
            new ApiResource("ApiService", "Multilinks API Service", new List<string> { "role" })
         };
      }

      // clients want to access resources (aka scopes)
      public static IEnumerable<Client> GetClients()
      {
         return new List<Client>
         {
            new Client
            {
               ClientId = "WebConsole",
               ClientName = "Multilinks Web Console",
               AllowAccessTokensViaBrowser = true,
               AllowedGrantTypes = GrantTypes.Implicit,
               RequireConsent = false,
               AccessTokenLifetime = 180,
               AllowedCorsOrigins = { "https://localhost:44302" },

               RedirectUris =
               {
                  "https://localhost:44302/signin-oidc",
                  "https://localhost:44302/redirect-silent-renew"
               },

               PostLogoutRedirectUris =
               {
                  "https://localhost:44302/signout-oidc"
               },
 
               // scopes that client has access to
               AllowedScopes =
               {
                  IdentityServerConstants.StandardScopes.OpenId,
                  IdentityServerConstants.StandardScopes.Profile,
                  "roles",
                  "ApiService"
               }
            }
         };
      }
   }
}
