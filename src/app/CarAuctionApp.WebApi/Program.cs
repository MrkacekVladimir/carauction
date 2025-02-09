using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using CarAuctionApp.Domain.Extensions;
using CarAuctionApp.Persistence.Extensions;
using CarAuctionApp.WebApi.Extensions;
using CarAuctionApp.WebApi.Endpoints;
using CarAuctionApp.WebApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddHealthChecks();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("http://localhost:5010")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddPersistence(builder.Configuration.GetConnectionString("AppPostgres")!);
builder.Services.AddDomainServices();
builder.Services.AddApiServices();


var app = builder.Build();

//TODO: For production purposes it will be better to apply migrations to the database manually
bool shouldApplyMigrations = app.Environment.IsDevelopment() || app.Environment.IsProduction();
if (shouldApplyMigrations)
{
    await app.ApplyMigrationsAsync();
    await app.SeedDevelopmentData();
}

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "Car Auction App";
    options.Servers = Array.Empty<ScalarServer>();
});

app.UseCors("AllowAll");

app.MapHealthChecks("/health");
app.MapAuctionEndpoints();
app.MapHub<AuctionHub>("/hubs/auction");

app.Run();

