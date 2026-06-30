using AutoMapper;
using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloCategoria.Dominio;

namespace eAgenda.WebApp.ModuloCategoria.Infra;

public class RepositorioCategoria : RepositorioSql<Categoria, Categoria>, IRepositorioCategoria
{
    public RepositorioCategoria(ISqlConnectionFactory connectionFactory, IMapper mapper) : base(connectionFactory, mapper)
    {
    }

    public List<Categoria> Registros => Selecionar();

    public void Cadastrar(Categoria registro)
    {
        string sqlQuery = """
            INSERT INTO dbo.TBCategoria (Titulo)
            VALUES (@Id, @Titulo)
        """;

        Execute(sqlQuery, registro);
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

        return Execute(sqlQuery, registroEditado) == 1;
    }

    public bool Excluir(Guid id)
    {
        string sqlQuery = """
            DELETE FROM dbo.TBCategoria
            WHERE Id = @Id;
        """;

        return Execute(sqlQuery, id) == 1;
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

    public List<Categoria> Selecionar(Func<Categoria, bool>? filtro = null)
    {
        string sqlQuery = """
            SELECT Id, Titulo
            FROM dbo.TBCategoria       
            ORDER BY Titulo;
        """;

        return Query(sqlQuery).Where(filtro ?? (t => true)).ToList();
    }
}
