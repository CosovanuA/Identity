using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Models;
using System.Linq;
using IdentityServer4.EntityFramework.Mappers;
using static IdentityServer4.IdentityServerConstants;
using Constants.IdentityServer;

namespace Identity.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void SeedIdentityServer(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();

            foreach (var client in IdentityServerDefaults.Clients)
            {
                var item = context.Clients.SingleOrDefault(c => c.ClientName == client.ClientId);

                if (item == null)
                {
                    context.Clients.Add(client.ToEntity());
                }
            }

            foreach (var scope in IdentityServerDefaults.ApiScopes)
            {
                var item = context.ApiScopes.SingleOrDefault(c => c.Name == scope.Name);

                if (item == null)
                {
                    context.ApiScopes.Add(scope.ToEntity());
                }
            }

            foreach (var apiResource in IdentityServerDefaults.ApiResources)
            {
                var item = context.ApiResources.SingleOrDefault(c => c.Name == apiResource.Name);

                if (item == null)
                {
                    context.ApiResources.Add(apiResource.ToEntity());
                }
            }

            context.SaveChanges();
        }
    }

    public class IdentityServerDefaults
    {
        public static List<ApiScope> ApiScopes = new()
        {
            new ApiScope(IdentityServerConstants.Scopes.Identity),
            new ApiScope(StandardScopes.OpenId),
            new ApiScope(StandardScopes.Profile)
        };

        public static List<Client> Clients = new()
        {
            new Client()
            {
                ClientName = "web",
                ClientId = "web",
                Description = "Angular web app client",
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                AccessTokenLifetime = 40,
                AllowOfflineAccess = true,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                ClientSecrets = { new Secret("a85bda2ea9d250e78e7c9edb61cd35eefab95aada26accb5af5043a85e053b33".Sha256()) },
                AllowedScopes = ApiScopes.Select(scope => scope.Name).ToList()
            }
        };

        public static List<ApiResource> ApiResources = new()
        {
            new ApiResource(IdentityServerConstants.API.Identity, "Identity api")
        };
    }
}
