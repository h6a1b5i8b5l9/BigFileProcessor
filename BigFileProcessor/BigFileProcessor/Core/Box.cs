namespace BigFileProcessor.Core;

public class Box
{
    public long Id { get; set; }
    public required string SupplierIdentifier { get; init; }
    public required string Identifier { get; init; }

    public required IEnumerable<Content> Contents { get; set; }
}