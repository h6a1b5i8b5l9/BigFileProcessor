namespace BigFileProcessor;

public class FileWatcherOptions
{
    public const string SectionName = "FileWatcher";

    public string? BaseDirectory { get; init; }

    public required string WatchFolderName { get; init; }
    public required string ProcessedFolderName { get; init; }
    public required string FailedFolderName { get; init; }
}