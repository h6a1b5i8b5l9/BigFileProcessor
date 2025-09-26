using BigFileProcessor.Core.Interfaces;
using BigFileProcessor.Infrastructure.Database;
using BigFileProcessor.Infrastructure.FileSystem;
using BigFileProcessor.Infrastructure.FileSystem.FileParsing;
using BigFileProcessor.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BigFileProcessor.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IBatchSaver, BatchSaver>();
        services.AddTransient<IBoxBatcher, BoxBatcher>();
        services.AddDatabaseServices();
        services.AddFileSystemServices();
    }

    private static void AddDatabaseServices(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddTransient<IBoxRepository, BoxRepository>();
        services.AddTransient<IBulkInsertHelper, BulkInsertHelper>();
    }

    private static void AddFileSystemServices(this IServiceCollection services)
    {
        services.AddTransient<ICheckpointManager, CheckpointManager>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IParsedLineSource, FileParsedLineSource>();
    }
}