using System.ComponentModel.DataAnnotations;

namespace BigFileProcessor.Infrastructure;

public class FileProcessingOptions
{
    public const string SectionName = "FileProcessing";

    [Required] public int BoxCountInRam { get; init; }

    [Required] public int SqlBatchSize { get; init; }
}