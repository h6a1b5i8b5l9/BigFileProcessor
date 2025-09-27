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
            switch (item.Parsed)
            {
                case HeaderLine header:
                    if (batcher.BatchIsFull)
                    {
                        var batch = batcher.FinalizeBatch();
                        await saver.SaveAsync(batch, checkpoint, item.LineNumber);
                    }

                    batcher.Add(header);
                    break;

                case ContentLine content:
                    batcher.Add(content);
                    break;
            }

        var remaining = batcher.FinalizeBatch();
        await saver.SaveFinalAsync(remaining, checkpoint);
    }
}