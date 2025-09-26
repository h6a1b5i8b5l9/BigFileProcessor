using BigFileProcessor.Core.Interfaces;
using Microsoft.Extensions.Hosting;

namespace BigFileProcessor;

public class FileWatcher(IBoxImportOrchestrator importOrchestrator, IFileService fileService, IWatchPaths paths)
    : BackgroundService
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private FileSystemWatcher? _watcher;


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var file in Directory.GetFiles(paths.WatchPath)) await ProcessFile(file);

        _watcher = new FileSystemWatcher(paths.WatchPath)
        {
            EnableRaisingEvents = true,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
        };
        _watcher.Created += OnNewFile;
    }

    private async void OnNewFile(object sender, FileSystemEventArgs e)
    {
        await ProcessFile(e.FullPath);
    }

    private async Task ProcessFile(string fileFullPath)
    {
        await _semaphore.WaitAsync();
        try
        {
            await fileService.WaitForFileReadyAsync(fileFullPath);

            await importOrchestrator.ProcessAsync(fileFullPath);

            fileService.MoveFile(fileFullPath, paths.ProcessedPath);
        }
        catch (Exception)
        {
            fileService.MoveFile(fileFullPath, paths.FailedPath);
        }
        finally
        {
            GC.Collect();
            _semaphore.Release();
        }
    }
}