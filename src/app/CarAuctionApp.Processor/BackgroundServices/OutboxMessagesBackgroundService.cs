using CarAuctionApp.Infrastructure.MessageBroker;

namespace CarAuctionApp.Processor.BackgroundServices
{
    internal sealed class OutboxMessagesBackgroundService: BackgroundService
    {
        private readonly ILogger<OutboxMessagesBackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private const int FrequencyInSeconds = 5;

        public OutboxMessagesBackgroundService(ILogger<OutboxMessagesBackgroundService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            this._logger = logger;
            this._serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation($"{nameof(OutboxMessagesBackgroundService)} is starting.");

                while (!stoppingToken.IsCancellationRequested)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    OutboxMessageProcessor outboxMessageProcessor = scope.ServiceProvider.GetRequiredService<OutboxMessageProcessor>();

                    await outboxMessageProcessor.Process(stoppingToken);

                    await Task.Delay(TimeSpan.FromSeconds(FrequencyInSeconds), stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"{nameof(OutboxMessagesBackgroundService)} has been cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error has occured in {nameof(OutboxMessagesBackgroundService)}.");
            }
            finally
            {
                _logger.LogInformation($"{nameof(OutboxMessagesBackgroundService)} has finished.");
            }
        }
    }
}
