using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Configuration
    {
       
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "rc.scope",
                    UserClaims =
                    {
                        "rc.garndma"
                    }
                }
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
                new ApiResource("ApiOne"),
                new ApiResource("ApiTwo", new string[] { "rc.api.garndma" }),
            };

        public static IEnumerable<Client> GetClients(IConfiguration config) =>
            new List<Client> {
                new Client {
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes = { "ApiOne" }
                },
                new Client {
                    ClientId = "client_id_mvc",
                    ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    //RequirePkce = true,
                    
                    RedirectUris = { $"{config.GetSection("Servers").GetSection("MvcClient").Value}/signin-oidc" },
                    PostLogoutRedirectUris = { $"{config.GetSection("Servers").GetSection("MvcClient").Value}/Home/Index" },

                    AllowedScopes = {
                        "ApiOne",
                        "ApiTwo",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "rc.scope",
                    },

                    // puts all the claims in the id token
                    //AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                },
                new Client {
                    ClientId = "client_id_js",

                    AllowedGrantTypes = GrantTypes.Code,
                    //RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { $"{config.GetSection("Servers").GetSection("JavascriptClient").Value}/home/signin" },
                    PostLogoutRedirectUris = { $"{config.GetSection("Servers").GetSection("JavascriptClient").Value}/Home/Index" },
                    AllowedCorsOrigins = { $"{config.GetSection("Servers").GetSection("JavascriptClient").Value}" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ApiOne",
                        "ApiTwo",
                        "rc.scope",
                    },

                    AccessTokenLifetime = 1,

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },

                new Client {
                    ClientId = "angular",

                    AllowedGrantTypes = GrantTypes.Code,
                    //RequirePkce = true,
                    RequireClientSecret = false,

                    //RedirectUris = { "http://localhost:4200" },
                    //PostLogoutRedirectUris = { "http://localhost:4200" },
                    //AllowedCorsOrigins = { "http://localhost:4200" },
                    
                    RedirectUris = { "http://localhost:81/angularclient/" },
                    PostLogoutRedirectUris = { "http://localhost:81/angularclient/" },
                    AllowedCorsOrigins = { "http://localhost:81/angularclient/" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ApiOne",
                    },

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },

                new Client {
                    ClientId = "wpf",

                    AllowedGrantTypes = GrantTypes.Code,
                    //RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "http://localhost/sample-wpf-app" },
                    AllowedCorsOrigins = { "http://localhost" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ApiOne",
                    },

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },
                new Client {
                    ClientId = "xamarin",

                    AllowedGrantTypes = GrantTypes.Code,
                    //RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "xamarinformsclients://callback" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ApiOne",
                    },

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },
		        new Client {
                    ClientId = "flutter",

                    AllowedGrantTypes = GrantTypes.Code,
                    //RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "http://localhost:4000/" },
                    AllowedCorsOrigins = { "http://localhost:4000" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ApiOne",
                    },

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },

            };
    }
}
