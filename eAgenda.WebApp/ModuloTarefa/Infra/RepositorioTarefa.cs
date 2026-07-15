using System.Linq.Expressions;
using AutoMapper;
using eAgenda.WebApp.Compartilhado.Infra.Sql;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloTarefa.Dominio;

namespace eAgenda.WebApp.ModuloTarefa.Infra;

public class RepositorioTarefa(ISqlConnectionFactory connectionFactory, IMapper mapper, IRepositorioItemTarefa repositorioItemTarefa) : RepositorioSql<Tarefa, Tarefa>(connectionFactory, mapper), IRepositorioTarefa
{
    public bool Cadastrar(Tarefa tarefa)
    {
        string sqlQuery = """
            INSERT INTO dbo.TBTarefa (Id, Titulo, Prioridade, DataCriacao, DataConclusao)
            VALUES (@Id, @Titulo, @Prioridade, @DataCriacao, @DataConclusao)
        """;

        return Execute(sqlQuery, tarefa);
    }

    public bool Editar(Guid id, Tarefa tarefaEditada)
    {
        tarefaEditada.Id = id;

        string sqlQuery = """
            UPDATE dbo.TBTarefa
            SET
                Titulo = @Titulo,
                Prioridade = @Prioridade
            WHERE Id = @Id;

        """;

        return Execute(sqlQuery, tarefaEditada);
    }

    public bool Excluir(Guid id)
    {
        string sqlQuery = """
            DELETE FROM dbo.TBTarefa
            WHERE Id = @Id;
        """;

        return Execute(sqlQuery, id);
    }

    public Tarefa? Selecionar(Guid id)
    {
        string sqlQuery = """
            SELECT Id, Titulo, Prioridade, DataCriacao, DataConclusao
            FROM dbo.TBTarefa
            WHERE Id = @Id;
        """;

        var tarefa = QuerySingle(sqlQuery, id);

        tarefa?.Itens = repositorioItemTarefa.Selecionar(tarefa);

        return tarefa;
    }

    public List<Tarefa> Selecionar(Expression<Func<Tarefa, bool>>? filtro = null)
    {
        string sqlQuery = """
            SELECT Id, Titulo, Prioridade, DataCriacao, DataConclusao
            FROM dbo.TBTarefa
            ORDER BY Titulo;
        """;

        var tarefas = Query(sqlQuery).Where(filtro?.Compile() ?? (t => true)).ToList();

        foreach (var tarefa in tarefas)
            tarefa.Itens = repositorioItemTarefa.Selecionar(tarefa);

        return tarefas;
    }

    public bool AtualizarDataConclusao(Tarefa tarefa)
    {
        const string sqlTarefa = """
            UPDATE dbo.TBTarefa
            SET DataConclusao = @DataConclusao
            WHERE Id = @Id;
        """;

        return Execute(sqlTarefa, tarefa);
    }
}
