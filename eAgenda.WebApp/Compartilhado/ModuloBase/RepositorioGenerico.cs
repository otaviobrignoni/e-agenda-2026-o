using Dapper;
using eAgenda.WebApp.Compartilhado.Infra;

namespace eAgenda.WebApp.Compartilhado.ModuloBase;

public class RepositorioGenerico(ISqlConnectionFactory connectionFactory) : IRepositorioGenerico
{
    public IEnumerable<T> Query<T>(string sqlQuery, Guid id) => Query<T>(sqlQuery, new { Id = id });

    public IEnumerable<T> Query<T>(string sqlQuery, object? parametros = null)
    {
        using var conexao = connectionFactory.CreateConnection();
        conexao.Open();
        return conexao.Query<T>(sqlQuery, parametros).ToList();
    }

    public T QuerySingle<T>(string sqlQuery, Guid id) => QuerySingle<T>(sqlQuery, new { Id = id });

    public T QuerySingle<T>(string sqlQuery, object parametros)
    {
        using var conexao = connectionFactory.CreateConnection();
        conexao.Open();
        return conexao.QuerySingle<T>(sqlQuery, parametros);
    }
}
