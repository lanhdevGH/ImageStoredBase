﻿//using Duende.IdentityServer;
//using Duende.IdentityServer.Models;

//namespace ImageStoreBase.Api.IdentityServer
//{
//    public static class Config
//    {
//        public static IEnumerable<IdentityResource> IdentityResources()
//        {
//            var resources = new List<IdentityResource>()
//            {
//                new IdentityResources.OpenId(),
//                new IdentityResources.Profile()
//            };
//            return resources;
//        }

//        public static IEnumerable<ApiScope> ApiResources()
//        {
//            var apiResources = new List<ApiScope>
//            {
//                new ApiScope("imagestoreapi", "Image Store API")
//            };
//            return apiResources;
//        }

//        public static IEnumerable<Client> Clients()
//        {
//            var clients = new Client[]
//            {
//                new Client
//                {
//                    ClientId = "webportal",
//                    ClientSecrets = { new Secret("secret".Sha256()) },

//                    AllowedGrantTypes = GrantTypes.Code,
//                    RequireConsent = false,
//                    RequirePkce = true,
//                    AllowOfflineAccess = true,

//                    // where to redirect to after login
//                    RedirectUris = { "https://localhost:5002/signin-oidc" },

//                    // where to redirect to after logout
//                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

//                    AllowedScopes = new List<string>
//                    {
//                        IdentityServerConstants.StandardScopes.OpenId,
//                        IdentityServerConstants.StandardScopes.Profile,
//                        IdentityServerConstants.StandardScopes.OfflineAccess,
//                        "api.knowledgespace"
//                    }
//                 },
//                new Client
//                {
//                    ClientId = "swagger",
//                    ClientName = "Swagger Client",

//                    AllowedGrantTypes = GrantTypes.Implicit,
//                    AllowAccessTokensViaBrowser = true,
//                    RequireConsent = false,

//                    RedirectUris =           { "https://localhost:5000/swagger/oauth2-redirect.html" },
//                    PostLogoutRedirectUris = { "https://localhost:5000/swagger/oauth2-redirect.html" },
//                    AllowedCorsOrigins =     { "https://localhost:5000" },

//                    AllowedScopes = new List<string>
//                    {
//                        IdentityServerConstants.StandardScopes.OpenId,
//                        IdentityServerConstants.StandardScopes.Profile,
//                        "api.knowledgespace"
//                    }
//                },
//                new Client
//                {
//                    ClientName = "Angular Admin",
//                    ClientId = "angular_admin",
//                    AccessTokenType = AccessTokenType.Reference,
//                    RequireConsent = false,

//                    RequireClientSecret = false,
//                    AllowedGrantTypes = GrantTypes.Code,
//                    RequirePkce = true,

//                    AllowAccessTokensViaBrowser = true,
//                    RedirectUris = new List<string>
//                    {
//                        "http://localhost:4200",
//                        "http://localhost:4200/authentication/login-callback",
//                        "http://localhost:4200/silent-renew.html"
//                    },
//                    PostLogoutRedirectUris = new List<string>
//                    {
//                        "http://localhost:4200/unauthorized",
//                        "http://localhost:4200/authentication/logout-callback",
//                        "http://localhost:4200"
//                    },
//                    AllowedCorsOrigins = new List<string>
//                    {
//                        "http://localhost:4200"
//                    },
//                    AllowedScopes = new List<string>
//                    {
//                        IdentityServerConstants.StandardScopes.OpenId,
//                        IdentityServerConstants.StandardScopes.Profile,
//                        "api.knowledgespace"
//                    }
//                }
//            };
//            return clients;
//        }
//    }
//}
