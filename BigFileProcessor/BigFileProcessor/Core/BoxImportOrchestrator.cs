using System.Diagnostics;
using BigFileProcessor.Core.Interfaces;
using BigFileProcessor.Infrastructure.FileSystem.FileParsing;

namespace BigFileProcessor.Core;

public class BoxImportOrchestrator(
    ICheckpointManager checkpointManager,
    IParsedLineSource source,
    IBoxBatcher batcher,
    IBatchSaver saver) : IBoxImportOrchestrator
{
    public async Task ProcessAsync(
        string inputFile,
        CancellationToken cancellationToken = default)
    {
        var checkpoint = await checkpointManager.LoadAsync(inputFile);

        await foreach (var item in source.ReadAsync(inputFile, checkpoint.LastProcessedLine, cancellationToken))
        {
            if (cancellationToken.IsCancellationRequested) break;

            if (item.Parsed is HeaderLine header)
            {
                if (batcher.BatchIsFull)
                {
                    var batch = batcher.FinalizeBatch();
                    await saver.SaveAsync(batch, checkpoint, item.LineNumber);
                }

                batcher.Add(header);;
            }

            if (item.Parsed is ContentLine content)
            {
                batcher.Add(content);
            }
        }

        var remaining = batcher.FinalizeBatch();
        await saver.SaveFinalAsync(remaining, checkpoint);
    }
}