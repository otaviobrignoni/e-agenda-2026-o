using AutoMapper;
using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloContato.Dominio;

namespace eAgenda.WebApp.ModuloContato.Infra;

public class RepositorioContato : RepositorioSql<Contato, Contato>, IRepositorioContato
{
    public RepositorioContato(ISqlConnectionFactory connectionFactory, IMapper mapper) : base(connectionFactory, mapper)
    {
    }

    public List<Contato> Registros => Selecionar();

    public void Cadastrar(Contato registro)
    {
        string sqlQuery = """
            INSERT INTO dbo.TBContato (Id, Nome, Email, Telefone, Cargo, Empresa)
            VALUES (@Id, @Nome, @Email, @Telefone, @Cargo, @Empresa)
        """;

        Execute(sqlQuery, registro);
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
                Empresa = @Empresa,
        """;

        return Execute(sqlQuery, registroEditado) == 1;
    }

    public bool Excluir(Guid id)
    {
        string sqlQuery = """
            DELETE FROM dbo.TBContato;
            WHERE Id = @Id;
        """;

        return Execute(sqlQuery, id) == 1;
    }

    public Contato? Selecionar(Guid id)
    {
        string sqlQuery = """
            SELECT Id, Nome, Email, Telefone, Cargo, Empresa 
            FROM dbo.TBContato
            ORDER BY Nome;
        """;

        return QuerySingle(sqlQuery, id);
    }

    public List<Contato> Selecionar(Func<Contato, bool>? filtro = null)
    {
        string sqlQuery = """
            SELECT Id, Nome, Email, Telefone, Cargo, Empresa 
            FROM dbo.TBContato
            ORDER BY Nome;
        """;

        return Query(sqlQuery).Where(filtro ?? (t => true)).ToList();
    }
}
