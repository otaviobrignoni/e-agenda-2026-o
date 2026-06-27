using AutoMapper;
using Dapper;
using eAgenda.WebApp.Compartilhado.Infra;
using Microsoft.Data.SqlClient;

namespace eAgenda.WebApp.Compartilhado.ModuloBase;

public abstract class RepositorioSql<TRegistro, TRow>(ISqlConnectionFactory connectionFactory, IMapper mapper) where TRegistro : EntidadeBase<TRegistro> where TRow : class
{
    protected int Execute(string sqlQuery, TRegistro registro)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        return conexao.Execute(sqlQuery, registro);
    }

    protected int Execute(string sqlQuery, Guid id)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        return conexao.Execute(sqlQuery, new { Id = id });
    }

    protected IEnumerable<TRegistro> Query(string sqlQuery, Func<TRegistro, bool>? filtro = null)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        return conexao.Query<TRow>(sqlQuery).Select(Mapear).Where(filtro ?? (_ => true))!;
    }


    protected TRegistro? QuerySingle(string sqlQuery, Guid id)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        var row = conexao.QuerySingleOrDefault<TRow>(sqlQuery, new { Id = id });

        if (row is null)
            return null;

        return Mapear(row);
    }

    private TRegistro Mapear(TRow row)
    {
        if (row is TRegistro registro)
            return registro;

        return mapper.Map<TRegistro>(row);
    }
}
