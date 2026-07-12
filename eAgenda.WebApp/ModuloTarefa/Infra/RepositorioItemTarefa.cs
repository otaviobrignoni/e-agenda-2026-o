using AutoMapper;
using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloTarefa.Dominio;

namespace eAgenda.WebApp.ModuloTarefa.Infra;

public class RepositorioItemTarefa(ISqlConnectionFactory connectionFactory, IMapper mapper) : RepositorioSql<ItemTarefa, ItemTarefaRow>(connectionFactory, mapper), IRepositorioItemTarefa
{
    public List<ItemTarefa> Selecionar(Tarefa tarefa)
    {
        string sqlQuery = """
            SELECT Id, Titulo, EstaConcluido, TarefaId
            FROM dbo.TBItemTarefa
            WHERE TarefaId = @Id
            ORDER BY Titulo;
        """;
        return [.. Query(sqlQuery, new {tarefa.Id }, (nameof(ItemTarefa.Tarefa), tarefa))];
    }

    public bool Cadastrar(ItemTarefa item)
    {
        string sqlQuery = """
            INSERT INTO dbo.TBItemTarefa (Id, Titulo, EstaConcluido, TarefaId)
            VALUES (@Id, @Titulo, @EstaConcluido, @TarefaId);
        """;
        return Execute(sqlQuery, item);
    }

    public bool Excluir(ItemTarefa item)
    {
        string sqlQuery = """
            DELETE FROM dbo.TBItemTarefa
            WHERE Id = @Id;
        """;
        return Execute(sqlQuery, item);
    }

    public bool Editar(IEnumerable<ItemTarefa> itensExcluidos, IEnumerable<ItemTarefa> itensAdicionados, IEnumerable<ItemTarefa> itensEditados)
    {
        List<(string, object?)> comandos = [];
        (string, IEnumerable<ItemTarefa>)[] sqlItens = [
            ("""
                DELETE FROM dbo.TBItemTarefa
                WHERE Id = @Id;
            """, itensExcluidos),
            ("""
                INSERT INTO dbo.TBItemTarefa (Id, Titulo, EstaConcluido, TarefaId)
                VALUES (@Id, @Titulo, @EstaConcluido, @TarefaId);
            """, itensAdicionados),
            ("""
                UPDATE dbo.TBItemTarefa
                SET EstaConcluido = @EstaConcluido
                WHERE Id = @Id;
            """, itensEditados)
        ];

        foreach (var (sqlQuery, itens) in sqlItens)
            foreach (var it in itens)
                comandos.Add((sqlQuery, it));
                
        return Execute([.. comandos]);
    }
}

public class ItemTarefaRow
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public bool EstaConcluido { get; set; }
    public Guid TarefaId { get; set; }
}
