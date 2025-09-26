namespace BigFileProcessor.Infrastructure.FileSystem.FileParsing;

public record HeaderLine(string SupplierIdentifier, string Identifier) : ParsedLine;