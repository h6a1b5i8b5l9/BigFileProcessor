using BigFileProcessor.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BigFileProcessor.Infrastructure.Database;

public class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
                                                ?? throw new InvalidOperationException("Connection string not found.");

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}