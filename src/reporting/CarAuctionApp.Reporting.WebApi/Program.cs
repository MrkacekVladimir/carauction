using CarAuctionApp.Reporting.Infrastructure.MessageBroker;
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
            .WithOrigins("http://localhost:5010")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

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

        configurator.ConfigureEndpoints(context);
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
