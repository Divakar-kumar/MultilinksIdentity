using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Multilinks.DataService
{
   public class Config
   {
      // scopes define the resources in your system
      public static IEnumerable<IdentityResource> GetIdentityResources()
      {
         return new List<IdentityResource>
         {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
         };
      }

      public static IEnumerable<ApiResource> GetApiResources()
      {
         return new List<ApiResource>
         {
            /* TODO: What about ApiSecrets? */
            new ApiResource("ApiService", "Multilinks API Service"),
            new ApiResource("SpaClientBackend", "Multilinks SPA Client Backend API Service")
         };
      }

      // clients want to access resources (aka scopes)
      public static IEnumerable<Client> GetClients()
      {
         // client credentials client
         return new List<Client>
         {
            new Client
            {
               ClientId = "SpaClient",
               ClientName = "Angular Web Client",
               AllowAccessTokensViaBrowser = true,
               AllowedGrantTypes = GrantTypes.Implicit,

               RedirectUris = { "https://notused" },
               PostLogoutRedirectUris = { "https://notused" },
               FrontChannelLogoutUri = "http://localhost:5000/signout-idsrv", // for testing identityserver on localhost
 
               // scopes that client has access to
               AllowedScopes =
               {
                  IdentityServerConstants.StandardScopes.OpenId,
                  IdentityServerConstants.StandardScopes.Profile,
                  "ApiService",
                  "SpaClientBackend"
               },
            }
         };
      }
   }
}
