namespace BigFileProcessor.Infrastructure.Database;

public class ContentEntity
{
    public const string TableName = "dbo.Content";
    
    public long Id { get; set; }
    public required string PoNumber { get; init; }
    public required string Isbn { get; init; }
    public int Quantity { get; init; }
    
    public long BoxId { get; set; }
}