using Microsoft.Data.SqlClient;

namespace eAgenda.WebApp.Compartilhado.Infra;

public sealed class SqlConnectionFactory(IConfiguration config) : ISqlConnectionFactory
{
    private const string NomeConnectionString = "eAgenda";

    public SqlConnection CreateConnection()
    {
        string? connectionString = config.GetConnectionString(NomeConnectionString);

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException($"ConnectionString {NomeConnectionString} não encontrada");

        return new SqlConnection(connectionString);
    }
}
