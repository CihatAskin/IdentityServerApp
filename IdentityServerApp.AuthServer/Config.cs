using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerApp.AuthServer
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
                }
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
                new ApiScope("api_2_update","Update Auth For API2")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {

            return new List<IdentityResource>() {

                new IdentityResources.OpenId(),//subId
                new IdentityResources.Profile()
            };
        }
        public static List<TestUser> GetUsers()
        {

            return new List<TestUser>() {

                new TestUser
                {
                    SubjectId="1",Username="Adem",Password="male123",
                    Claims= new List<Claim>(){
                                new Claim("given_name","Ersin"),
                                new Claim("family_name","Mufit")
                    }
                },  new TestUser
                {
                    SubjectId="2",Username="Havva",Password="female123",
                    Claims= new List<Claim>(){
                                new Claim("given_name","pempe"),
                                new Claim("family_name","Mufit")
                    }
                },
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
                                    "api_1_read",
                                    IdentityServerConstants.StandardScopes.OfflineAccess},
                    AccessTokenLifetime=2*60*60,
                    AllowOfflineAccess=true,//for refresh token
                    RefreshTokenUsage=TokenUsage.ReUse,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=60*24*60*60
                }
            };

        }

    }
}
