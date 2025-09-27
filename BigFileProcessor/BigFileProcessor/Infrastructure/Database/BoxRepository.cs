using BigFileProcessor.Core;
using BigFileProcessor.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;

namespace BigFileProcessor.Infrastructure.Database;

public class BoxRepository(ISqlConnectionFactory connectionFactory, IBulkInsertHelper bulkInsertHelper) : IBoxRepository
{
    public async Task BulkSaveAsync(IReadOnlyList<BoxEntity> boxes)
    {
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync() as SqlTransaction;
        try
        {
            await SaveBoxesToDb(boxes, connection, transaction);

            await transaction!.CommitAsync();
        }
        catch
        {
            await transaction!.RollbackAsync();
            throw;
        }
    }

    private async Task SaveBoxesToDb(IReadOnlyList<BoxEntity> boxes, SqlConnection connection, SqlTransaction? transaction)
    {
        var batchStartBoxId = await GetBatchStartId(connection, transaction, "Boxes");
        var batchStartContentId = await GetBatchStartId(connection, transaction, "Contents");

        SetPrimaryKeys(boxes, batchStartBoxId, batchStartContentId);

        await bulkInsertHelper.BulkInsertBoxesAsync(connection, boxes, transaction);
        await bulkInsertHelper.BulkInsertContentsAsync(connection, boxes.SelectMany(b => b.Contents), transaction);
    }

    private static void SetPrimaryKeys(IReadOnlyList<BoxEntity> boxes, long batchStartBoxId, long batchStartContentId)
    {
        var currentBoxId = batchStartBoxId;
        var currentContentId = batchStartContentId;

        foreach (var box in boxes)
        {
            box.Id = currentBoxId++;
            foreach (var content in box.Contents)
            {
                content.Id = currentContentId++;
                content.BoxId = box.Id;
            }
        }
    }

    private static async Task<long> GetBatchStartId(
        SqlConnection connection,
        SqlTransaction? transaction,
        string tableName)
    {
        var sql = $"SELECT ISNULL(MAX(Id), 0) FROM {tableName}";
        await using var cmd = new SqlCommand(sql, connection, transaction);
        var maxId = (long)(await cmd.ExecuteScalarAsync())!;

        return maxId + 1;
    }
}