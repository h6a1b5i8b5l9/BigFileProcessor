namespace BigFileProcessor.Core.Interfaces;

public interface IBatchSaver
{
    Task SaveAsync(IReadOnlyList<Box> batch, Checkpoint checkpoint, long lineNumber);
    Task SaveFinalAsync(IReadOnlyList<Box> batch, Checkpoint checkpoint);
}