using BigFileProcessor.Core;
using Microsoft.Data.SqlClient;

namespace BigFileProcessor.Infrastructure.Interfaces;

public interface IBulkInsertHelper
{
    Task BulkInsertBoxesAsync(
        SqlConnection connection,
        IEnumerable<Box> boxes,
        SqlTransaction? transaction = null);

    Task BulkInsertContentsAsync(
        SqlConnection connection,
        IEnumerable<Content> contents,
        SqlTransaction? transaction = null);
}