using System.Runtime.CompilerServices;
using BigFileProcessor.Core.Interfaces;

namespace BigFileProcessor.Infrastructure.FileSystem.FileParsing;

public class FileParsedLineSource : IParsedLineSource
{
    public async IAsyncEnumerable<ParsedItem> ReadAsync(
        string inputFile,
        long startAtLineExclusive,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await using var stream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        using var reader = new StreamReader(stream);

        long lineNumber = 0;
        while (!reader.EndOfStream)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var line = await reader.ReadLineAsync(cancellationToken);
            if (line is null) break;

            lineNumber++;
            if (lineNumber <= startAtLineExclusive) continue;

            var parsed = LineParser.Parse(line);
            if (parsed is not null) yield return new ParsedItem(lineNumber, parsed);
        }
    }
}