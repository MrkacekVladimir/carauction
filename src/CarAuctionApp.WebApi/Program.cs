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

string connectionString = builder.Configuration.GetConnectionString("Postgres")!;
builder.Services.AddPersistence(connectionString);
builder.Services.AddDomainServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrationsAsync();
    await app.SeedDevelopmentData();
}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.MapAuctionEndpoints();
app.MapHub<AuctionHub>("/hubs/auction");

app.Run();

