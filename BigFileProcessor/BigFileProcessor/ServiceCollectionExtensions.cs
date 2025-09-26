using BigFileProcessor.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BigFileProcessor;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationOptions(this IServiceCollection services)
    {
        services
            .AddOptions<FileProcessingOptions>()
            .BindConfiguration(FileProcessingOptions.SectionName)
            .ValidateOnStart();

        services
            .AddOptions<FileWatcherOptions>()
            .BindConfiguration(FileWatcherOptions.SectionName)
            .ValidateOnStart();
    }
}