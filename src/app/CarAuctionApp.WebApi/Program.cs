using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using CarAuctionApp.Domain.Extensions;
using CarAuctionApp.Persistence.Extensions;
using CarAuctionApp.WebApi.Extensions;
using CarAuctionApp.WebApi.Endpoints;
using CarAuctionApp.WebApi.Hubs;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;
using Serilog;
using CarAuctionApp.Application.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();
builder.Services.AddAuthentication();
builder.Services.AddOpenApi(options =>
{
    //options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddSignalR()
    .AddStackExchangeRedis(builder.Configuration.GetConnectionString("Redis")!, options =>
    {
        options.Configuration.ChannelPrefix = RedisChannel.Literal("WebApi");
    });

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

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("WebApi"))
    .WithMetrics(metrics =>
    {
        metrics.AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation();

        metrics.AddOtlpExporter();
    })
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation();

        tracing.AddOtlpExporter();
    });

builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

builder.Services.AddHttpContextAccessor();
builder.Services.AddPersistence(builder.Configuration.GetConnectionString("Postgres")!);
builder.Services.AddApplicationServices();
builder.Services.AddDomainServices();
builder.Services.AddApiServices();


var app = builder.Build();

app.UseExceptionHandlerWithFallback();

//TODO: For production purposes it will be better to apply migrations to the database manually
bool shouldApplyMigrations = app.Environment.IsDevelopment() || app.Environment.IsProduction();
if (shouldApplyMigrations)
{
    await app.ApplyMigrationsAsync();
    await app.SeedDevelopmentData();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "Car Auction App";
    options.Servers = Array.Empty<ScalarServer>();

    //options.WithPreferredScheme(JwtBearerDefaults.AuthenticationScheme)
    //    .WithHttpBearerAuthentication(bearer =>
    //    {
    //        bearer.Token = "your-bearer-token";
    //    });
});

app.UseCors("AllowAll");

app.MapHealthChecks("/health");
app.MapAuctionsEndpoints();
app.MapHub<AuctionHub>("/hubs/auction");

app.Run();

