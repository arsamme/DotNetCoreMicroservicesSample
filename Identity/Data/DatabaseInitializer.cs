using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data;

public static class DatabaseInitializer
{
    public static void PopulateIdentityServer(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();
        serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        context.Database.Migrate();

        if (!context.Clients.Any())
        {
            foreach (var client in IdentityConfiguration.Clients)
            {
                context.Clients.Add(client.ToEntity());
            }

            context.SaveChanges();
        }

        if (!context.IdentityResources.Any())
        {
            foreach (var resource in IdentityConfiguration.IdentityResources)
            {
                context.IdentityResources.Add(resource.ToEntity());
            }

            context.SaveChanges();
        }

        if (!context.ApiScopes.Any())
        {
            foreach (var resource in IdentityConfiguration.ApiScopes)
            {
                context.ApiScopes.Add(resource.ToEntity());
            }

            context.SaveChanges();
        }
    }
}