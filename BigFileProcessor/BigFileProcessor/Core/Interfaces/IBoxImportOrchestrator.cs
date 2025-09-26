namespace BigFileProcessor.Core.Interfaces;

public interface IBoxImportOrchestrator
{
    Task ProcessAsync(
        string inputFile,
        CancellationToken cancellationToken = default);
}