using AutoMapper;
using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloTarefa.Dominio;

namespace eAgenda.WebApp.ModuloTarefa.Infra;

public class RepositorioTarefa(ISqlConnectionFactory connectionFactory, IMapper mapper, IRepositorioItemTarefa repositorioItemTarefa) : RepositorioSql<Tarefa, Tarefa>(connectionFactory, mapper), IRepositorioTarefa
{
    public List<Tarefa> Registros => Selecionar();

    public void Cadastrar(Tarefa tarefa)
    {
        string sqlQuery = """
            INSERT INTO dbo.TBTarefa (Id, Titulo, Prioridade, DataCriacao, DataConclusao)
            VALUES (@Id, @Titulo, @Prioridade, @DataCriacao, @DataConclusao)
        """;

        Execute(sqlQuery, tarefa);
    }

    public bool Editar(Guid id, Tarefa tarefaEditada)
    {
        tarefaEditada.Id = id;

        string sqlQuery = """
            UPDATE dbo.TBTarefa
            SET
                Titulo = @Titulo,
                Prioridade = @Prioridade,
                DataCriacao = @DataCriacao,
                DataConclusao = @DataConclusao
            WHERE Id = @Id;

        """;

        return Execute(sqlQuery, tarefaEditada) == 1;
    }

    public bool Excluir(Guid id)
    {
        string sqlQuery = """
            DELETE FROM dbo.TBTarefa
            WHERE Id = @Id;
        """;

        return Execute(sqlQuery, id) == 1;
    }

    public Tarefa? Selecionar(Guid id)
    {
        string sqlQuery = """
            SELECT Id, Titulo, Prioridade, DataCriacao, DataConclusao
            FROM dbo.TBTarefa
            WHERE Id = @Id;
        """;

        var tarefa = QuerySingle(sqlQuery, id);

        if (tarefa is not null)
            tarefa.Itens = repositorioItemTarefa.Selecionar(tarefa);

        return tarefa;
    }

    public List<Tarefa> Selecionar(Func<Tarefa, bool>? filtro = null)
    {
        string sqlQuery = """
            SELECT Id, Titulo, Prioridade, DataCriacao, DataConclusao
            FROM dbo.TBTarefa
            ORDER BY Titulo;
        """;

        var tarefas = Query(sqlQuery).Where(filtro ?? (t => true)).ToList();

        foreach (var tarefa in tarefas)
            tarefa.Itens = repositorioItemTarefa.Selecionar(tarefa);

        return tarefas;
    }
}
