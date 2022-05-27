using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace QNA.IdentityServer
{
    /// <summary>
    /// This is the Identity Server configurations file. Use this to register new clients
    /// </summary>
    public class Config
    {

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API"),
                //new ApiResource("questionnaire.webapi", "Questionnaire API")
                new ApiResource("Questionnaire API")
                {
                    ApiSecrets = { new Secret("secret".Sha256()) }
                },
                //new ApiResource("audiencediscovery.api", "Audience Discovery API")
                //{
                //    ApiSecrets = { new Secret("secret".Sha256()) }
                //},
                //new ApiResource("RPWatermarkDashBoard.WebApi", "RPWatermarkDashBoard API")
                //{
                //    ApiSecrets = { new Secret("secret".Sha256()) }
                //},
                //new ApiResource("rpannotationapi", "Annotations API")
                //{
                //    ApiSecrets = { new Secret("secret".Sha256()) }
                //},
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration config)
        {

            return new List<Client>
            {
                //new Client
                //{
                //    ClientId = "client",

                //    // no interactive user, use the clientid/secret for authentication
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,

                //    // secret for authentication
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },

                //    // scopes that client has access to
                //    AllowedScopes = { "api1", "rpannotationapi", "ratingspro.webapi" , "RPWatermarkDashBoard.WebApi", "audiencediscovery.api" }
                //},
                new Client
                {
                    ClientId = "questionnaire.web",
                    ClientName = "Questionnaire Web",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    //RedirectUris = { "http://localhost:5002/signin-oidc/" },
                    //PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc/" },
                    RedirectUris           = { config.GetSection("QNAReUrl").Value.ToString() },
                    PostLogoutRedirectUris = { config.GetSection("QNAPostLogoutReUrl").Value.ToString() },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1",
                        "roles"
                        //"rpannotationapi",
                        //"ratingspro.webapi",
                        //"RPWatermarkDashBoard.WebApi",
                        //"audiencediscovery.api"
                    },
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AlwaysSendClientClaims = true,
                    AccessTokenType = AccessTokenType.Reference,
                    AccessTokenLifetime = Convert.ToInt32(config.GetSection("TokenLifetime").Value), // 24 hour
                    IdentityTokenLifetime = Convert.ToInt32(config.GetSection("TokenLifetime").Value)
                },
                   
                //new Client
                //{
                //    ClientId = "ratingspro.webapi",

                //    // no interactive user, use ratingspro.webapi clientid/secret for authentication
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,

                //    // secret for authentication
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },

                //    // scopes that client has access to
                //    AllowedScopes = { "ratingspro.webapi" }
                //},
                //new Client
                //{
                //    ClientId = "rpannotationapi",

                //    // no interactive user, use the clientid/secret for authentication
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,

                //    // secret for authentication
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },

                //    // scopes that client has access to
                //    AllowedScopes = { "api1", "rpannotationapi" }
                //},
                //new Client
                //{
                //    ClientId = "audiencediscovery.api",

                //    // no interactive user, use ratingspro.webapi clientid/secret for authentication
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,

                //    // secret for authentication
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },

                //    // scopes that client has access to
                //    AllowedScopes = { "audiencediscovery.api" }
                //},
                //new Client
                //{
                //    ClientId = "ratingspro.web",
                //    ClientName = "RatingsPro Web",
                //    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },

                //    RedirectUris           = { config.GetSection("RatingsProReUrl").Value.ToString() },
                //    PostLogoutRedirectUris = { config.GetSection("RatingsProPostLogoutReUrl").Value.ToString() },

                //    AllowedScopes =
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        "api1",
                //        "ratingspro.webapi",
                //        "roles"
                //    },
                //    AllowOfflineAccess = true,
                //    RequireConsent = false,
                //    AllowAccessTokensViaBrowser = true,
                //    AlwaysIncludeUserClaimsInIdToken = true,
                //    AlwaysSendClientClaims = true,
                //    AccessTokenType = AccessTokenType.Reference,
                //    AccessTokenLifetime = Convert.ToInt32(config.GetSection("TokenLifetime").Value), // 24 hour
                //    IdentityTokenLifetime = Convert.ToInt32(config.GetSection("TokenLifetime").Value)
                //},
                //new Client
                //{
                //    ClientId = "rpWatermarkDashBoard.Web",
                //    ClientName = "RPWatermarkDashBoard Web",
                //    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },

                //    RedirectUris           = { config.GetSection("RPWatermarkDashboardReUrl").Value.ToString() },
                //    PostLogoutRedirectUris = { config.GetSection("RPWatermarkDashboardPostLogoutReUrl").Value.ToString() },

                //    AllowedScopes =
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        "api1",
                //        "RPWatermarkDashBoard.WebApi",
                //        "roles"
                //    },
                //    AllowOfflineAccess = true,
                //    RequireConsent = false,
                //    AllowAccessTokensViaBrowser = true,
                //    AlwaysIncludeUserClaimsInIdToken = true,
                //    AlwaysSendClientClaims = true,
                //    AccessTokenType = AccessTokenType.Reference,
                //    AccessTokenLifetime = Convert.ToInt32(config.GetSection("TokenLifetime").Value), // 24 hour
                //    IdentityTokenLifetime = Convert.ToInt32(config.GetSection("TokenLifetime").Value)
                //},
                //new Client
                //{
                //    ClientId = "RPWatermarkDashBoard.WebApi",

                //    // no interactive user, use ratingspro.webapi clientid/secret for authentication
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,

                //    // secret for authentication
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },

                //    // scopes that client has access to
                //    AllowedScopes = { "RPWatermarkDashBoard.WebApi" }
                //},
                //new Client
                //{
                //    ClientId = "mvc.annotation",
                //    ClientName = "Annotation app",
                //    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },

                //    RedirectUris           = { config.GetSection("RPAnnotationReUrl").Value.ToString() },
                //    PostLogoutRedirectUris = { config.GetSection("RPAnnotationProPostLogoutReUrl").Value.ToString() },

                //    AllowedScopes =
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        IdentityServerConstants.StandardScopes.Email,
                //        "api1",
                //        "rpannotationapi",
                //        "roles"
                //    },
                //    AllowOfflineAccess = true,
                //    RequireConsent = false,
                //    AllowAccessTokensViaBrowser = true,
                //    AlwaysIncludeUserClaimsInIdToken = true,
                //    AlwaysSendClientClaims = true,
                //    AccessTokenType = AccessTokenType.Reference,
                //    AccessTokenLifetime = Convert.ToInt32(config.GetSection("TokenLifetime").Value), // 24 hour
                //    IdentityTokenLifetime = Convert.ToInt32(config.GetSection("TokenLifetime").Value)
                //},
                //new Client
                //{
                //    ClientId = "audiencediscovery.web",
                //    ClientName = "Audience Discovery Web",
                //    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    RedirectUris           = { config.GetSection("AudienceDiscoveryReUrl").Value.ToString() },
                //    PostLogoutRedirectUris = { config.GetSection("AudienceDiscoveryPostLogoutUrl").Value.ToString() },
                //    AllowedScopes =
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        "api1",
                //        "audiencediscovery.api",
                //    },
                //    AllowOfflineAccess = true,
                //    RequireConsent = false,
                //    AllowAccessTokensViaBrowser = true,
                //    AlwaysIncludeUserClaimsInIdToken = true,
                //    AlwaysSendClientClaims = true,
                //    AccessTokenType = AccessTokenType.Reference,
                //    UpdateAccessTokenClaimsOnRefresh = true,
                //    AccessTokenLifetime = Convert.ToInt32(config.GetSection("TokenLifetime").Value), // 24 hour
                //    IdentityTokenLifetime = Convert.ToInt32(config.GetSection("TokenLifetime").Value)
                //},
            };
        }

        // hard coded test users
        //public static List<TestUser> GetUsers()
        //{
        //    return new List<TestUser>
        //    {
        //        new TestUser
        //        {
        //            SubjectId = "1",
        //            Username = "alice",
        //            Password = "password",
        //            Claims = new []
        //            {
        //                new Claim("name", "Alice"),
        //                new Claim("website", "https://alice.com")
        //            }
        //        },
        //        new TestUser
        //        {
        //            SubjectId = "2",
        //            Username = "bob",
        //            Password = "password",
        //            Claims = new []
        //            {
        //                new Claim("name", "Bob"),
        //                new Claim("website", "https://bob.com")
        //            }
        //        }
        //    };
        //}
    }
}