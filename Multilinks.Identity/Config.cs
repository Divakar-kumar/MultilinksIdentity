﻿using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Options;
using Multilinks.Identity.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Multilinks.Identity
{
   public class Config
   {
      private readonly MultilinksCoreConfigOptions _coreConfig;
      private readonly PortalConfigOptions _portalConfig;
      private readonly SystemOwnerOptions _systemOwnerOptions;

      public Config(IOptions<MultilinksCoreConfigOptions> coreConfig,
                    IOptions<PortalConfigOptions> portalConfig,
                    IOptions<SystemOwnerOptions> systemOwnerOptions)
      {
         _coreConfig = coreConfig.Value;
         _portalConfig = portalConfig.Value;
         _systemOwnerOptions = systemOwnerOptions.Value;
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
            new ApiResource(_coreConfig.Name,
                            _coreConfig.DisplayName,
                            new List<string> { JwtClaimTypes.Role })
         };
      }

      public IEnumerable<Client> GetClients()
      {
         return new List<Client>
         {
            new Client
            {
               ClientId = "54b8b2f7-cb54-44c0-905d-467522c7988f",
               ClientName = "Multilinks Portal",
               AllowAccessTokensViaBrowser = true,
               AllowedGrantTypes = GrantTypes.Code,
               RequirePkce = true,
               RequireClientSecret = false,
               RequireConsent = false,
               AccessTokenLifetime = 600,
               AllowedCorsOrigins =
               {
                  _portalConfig.AllowedCorsOriginsIdp,
                  _portalConfig.AllowedCorsOriginsApi
               },

               RedirectUris =
               {
                  _portalConfig.LoginRedirectUri,
                  _portalConfig.SilentLoginRedirectUri
               },

               PostLogoutRedirectUris =
               {
                  _portalConfig.LogoutRedirectUri
               },
 
               // scopes that client has access to
               AllowedScopes =
               {
                  IdentityServerConstants.StandardScopes.OpenId,
                  IdentityServerConstants.StandardScopes.Profile,
                  "roles",
                  _coreConfig.Name
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

      public SystemOwnerOptions GetSystemOwnerOptions()
      {
         return _systemOwnerOptions;
      }
   }
}
