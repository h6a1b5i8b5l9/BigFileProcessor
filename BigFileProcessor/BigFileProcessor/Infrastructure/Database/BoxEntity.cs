namespace BigFileProcessor.Core;

public class BoxEntity
{
    public long Id { get; set; }
    public required string SupplierIdentifier { get; init; }
    public required string Identifier { get; init; }

    public required IEnumerable<ContentEntity> Contents { get; init; }
}