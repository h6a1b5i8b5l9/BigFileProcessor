namespace BigFileProcessor.Infrastructure.Database;

public class BoxEntity
{
    public const string TableName = "dbo.Box";

    public long Id { get; set; }
    public required string SupplierIdentifier { get; init; }
    public required string Identifier { get; init; }

    public required IEnumerable<ContentEntity> Contents { get; init; }
}