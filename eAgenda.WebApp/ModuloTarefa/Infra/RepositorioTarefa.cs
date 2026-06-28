using AutoMapper;
using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloTarefa.Dominio;

namespace eAgenda.WebApp.ModuloTarefa.Infra;

public class RepositorioTarefa(ISqlConnectionFactory connectionFactory, IMapper mapper) : RepositorioSql<Tarefa, Tarefa>(connectionFactory, mapper), IRepositorioTarefa
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

        return QuerySingle(sqlQuery, id);
    }

    public List<Tarefa> Selecionar(Func<Tarefa, bool>? filtro = null)
    {
        string sqlQuery = """
            SELECT Id, Titulo, Prioridade, DataCriacao, DataConclusao
            FROM dbo.TBTarefa
            ORDER BY Titulo;
        """;

        return Query(sqlQuery).Where(filtro ?? (t => true)).ToList();
    }
    public List<ItemTarefa> SelecionarItens(Guid id)
    {
        string sqlQuery = """
            SELECT Titulo, EstaConcluido, TarefaId
            FROM dbo.TBItemTarefa
            WHERE TarefaId = @Id
            ORDER BY Titulo;
        """;
        var tarefa = Selecionar(id);

        if (tarefa is null)
            return [];

        var rows = Query<ItemTarefaRow>(sqlQuery, id).ToList();

        return rows.Select(r => new ItemTarefa(r.Titulo, tarefa, r.EstaConcluido)).ToList();
    }

    public bool AdicionarItem(ItemTarefa item)
    {
        string sqlQuery = """
            INSERT INTO dbo.TBItemTarefa (Titulo, EstaConcluido, TarefaId)
            VALUES (@Titulo, @EstaConcluido, @TarefaId);
        """;

        return Execute(sqlQuery, new { item.Titulo, item.EstaConcluido, item.TarefaId }) == 1;
    }

    public bool RemoverItem(Guid tarefaId, string titulo)
    {
        string sqlQuery = """
            DELETE FROM dbo.TBItemTarefa
            WHERE TarefaId = @TarefaId AND Titulo = @Titulo;
        """;

        return Execute(sqlQuery, new { TarefaId = tarefaId, Titulo = titulo }) == 1;
    }

    public bool AlterarConclusaoItem(Guid tarefaId, string titulo, bool estaConcluido)
    {
        string sqlQuery = """
            UPDATE dbo.TBItemTarefa
            SET EstaConcluido = @EstaConcluido
            WHERE TarefaId = @TarefaId AND Titulo = @Titulo;
        """;

        return Execute(sqlQuery, new { TarefaId = tarefaId, Titulo = titulo, EstaConcluido = estaConcluido }) == 1;
    }

    public bool AtualizarDataConclusao(Guid id, DateTime? dataConclusao)
    {
        string sqlQuery = """
            UPDATE dbo.TBTarefa
            SET DataConclusao = @DataConclusao
            WHERE Id = @Id;
        """;

        return Execute(sqlQuery, new { Id = id, DataConclusao = dataConclusao }) == 1;
    }
}

public class ItemTarefaRow
{
    public string Titulo { get; set; } = string.Empty;
    public bool EstaConcluido { get; set; }
    public Guid TarefaId { get; set; }
}

