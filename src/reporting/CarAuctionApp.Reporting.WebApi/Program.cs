using CarAuctionApp.Reporting.Data;
using CarAuctionApp.Reporting.Data.MessageBroker;
using CarAuctionApp.Reporting.WebApi.Consumers;
using MassTransit;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("http://localhost:6010")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


builder.Services.AddSingleton(new ReportingConnectionFactory(builder.Configuration.GetConnectionString("ReportingPostgres")));

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));
builder.Services.AddMassTransit(busConfig =>
{
    busConfig.SetKebabCaseEndpointNameFormatter();

    busConfig.AddConsumer<AuctionBidCreatedEventConsumer>();

    busConfig.UsingRabbitMq((context, config) =>
    {
        IOptions<MessageBrokerSettings> options = context.GetRequiredService<IOptions<MessageBrokerSettings>>();

        config.Host(new Uri(options.Value.Host), hostConfigurator =>
        {
            hostConfigurator.Username(options.Value.Username);
            hostConfigurator.Password(options.Value.Password);
        });

        config.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "Car Auction App Reporting";
    options.Servers = Array.Empty<ScalarServer>();
});

app.UseCors("AllowAll");

app.MapHealthChecks("/health");

app.Run();
