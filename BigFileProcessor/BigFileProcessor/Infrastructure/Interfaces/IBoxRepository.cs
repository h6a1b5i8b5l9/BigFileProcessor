using BigFileProcessor.Core;

namespace BigFileProcessor.Infrastructure.Interfaces;

public interface IBoxRepository
{
    Task BulkSaveAsync(IReadOnlyList<Box> boxes);
}