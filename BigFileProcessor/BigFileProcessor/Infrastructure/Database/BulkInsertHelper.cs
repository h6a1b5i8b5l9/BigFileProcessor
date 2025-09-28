using System.Data;
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
        bulkCopy.DestinationTableName = BoxEntity.TableName;
        bulkCopy.BatchSize = _options.SqlBatchSize;
        bulkCopy.BulkCopyTimeout = 0;

        var table = CreateBoxDataTable();
        AddDefaultColumnMappings(bulkCopy, table);  

        foreach (var box in boxes) table.Rows.Add(box.Id, box.SupplierIdentifier, box.Identifier);

        await bulkCopy.WriteToServerAsync(table);
    }

    public async Task BulkInsertContentsAsync(
        SqlConnection connection,
        IEnumerable<ContentEntity> contents,
        SqlTransaction? transaction = null)
    {
        using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction);
        bulkCopy.DestinationTableName = ContentEntity.TableName;
        bulkCopy.BatchSize = _options.SqlBatchSize;
        bulkCopy.BulkCopyTimeout = 0;

        var table = CreateContentDataTable();
        AddDefaultColumnMappings(bulkCopy, table);       

        foreach (var content in contents)
            table.Rows.Add(content.Id, content.PoNumber, content.Isbn, content.Quantity, content.BoxId);

        await bulkCopy.WriteToServerAsync(table);
    }

    private static DataTable CreateBoxDataTable()
    {
        var table = new DataTable();
        table.Columns.Add(nameof(BoxEntity.Id), typeof(long));
        table.Columns.Add(nameof(BoxEntity.SupplierIdentifier), typeof(string));
        table.Columns.Add(nameof(BoxEntity.Identifier), typeof(string));

        return table;
    }

    private static DataTable CreateContentDataTable()
    {
        var table = new DataTable();
        table.Columns.Add(nameof(ContentEntity.Id), typeof(long));
        table.Columns.Add(nameof(ContentEntity.PoNumber), typeof(string));
        table.Columns.Add(nameof(ContentEntity.Isbn), typeof(string));
        table.Columns.Add(nameof(ContentEntity.Quantity), typeof(int));
        table.Columns.Add(nameof(ContentEntity.BoxId), typeof(long));

        return table;
    }

    private static void AddDefaultColumnMappings(SqlBulkCopy bulkCopy, DataTable table)
    {
        foreach (DataColumn col in table.Columns)
            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
    }
}