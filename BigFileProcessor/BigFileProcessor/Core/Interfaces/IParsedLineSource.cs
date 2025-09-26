using BigFileProcessor.Infrastructure.FileSystem.FileParsing;

namespace BigFileProcessor.Core.Interfaces;

public interface IParsedLineSource
{
    IAsyncEnumerable<ParsedItem> ReadAsync(
        string inputFile,
        long startAtLineExclusive,
        CancellationToken cancellationToken = default);
}
