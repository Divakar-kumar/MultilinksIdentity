using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Options;
using Multilinks.TokenService.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Multilinks.TokenService
{
   public class Config
   {
      private readonly ApiServiceOptions _defaultApiServiceOptions;
      private readonly WebConsoleClientOptions _webConsoleClientOptions;

      public Config(IOptions<ApiServiceOptions> defaultApiServiceOptions,
                    IOptions<WebConsoleClientOptions> webConsoleClientOptions)
      {
         _defaultApiServiceOptions = defaultApiServiceOptions.Value;
         _webConsoleClientOptions = webConsoleClientOptions.Value;
      }

      public IEnumerable<IdentityResource> GetIdentityResources()
      {
         return new List<IdentityResource>
         {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", "User role(s)", new List<string> { JwtClaimTypes.Role })
         };
      }

      public IEnumerable<ApiResource> GetApiResources()
      {
         return new List<ApiResource>
         {
            new ApiResource(_defaultApiServiceOptions.Name,
                            _defaultApiServiceOptions.DisplayName,
                            new List<string> { JwtClaimTypes.Role })
         };
      }

      public IEnumerable<Client> GetClients()
      {
         return new List<Client>
         {
            new Client
            {
               ClientId = "WebConsole",
               ClientName = "Multilinks Web Console",
               AllowAccessTokensViaBrowser = true,
               AllowedGrantTypes = GrantTypes.Code,
               RequirePkce = true,
               RequireClientSecret = false,
               RequireConsent = false,
               AccessTokenLifetime = 1800,
               AllowedCorsOrigins =
               {
                  _webConsoleClientOptions.AllowedCorsOriginsIdp,
                  _webConsoleClientOptions.AllowedCorsOriginsApi
               },

               RedirectUris =
               {
                  _webConsoleClientOptions.LoginRedirectUri,
                  _webConsoleClientOptions.SilentLoginRedirectUri
               },

               PostLogoutRedirectUris =
               {
                  _webConsoleClientOptions.LogoutRedirectUri
               },
 
               // scopes that client has access to
               AllowedScopes =
               {
                  IdentityServerConstants.StandardScopes.OpenId,
                  IdentityServerConstants.StandardScopes.Profile,
                  "roles",
                  "ApiService"
               },

               /* Custom claims to include with access token for this client. */
               Claims = new List<Claim>
               {
                  new Claim("Type", "SPA_CLIENT")
               },

               AlwaysSendClientClaims = true
            }
         };
      }
   }
}
