using BigFileProcessor.Core;
using BigFileProcessor.Infrastructure.Database;

namespace BigFileProcessor.Infrastructure.Interfaces;

public interface IBoxRepository
{
    Task BulkSaveAsync(IReadOnlyList<BoxEntity> boxes);
}