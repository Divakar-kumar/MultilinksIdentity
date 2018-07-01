using IdentityServer4.Models;
using System;
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
            new ApiResource("ApiService", "Multilinks API Service"),
            new ApiResource("SpaClientBackend", "Multilinks SPA Client Backend API Service")
         };
      }

      // clients want to access resources (aka scopes)
      public static IEnumerable<Client> GetClients()
      {
         var spaClientKey = Environment.GetEnvironmentVariable("MDS_MULTILINKS_SPACLIENT_KEY");

         if(spaClientKey == null)
         {
            throw new ApplicationException("Required keys missing.");
         }

         // client credentials client
         return new List<Client>
         {
            new Client
            {
               ClientId = "SpaClient",
 
               /* TODO: Is this the right grant type for an Angular app? */
               AllowedGrantTypes = GrantTypes.ClientCredentials,
 
               // secret for authentication
               ClientSecrets =
               {
                  new Secret(spaClientKey.Sha256())
               },
 
               // scopes that client has access to
               AllowedScopes = { "ApiService", "SpaClientBackend" }
            }
         };
      }
   }
}
