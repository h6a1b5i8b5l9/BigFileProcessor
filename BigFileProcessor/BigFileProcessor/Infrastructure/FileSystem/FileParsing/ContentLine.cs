namespace BigFileProcessor.Infrastructure.FileSystem.FileParsing;

public record ContentLine(string PoNumber, string Isbn, int Quantity) : ParsedLine;