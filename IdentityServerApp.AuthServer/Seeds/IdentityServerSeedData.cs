using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerApp.AuthServer.Seeds
{
    public static class IdentityServerSeedData
    {
        public static void Seed(ConfigurationDbContext context)
        {

            if (!context.Clients.Any())
            {
                Config.GetClients().ToList().ForEach(client =>
                context.Clients.Add(client.ToEntity()));
            }
            if (!context.ApiResources.Any())
            {
                Config.GetApiResources().ToList().ForEach(apiResourse =>
                context.ApiResources.Add(apiResourse.ToEntity()));
            }
            if (!context.ApiScopes.Any())
            {
                Config.GetApiScopes().ToList().ForEach(apiScope =>
                context.ApiScopes.Add(apiScope.ToEntity()));
            }

            if (!context.IdentityResources.Any())
            {
                Config.GetIdentityResources().ToList().ForEach(apiScope =>
                context.IdentityResources.Add(apiScope.ToEntity()));
            }

            context.SaveChanges();
        }
    }
}
