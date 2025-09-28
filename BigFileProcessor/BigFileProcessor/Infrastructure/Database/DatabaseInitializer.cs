using BigFileProcessor.Infrastructure.Interfaces;

namespace BigFileProcessor.Infrastructure.Database;

public class DatabaseInitializer(ISqlConnectionFactory sqlConnectionFactory)
{
    public async Task EnsureTablesExistAsync()
    {
        var createTableCommands = new[]
        {
            SchemaGenerator.GenerateCreateTableSql<BoxEntity>(),
            SchemaGenerator.GenerateCreateTableSql<ContentEntity>()
        };

        await using var connection = sqlConnectionFactory.CreateConnection();
        await connection.OpenAsync();

        foreach (var sql in createTableCommands)
        {
            await using var command = connection.CreateCommand();
            command.CommandText = sql;
            await command.ExecuteNonQueryAsync();
        }
    }
}