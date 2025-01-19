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
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();

string connectionString = builder.Configuration.GetConnectionString("carauctionapp.postgres")!;
builder.Services.AddPersistence(connectionString);
builder.Services.AddDomainServices();
builder.Services.AddApiServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrationsAsync();
    await app.SeedDevelopmentData();
}

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "Car Auction App";
    options.Servers = Array.Empty<ScalarServer>();
});

app.UseHttpsRedirection();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.MapAuctionEndpoints();
app.MapHub<AuctionHub>("/hubs/auction");

app.Run();

