using Microsoft.Data.SqlClient;

namespace BigFileProcessor.Infrastructure.Interfaces;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}