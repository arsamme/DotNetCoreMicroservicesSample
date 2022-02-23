using System.Reflection;
using Identity;
using Identity.Data;
using Identity.Migrations;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true;

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
string connectionString = builder.Configuration.GetConnectionString("IdentityServerDatabase");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddTestUsers(IdentityConfiguration.TestUsers)
    .AddAspNetIdentity<ApplicationUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
            sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
            sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddDeveloperSigningCredential();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}
DatabaseInitializer.PopulateIdentityServer(app);

app.MapGet("/", () => "Hello World!");

app.UseIdentityServer();

app.Run();