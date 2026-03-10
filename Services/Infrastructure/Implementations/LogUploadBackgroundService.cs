using Hei_Hei_Api.Services.Infrastructure.Abstractions;

namespace Hei_Hei_Api.Services.Infrastructure.Implementations;

public class LogUploadBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public LogUploadBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerService>();

            await logger.UploadTodayLogToS3Async();

            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}
