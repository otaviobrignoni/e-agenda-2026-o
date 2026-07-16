using Microsoft.Data.SqlClient;

namespace eAgenda.WebApp.Compartilhado.Infra.Sql;

public sealed class SqlConnectionFactory(IConfiguration config) : ISqlConnectionFactory
{
    private const string NomeConnectionString = "SqlServerDapper";

    public SqlConnection CreateConnection()
    {
        string? connectionString = config.GetConnectionString(NomeConnectionString);

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException($"ConnectionString {NomeConnectionString} não encontrada");

        return new SqlConnection(connectionString);
    }
}
