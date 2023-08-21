using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SS14.ServerHub.Shared;
using SS14.ServerHub.Shared.Data;

namespace SS14.ServerHub;

/// <summary>
/// Periodicially prune the database.
/// </summary>
/// <remarks>
/// Patterned off of https://stackoverflow.com/a/63926118
/// </remarks>
public class PeriodicPruneService : IHostedService, IDisposable
{
    private readonly ILogger<PeriodicPruneService> logger;
    private readonly IServiceScopeFactory scopeFactory;

    private Timer? timer;
    private const int PRUNE_INTERVAL_SECONDS = 6 * 60 * 60;  // 6 hrs

    public PeriodicPruneService(ILogger<PeriodicPruneService> logger, IServiceScopeFactory scopeFactory)
        //, HubDbContext dbContext)
    {
        this.logger = logger;
        this.scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Periodic Prune Service running.");

        timer = new Timer(DoWork, null, TimeSpan.Zero, 
            TimeSpan.FromSeconds(PRUNE_INTERVAL_SECONDS));

        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        logger.LogInformation("Periodic Prune Service - start pruning old data");

        using (var scope = scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<HubDbContext>();
            DataPrune.PruneServerStatusArchive(dbContext);
        }

        logger.LogInformation("Periodic Prune Service - prune complete.");
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Periodic Prune Service is stopping.");

        timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer?.Dispose();
    }
}