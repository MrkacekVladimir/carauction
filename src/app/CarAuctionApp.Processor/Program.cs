using CarAuctionApp.Infrastructure.MessageBroker;
using CarAuctionApp.Processor.BackgroundServices;
using CarAuctionApp.Domain.Extensions;
using CarAuctionApp.Persistence.Extensions;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddHttpContextAccessor();

builder.Services.AddPersistence(builder.Configuration.GetConnectionString("AppPostgres")!);
builder.Services.AddDomainServices();

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));
builder.Services.AddMassTransit(busConfig =>
{
    busConfig.SetKebabCaseEndpointNameFormatter();

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

builder.Services.AddScoped<OutboxMessageProcessor>();
builder.Services.AddHostedService<OutboxMessagesBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.Run();
