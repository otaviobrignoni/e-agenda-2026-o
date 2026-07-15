using System.Linq.Expressions;
using AutoMapper;
using eAgenda.WebApp.Compartilhado.Infra.Sql;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloContato.Dominio;

namespace eAgenda.WebApp.ModuloContato.Infra;

public class RepositorioContato : RepositorioSql<Contato, Contato>, IRepositorioContato
{
    private readonly IRepositorioGenerico repositorioGenerico;

    public RepositorioContato(ISqlConnectionFactory connectionFactory, IMapper mapper, IRepositorioGenerico repositorioGenerico) : base(connectionFactory, mapper)
    {
        this.repositorioGenerico = repositorioGenerico;
    }

    public bool Cadastrar(Contato registro)
    {
        string sqlQuery = """
            INSERT INTO dbo.TBContato (Id, Nome, Email, Telefone, Cargo, Empresa)
            VALUES (@Id, @Nome, @Email, @Telefone, @Cargo, @Empresa)
        """;

        return Execute(sqlQuery, registro);
    }

    public bool Editar(Guid id, Contato registroEditado)
    {
        registroEditado.Id = id;

        string sqlQuery = """
            UPDATE dbo.TBContato
            SET 
                Nome = @Nome,
                Email = @Email,
                Telefone = @Telefone,
                Cargo = @Cargo,
                Empresa = @Empresa
            WHERE Id = @Id;
        """;

        return Execute(sqlQuery, registroEditado);
    }

    public bool Excluir(Guid id)
    {
        string sqlQuery = """
            DELETE FROM dbo.TBContato
            WHERE Id = @Id;
        """;

        return Execute(sqlQuery, id);
    }

    public Contato? Selecionar(Guid id)
    {
        string sqlQuery = """
            SELECT Id, Nome, Email, Telefone, Cargo, Empresa 
            FROM dbo.TBContato
            WHERE Id = @Id;
        """;

        return QuerySingle(sqlQuery, id);
    }

    public List<Contato> Selecionar(Expression<Func<Contato, bool>>? filtro = null)
    {
        string sqlQuery = """
            SELECT Id, Nome, Email, Telefone, Cargo, Empresa 
            FROM dbo.TBContato
            ORDER BY Nome;
        """;

        return [.. Query(sqlQuery).Where(filtro?.Compile() ?? (t => true))];
    }

    public bool PossuiCompromissos(Guid id)
    {
        string sqlQuery = """
            SELECT COUNT(1)
            FROM dbo.TBCompromisso
            WHERE ContatoId = @Id;
        """;

        return repositorioGenerico.QuerySingle<int>(sqlQuery, id) > 0;
    }
}
