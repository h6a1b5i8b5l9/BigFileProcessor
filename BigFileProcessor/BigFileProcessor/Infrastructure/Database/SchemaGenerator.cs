using System.Reflection;
using System.Text;

namespace BigFileProcessor.Infrastructure.Database;

public static class SchemaGenerator
{
    public static string GenerateCreateTableSql<T>()
    {
        var type = typeof(T);
        var field = type.GetField("TableName", BindingFlags.Public | BindingFlags.Static);
        var tableName = field?.GetValue(null) as string
                        ?? throw new InvalidOperationException($"Entity {type.Name} must have a public static TableName field.");

        var sb = new StringBuilder();
        sb.AppendLine($"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{tableName}' AND xtype='U')");
        sb.AppendLine("BEGIN");
        sb.AppendLine($"CREATE TABLE [dbo].[{tableName}] (");

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var columnDefinitions = new List<string>();

        foreach (var prop in properties)
        {
            var columnName = prop.Name;
            var sqlType = GetSqlType(prop.PropertyType);
            if (string.IsNullOrEmpty(sqlType)) continue;
            var nullable = IsNullable(prop);

            var line = $"    [{columnName}] {sqlType}{(nullable ? "" : " NOT NULL")}";
            if (columnName == "Id")
                line += " PRIMARY KEY";

            columnDefinitions.Add(line);
        }

        sb.AppendLine(string.Join(",\n", columnDefinitions));
        sb.AppendLine(");");
        sb.AppendLine("END");

        return sb.ToString();
    }

    private static string GetSqlType(Type type)
    {
        var t = Nullable.GetUnderlyingType(type) ?? type;
        return t.Name switch
        {
            "Int32" => "INT",
            "Int64" => "BIGINT",
            "String" => "NVARCHAR(20)",
            _ => string.Empty
        };
    }

    private static bool IsNullable(PropertyInfo prop)
    {
        return Nullable.GetUnderlyingType(prop.PropertyType) != null
               || !prop.PropertyType.IsValueType;
    }
}