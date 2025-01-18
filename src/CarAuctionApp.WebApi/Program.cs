using CarAuctionApp.WebApi.Extensions;
using CarAuctionApp.Infrastructure.Persistence.Extensions;
using CarAuctionApp.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

string connectionString = builder.Configuration.GetConnectionString("Postgres")!;
builder.Services.AddPersistence(connectionString);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    await app.ApplyMigrationsAsync();
}

app.UseHttpsRedirection();

app.MapGet("/auctions", async (AuctionDbContext dbContext) =>
{
    var auctions = await dbContext.Auctions.ToListAsync();
    return Results.Json(auctions);
}).WithName("GetAuctions");

app.Run();

