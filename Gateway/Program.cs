using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add config files to the container
builder.Configuration.AddJsonFile("ocelot.json");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authenticationProviderKey = "Bearer";
builder.Services.AddAuthentication()
    .AddJwtBearer(authenticationProviderKey, options =>
    {
        options.Authority = "https://localhost:7000";
        options.RequireHttpsMetadata = false;
        options.Audience = "myApi";
    });

builder.Services.AddOcelot();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.UseOcelot().Wait();

app.Run();