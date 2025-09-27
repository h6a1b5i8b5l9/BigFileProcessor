using BigFileProcessor.Core;
using BigFileProcessor.Core.Interfaces;
using BigFileProcessor.Infrastructure.Database;
using BigFileProcessor.Infrastructure.Interfaces;

namespace BigFileProcessor.Infrastructure;

public class BatchSaver(IBoxRepository boxRepository, ICheckpointManager checkpointManager) : IBatchSaver
{
    public async Task SaveAsync(IReadOnlyList<Box> batch, Checkpoint checkpoint, long lineNumber)
    {
        if (batch.Count == 0) return;

        await boxRepository.BulkSaveAsync(batch.ToEntities());
        checkpoint.LastProcessedLine = lineNumber;
        await checkpointManager.SaveAsync(checkpoint);
    }

    public async Task SaveFinalAsync(IReadOnlyList<Box> batch, Checkpoint checkpoint)
    {
        if (batch.Count == 0) return;

        await boxRepository.BulkSaveAsync(batch.ToEntities());
        await checkpointManager.DeleteAsync(checkpoint.FileName); 
    }
}