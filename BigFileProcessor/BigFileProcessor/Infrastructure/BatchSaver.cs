using BigFileProcessor.Core;
using BigFileProcessor.Core.Interfaces;
using BigFileProcessor.Infrastructure.Interfaces;

namespace BigFileProcessor.Infrastructure;

public class BatchSaver(IBoxRepository boxRepository, ICheckpointManager checkpointManager) : IBatchSaver
{
    public async Task SaveAsync(IReadOnlyList<Box> batch, Checkpoint checkpoint, long lineNumber)
    {
        if (batch.Count == 0) return;

        await boxRepository.BulkSaveAsync(batch);
        checkpoint.LastProcessedLine = lineNumber;
        await checkpointManager.SaveAsync(checkpoint);
    }

    public async Task SaveFinalAsync(IReadOnlyList<Box> batch, Checkpoint checkpoint)
    {
        if (batch.Count == 0) return;

        await boxRepository.BulkSaveAsync(batch);
        await checkpointManager.DeleteAsync(checkpoint.FileName);
    }
}