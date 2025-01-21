using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using CarAuctionApp.Domain.Extensions;
using CarAuctionApp.Persistence.Extensions;
using CarAuctionApp.WebApi.Extensions;
using CarAuctionApp.WebApi.Endpoints;
using CarAuctionApp.WebApi.Hubs;
using CarAuctionApp.Infrastructure.MessageBroker;
using MassTransit;
using Microsoft.Extensions.Options;
using CarAuctionApp.WebApi.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSignalR();
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

builder.Services.AddPersistence(builder.Configuration.GetConnectionString("carauctionapp.postgres")!);
builder.Services.AddDomainServices();
builder.Services.AddApiServices();

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));
builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        IOptions<MessageBrokerSettings> options = context.GetRequiredService<IOptions<MessageBrokerSettings>>();

        configurator.Host(new Uri(options.Value.Host), hostConfigurator =>
        {
            hostConfigurator.Username(options.Value.Username);
            hostConfigurator.Password(options.Value.Password);
        });
    });
});
builder.Services.AddScoped<OutboxMessageProcessor>();
builder.Services.AddHostedService<OutboxMessagesBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
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

