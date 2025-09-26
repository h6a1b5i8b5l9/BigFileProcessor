namespace BigFileProcessor;

public interface IWatchPaths
{
    string WatchPath { get; }
    string ProcessedPath { get; }
    string FailedPath { get; }
}