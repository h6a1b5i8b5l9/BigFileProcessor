using BigFileProcessor.Core;
using Microsoft.Data.SqlClient;

namespace BigFileProcessor.Infrastructure.Interfaces;

public interface IBulkInsertHelper
{
    Task BulkInsertBoxesAsync(
        SqlConnection connection,
        IEnumerable<BoxEntity> boxes,
        SqlTransaction? transaction = null);

    Task BulkInsertContentsAsync(
        SqlConnection connection,
        IEnumerable<ContentEntity> contents,
        SqlTransaction? transaction = null);
}