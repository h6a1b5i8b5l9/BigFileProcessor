using BigFileProcessor;
using BigFileProcessor.Core;
using BigFileProcessor.Core.Interfaces;
using BigFileProcessor.Infrastructure;
using BigFileProcessor.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config => { config.AddJsonFile("appsettings.json", false, true); })
    .ConfigureServices(services =>
    {
        services.AddApplicationOptions();

        services.AddInfrastructureServices();

        services.AddTransient<IBoxImportOrchestrator, BoxImportOrchestrator>();
        services.AddSingleton<IWatchPaths, WatchPaths>();
        services.AddHostedService<FileWatcher>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
await initializer.EnsureTablesExistAsync();

await host.RunAsync();