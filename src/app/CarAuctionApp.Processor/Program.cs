using CarAuctionApp.Infrastructure.MessageBroker;
using CarAuctionApp.Processor.BackgroundServices;
using CarAuctionApp.Domain.Extensions;
using CarAuctionApp.Persistence.Extensions;
using MassTransit;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;
using CarAuctionApp.Application.Extensions;
using CarAuctionApp.Application.Authentication;
using CarAuctionApp.Processor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddHealthChecks();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplicationServices();
builder.Services.AddSingleton<ICurrentUserProvider, NullCurrentUserProvider>();
builder.Services.AddPersistence(builder.Configuration.GetConnectionString("Postgres")!);
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
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("Processor"))
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
            .AddEntityFrameworkCoreInstrumentation()
            .AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);

        tracing.AddOtlpExporter();
    });

builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

builder.Services.AddScoped<OutboxMessageProcessor>();
builder.Services.AddHostedService<OutboxMessagesBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.MapHealthChecks("/health");

app.Run();
