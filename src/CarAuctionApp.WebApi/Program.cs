using CarAuctionApp.WebApi.Extensions;
using CarAuctionApp.Infrastructure.Persistence.Extensions;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using CarAuctionApp.WebApi.Routes;
using CarAuctionApp.WebApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddCors();

string connectionString = builder.Configuration.GetConnectionString("Postgres")!;
builder.Services.AddPersistence(connectionString);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    await app.ApplyMigrationsAsync();
    await app.SeedDevelopmentData();
}

app.UseHttpsRedirection();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.MapAuctionRoutes();
app.MapHub<AuctionHub>("/hubs/auction");

app.Run();

