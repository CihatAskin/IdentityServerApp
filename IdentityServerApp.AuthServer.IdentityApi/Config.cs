// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerApp.AuthServer.IdentityApi
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("api_1"){
                    Scopes={ "api_1_read", "api_1_write", "api_1_update" },
                    ApiSecrets= new[]{new Secret("secret_api_1".Sha256()) }
                },
                new ApiResource("api_2"){
                    Scopes={ "api_2_read", "api_2_write", "api_2_update" },
                    ApiSecrets= new[]{new Secret("secret_api_2".Sha256()) }
                },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };
        }
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>()
            {
                new ApiScope("api_1_read","Read Auth For API1"),
                new ApiScope("api_1_write","Write Auth For API1"),
                new ApiScope("api_1_update","Update Auth For API1"),
                new ApiScope("api_2_read","Read Auth For API2"),
                new ApiScope("api_2_write","Write Auth For API2"),
                new ApiScope("api_2_update","Update Auth For API2"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)

            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {

            return new List<IdentityResource>() {

                new IdentityResources.OpenId(),//subId
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource(){ Name="CountryAndCity",DisplayName="Country And City",Description="User's Country and City",
                UserClaims= new []{ "country","city"} },
                new IdentityResource(){ Name="Roles",DisplayName="Roles",Description="User's Roles",UserClaims= new[]{"role" } }

            };
        }
       
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>() {
                new Client()
                {
                    ClientId="Client1",
                    ClientName="Client1 APP",
                    ClientSecrets=new[] {new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes= { "api_1_read", "api_1_write", "api_1_update" }
                },
                 new Client()
                {
                    ClientId="Client2",
                    ClientName="Client2 APP",
                    ClientSecrets=new[] {new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes= { "api_1_read"}
                },
                 new Client()
                {
                    ClientId="Client1-Mvc",
                    ClientName="Client1 APP",
                    RequirePkce=false,
                    ClientSecrets=new[] {new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris= new List<string>(){ "https://localhost:5003/signin-oidc"},
                    PostLogoutRedirectUris= { "https://localhost:5003/signout-callback-oidc" },
                    AllowedScopes= {IdentityServerConstants.StandardScopes.OpenId,
                                    IdentityServerConstants.StandardScopes.Profile,
                                    IdentityServerConstants.StandardScopes.Email,
                                    "api_1_read",
                                    IdentityServerConstants.StandardScopes.OfflineAccess,
                                    "CountryAndCity",
                                    "Roles"},
                    AccessTokenLifetime=2*60*60,
                    AllowOfflineAccess=true,//for refresh token
                    RefreshTokenUsage=TokenUsage.ReUse,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=60*24*60*60,
                    RequireConsent=true
                },
                 new Client()
                {
                    ClientId="Client2-Mvc",
                    ClientName="Client2 APP",
                    RequirePkce=false,
                    ClientSecrets=new[] {new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,//for code id_token 
                    RedirectUris= new List<string>(){ "https://localhost:5005/signin-oidc"},
                    PostLogoutRedirectUris= { "https://localhost:5005/signout-callback-oidc" },
                    AllowedScopes= {IdentityServerConstants.StandardScopes.OpenId,
                                    IdentityServerConstants.StandardScopes.Profile,
                                    "api_2_read",
                                    IdentityServerConstants.StandardScopes.OfflineAccess,
                                    "CountryAndCity",
                                    "Roles"},
                    AccessTokenLifetime=2*60*60,
                    AllowOfflineAccess=true,//for refresh token
                    RefreshTokenUsage=TokenUsage.ReUse,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=60*24*60*60,
                    RequireConsent=true
                },

                 new Client(){
                    ClientId="js-client",
                    ClientName="Js Client (Angular)",
                    RequireClientSecret=false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris= { "http://localhost:4200/callback"},
                    AllowedCorsOrigins={ "http://localhost:4200"},
                    PostLogoutRedirectUris={ "http://localhost:4200"},
                    AllowedScopes= {IdentityServerConstants.StandardScopes.OpenId,
                                    IdentityServerConstants.StandardScopes.Profile,
                                    IdentityServerConstants.StandardScopes.Email,
                                    "api_1_read",
                                    IdentityServerConstants.StandardScopes.OfflineAccess,
                                    "CountryAndCity",
                                    "Roles"},
                 },
                 new Client()
                 {
                    ClientId="Client1-ResourceOwner-Mvc",
                    ClientName="Client1 APP MVC",
                    ClientSecrets=new[] {new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes= {IdentityServerConstants.StandardScopes.OpenId,
                                    IdentityServerConstants.StandardScopes.Profile,
                                    IdentityServerConstants.StandardScopes.Email,
                                    "api_1_read",
                                    IdentityServerConstants.StandardScopes.OfflineAccess,
                                    "CountryAndCity",
                                    "Roles",
                                    IdentityServerConstants.LocalApi.ScopeName},
                    AccessTokenLifetime=2*60*60,
                    AllowOfflineAccess=true,//for refresh token
                    RefreshTokenUsage=TokenUsage.ReUse,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=60*24*60*60,
                 }

            };

        }

    }
}