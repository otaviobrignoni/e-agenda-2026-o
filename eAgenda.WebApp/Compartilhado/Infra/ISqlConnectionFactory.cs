using Microsoft.Data.SqlClient;

namespace eAgenda.WebApp.Compartilhado.Infra;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}
