namespace BigFileProcessor.Core;

public class Checkpoint
{
    public required string FileName { get; init; }
    public long LastProcessedLine { get; set; }
}