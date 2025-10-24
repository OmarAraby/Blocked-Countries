
using BlockedCountries.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace BlockedCountries.Application.BackgroundServices
{
    public class TemporalBlockCleanupService : BackgroundService
    {
        private readonly ITemporalBlocksRepository _tempRepo;
        private readonly ILogger<TemporalBlockCleanupService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

        public TemporalBlockCleanupService(ITemporalBlocksRepository temporalBlocksRepository,ILogger<TemporalBlockCleanupService> logger)
        {
            _tempRepo = temporalBlocksRepository;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Clean Up Service Start");

            using var timer = new PeriodicTimer(_interval);

            await CleanUp();
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await CleanUp();
            }

            _logger.LogInformation("TemporalBlockCleanupService stopped.");

        }

        private async Task CleanUp()
        {
            try
            {
                _logger.LogInformation("Cleaning up expired temporal blocks at {time}", DateTime.UtcNow);
                _tempRepo.RemoveExpired();
                _logger.LogInformation("Cleanup finished at {time}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while cleaning up expired temporal blocks.");
            }
            await Task.CompletedTask;
        }

    }
}
