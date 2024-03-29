﻿using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

public static class Config
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new IdentityResource[]
        {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()

        };
    }

    public static IEnumerable<ApiResource> GetApis()
    {
        return new ApiResource[]
        {
                new ApiResource("BotWebAPI")
        };
    }

    public static IEnumerable<Client> GetClients()
    {
        return new[]
        {
                // client credentials flow client
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                    AllowedScopes = { "BotWebAPI" }
                },

                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    RedirectUris = { "http://localhost:5001/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5001/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "BotWebAPI" }
                },

                // SPA client using implicit flow
                new Client
                {
                    ClientId = "spa",
                    ClientName = "FlowGraph Web",
                    ClientUri = "https://localhost:4200",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris =
                    {
                        "https://localhost:4200/callback",
                    },
                    PostLogoutRedirectUris = { "https://localhost:4200/callback" },
                    AllowedCorsOrigins = { "https://localhost:4200" },
                    AllowedScopes = 
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "BotWebAPI"

                    }
                }
            };
    }
}
