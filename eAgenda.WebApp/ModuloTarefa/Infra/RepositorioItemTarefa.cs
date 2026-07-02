using AutoMapper;
using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloTarefa.Dominio;

namespace eAgenda.WebApp.ModuloTarefa.Infra;

public class RepositorioItemTarefa(ISqlConnectionFactory connectionFactory, IMapper mapper)
    : RepositorioSql<ItemTarefa, ItemTarefaRow>(connectionFactory, mapper), IRepositorioItemTarefa
{
    public List<ItemTarefa> Selecionar(Tarefa tarefa)
    {
        string sqlQuery = """
            SELECT Id, Titulo, EstaConcluido, TarefaId
            FROM dbo.TBItemTarefa
            WHERE TarefaId = @Id
            ORDER BY Titulo;
        """;

        var rows = Query<ItemTarefaRow>(sqlQuery, tarefa.Id).ToList();

        return rows.Select(r => new ItemTarefa(r.Titulo, tarefa, r.EstaConcluido) { Id = r.Id }).ToList();
    }

    public void Cadastrar(ItemTarefa item)
    {
        string sqlQuery = """
            INSERT INTO dbo.TBItemTarefa (Id, Titulo, EstaConcluido, TarefaId)
            VALUES (@Id, @Titulo, @EstaConcluido, @TarefaId);
        """;

        Execute(sqlQuery, item);
    }

    public bool Excluir(ItemTarefa item)
    {
        string sqlQuery = """
            DELETE FROM dbo.TBItemTarefa
            WHERE Id = @Id;
        """;

        return Execute(sqlQuery, item) == 1;
    }

    public bool Editar(ItemTarefa item)
    {
        string sqlQuery = """
            UPDATE dbo.TBItemTarefa
            SET EstaConcluido = @EstaConcluido
            WHERE Id = @Id;
        """;

        return Execute(sqlQuery, item) == 1;
    }
}

public class ItemTarefaRow
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public bool EstaConcluido { get; set; }
    public Guid TarefaId { get; set; }
}
