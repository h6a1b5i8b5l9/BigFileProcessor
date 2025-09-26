using Microsoft.Extensions.Options;

namespace BigFileProcessor;

public class WatchPaths : IWatchPaths
{
    public WatchPaths(IOptions<FileWatcherOptions> options)
    {
        var optionsValue = options.Value;
        var baseDir = string.IsNullOrWhiteSpace(optionsValue.BaseDirectory)
            ? AppContext.BaseDirectory
            : optionsValue.BaseDirectory;

        WatchPath = Path.Combine(baseDir, optionsValue.WatchFolderName);
        ProcessedPath = Path.Combine(WatchPath, optionsValue.ProcessedFolderName);
        FailedPath = Path.Combine(WatchPath, optionsValue.FailedFolderName);

        EnsureDirectoriesExist();
    }

    public string WatchPath { get; }
    public string ProcessedPath { get; }
    public string FailedPath { get; }

    private void EnsureDirectoriesExist()
    {
        Directory.CreateDirectory(WatchPath);
        Directory.CreateDirectory(ProcessedPath);
        Directory.CreateDirectory(FailedPath);
    }
}