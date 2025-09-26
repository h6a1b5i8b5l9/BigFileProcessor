using BigFileProcessor.Infrastructure.FileSystem.FileParsing;

namespace BigFileProcessor.Core.Interfaces;

public interface IBoxBatcher
{
    void Add(HeaderLine header);
    void Add(ContentLine contentLine);
    IReadOnlyList<Box> GetCurrentBatch();
    bool BatchIsFull { get; }
    IReadOnlyList<Box> FinalizeBatch();
    IReadOnlyList<Box> FlushRemaining();
}