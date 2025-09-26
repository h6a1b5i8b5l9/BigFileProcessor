namespace BigFileProcessor.Infrastructure.FileSystem.FileParsing;

public readonly record struct ParsedItem(long LineNumber, ParsedLine Parsed);