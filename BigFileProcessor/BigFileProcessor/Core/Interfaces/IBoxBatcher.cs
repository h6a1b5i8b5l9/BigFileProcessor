using BigFileProcessor.Infrastructure.FileSystem.FileParsing;

namespace BigFileProcessor.Core.Interfaces;

public interface IBoxBatcher
{
    void Add(HeaderLine header);
    void Add(ContentLine contentLine);
    bool BatchIsFull { get; }
    IReadOnlyList<Box> FinalizeBatch();
}