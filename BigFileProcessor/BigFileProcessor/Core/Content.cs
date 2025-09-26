namespace BigFileProcessor.Core;

public class Content
{
    public long Id { get; set; }
    public required string PoNumber { get; init; }
    public required string Isbn { get; init; }
    public int Quantity { get; init; }
    
    public long BoxId { get; set; }
}