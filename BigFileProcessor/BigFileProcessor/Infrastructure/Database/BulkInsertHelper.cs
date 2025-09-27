using System.Data;
using BigFileProcessor.Core;
using BigFileProcessor.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace BigFileProcessor.Infrastructure.Database;

public class BulkInsertHelper(IOptions<FileProcessingOptions> options) : IBulkInsertHelper
{
    private readonly FileProcessingOptions _options = options.Value;

    public async Task BulkInsertBoxesAsync(
        SqlConnection connection,
        IEnumerable<BoxEntity> boxes,
        SqlTransaction? transaction = null)
    {
        using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction);
        bulkCopy.DestinationTableName = "dbo.Boxes";
        bulkCopy.BatchSize = _options.SqlBatchSize;
        bulkCopy.BulkCopyTimeout = 0;

        var table = new DataTable();
        table.Columns.Add("Id", typeof(long));
        table.Columns.Add("SupplierIdentifier", typeof(string));
        table.Columns.Add("Identifier", typeof(string));

        foreach (var box in boxes) table.Rows.Add(box.Id, box.SupplierIdentifier, box.Identifier);

        await bulkCopy.WriteToServerAsync(table);
    }

    public async Task BulkInsertContentsAsync(
        SqlConnection connection,
        IEnumerable<ContentEntity> contents,
        SqlTransaction? transaction = null)
    {
        using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction);
        bulkCopy.DestinationTableName = "dbo.Contents";
        bulkCopy.BatchSize = _options.SqlBatchSize;
        bulkCopy.BulkCopyTimeout = 0;

        var table = new DataTable();
        table.Columns.Add("Id", typeof(long));
        table.Columns.Add("PoNumber", typeof(string));
        table.Columns.Add("Isbn", typeof(string));
        table.Columns.Add("Quantity", typeof(int));
        table.Columns.Add("BoxIdentifier", typeof(string));

        foreach (var content in contents)
            table.Rows.Add(content.Id, content.PoNumber, content.Isbn, content.Quantity, content.BoxId);

        await bulkCopy.WriteToServerAsync(table);
    }
}