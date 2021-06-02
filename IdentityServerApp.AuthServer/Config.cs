using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                }
            };

        }
    }
}
