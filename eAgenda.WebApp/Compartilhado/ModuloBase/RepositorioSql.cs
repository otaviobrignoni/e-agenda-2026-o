using AutoMapper;
using Dapper;
using eAgenda.WebApp.Compartilhado.Extensions;
using eAgenda.WebApp.Compartilhado.Infra;
using Microsoft.Data.SqlClient;

namespace eAgenda.WebApp.Compartilhado.ModuloBase;

public abstract class RepositorioSql<TRegistro, TRow>(ISqlConnectionFactory connectionFactory, IMapper mapper) where TRegistro : EntidadeBase<TRegistro> where TRow : class
{
    protected IMapper Mapper { get; } = mapper;

    protected bool Execute(string sqlQuery, Guid id) => Execute(sqlQuery, new { Id = id });

    protected bool Execute(string sqlQuery, object parametros) => Execute((sqlQuery, parametros));

    protected bool Execute(params (string sqlQuery, object? parametros)[] comandos)
    {
        if (comandos.Length == 0) return false;
        
        using var conexao = AbrirConexao();
        using var transacao = conexao.BeginTransaction();
        try
        {
            foreach (var (sqlQuery, parametros) in comandos)
            {
                if (conexao.Execute(sqlQuery, parametros, transacao) > 0)
                    continue;

                transacao.Rollback();
                return false;
            }

            transacao.Commit();
            return true;
        }
        catch
        {
            transacao.Rollback();
            return false;
        }
    }

    protected IEnumerable<TRegistro> Query(string sqlQuery) => Query(sqlQuery, null);

    protected IEnumerable<TRegistro> Query(string sqlQuery, object? parametros, params (string Key, object Value)[] itens)
    {
        using var conexao = AbrirConexao();
        var rows = conexao.Query<TRow>(sqlQuery, parametros);

        if (itens.Length == 0)
            return [.. rows.Select(Mapear)];

        return [.. rows.Select(row => Mapper.MapWith<TRegistro>(row, itens))];
    }

    protected TRegistro? QuerySingle(string sqlQuery, Guid id) => QuerySingle(sqlQuery, new { Id = id });

    protected TRegistro? QuerySingle(string sqlQuery, object parametros)
    {
        using var conexao = AbrirConexao();
        var row = conexao.QuerySingleOrDefault<TRow>(sqlQuery, parametros);
        return row is null ? null : Mapear(row);
    }

    private TRegistro Mapear(TRow row)
    {
        if (row is TRegistro registro)
            return registro;
        return Mapper.Map<TRegistro>(row);
    }

    private SqlConnection AbrirConexao()
    {
        var conexao = connectionFactory.CreateConnection();
        conexao.Open();
        return conexao;
    }
}
