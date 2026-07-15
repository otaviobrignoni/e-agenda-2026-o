using System.Linq.Expressions;
using AutoMapper;
using eAgenda.WebApp.Compartilhado.Infra.Sql;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloCategoria.Dominio;

namespace eAgenda.WebApp.ModuloCategoria.Infra;

public class RepositorioCategoria(ISqlConnectionFactory connectionFactory, IMapper mapper, IRepositorioGenerico repositorioGenerico) : RepositorioSql<Categoria, Categoria>(connectionFactory, mapper), IRepositorioCategoria
{
    public bool Cadastrar(Categoria registro)
    {
        string sqlQuery = """
            INSERT INTO dbo.TBCategoria (Id, Titulo)
            VALUES (@Id, @Titulo)
        """;

        return Execute(sqlQuery, registro);
    }

    public bool Editar(Guid id, Categoria registroEditado)
    {
        registroEditado.Id = id;

        string sqlQuery = """
            UPDATE dbo.TBCategoria
            SET 
                Titulo = @Titulo
            WHERE Id = @Id;
        """;

        return Execute(sqlQuery, registroEditado);
    }

    public bool Excluir(Guid id)
    {
        string sqlQuery = """
            DELETE FROM dbo.TBCategoria
            WHERE Id = @Id;
        """;

        return Execute(sqlQuery, id);
    }

    public Categoria? Selecionar(Guid id)
    {
        string sqlQuery = """
            SELECT Id, Titulo
            FROM dbo.TBCategoria
            WHERE Id = @Id
        """;

        return QuerySingle(sqlQuery, id);
    }

    public List<Categoria> Selecionar(Expression<Func<Categoria, bool>>? filtro = null)
    {
        string sqlQuery = """
            SELECT Id, Titulo
            FROM dbo.TBCategoria       
            ORDER BY Titulo;
        """;

        return [.. Query(sqlQuery).Where(filtro?.Compile() ?? (t => true))];
    }

    public List<Categoria> Selecionar(IEnumerable<Guid> ids)
    {
        string sqlQuery = """
            SELECT Id, Titulo
            FROM dbo.TBCategoria
            WHERE Id IN @Ids
            ORDER BY Titulo;
        """;

        var idsLista = ids.ToList();

        return [.. Query(sqlQuery, new { Ids = idsLista })];
    }

    public bool PossuiDespesas(Guid id)
    {
        string sqlQuery = """
            SELECT COUNT(1)
            FROM dbo.TBCategoriaDespesa
            WHERE CategoriaId = @Id;
        """;

        return repositorioGenerico.QuerySingle<int>(sqlQuery, id) > 0;
    }
}
