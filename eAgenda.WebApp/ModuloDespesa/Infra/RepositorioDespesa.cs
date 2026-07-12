using AutoMapper;
using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloCategoria.Dominio;
using eAgenda.WebApp.ModuloDespesa.Dominio;

namespace eAgenda.WebApp.ModuloDespesa.Infra;

public class RepositorioDespesa(ISqlConnectionFactory connectionFactory, IMapper mapper, IRepositorioGenerico repositorioGenerico, IRepositorioCategoria repositorioCategoria) : RepositorioSql<Despesa, Despesa>(connectionFactory, mapper), IRepositorioDespesa
{
    public bool Cadastrar(Despesa despesa)
    {
        string sqlQuery = """
            INSERT INTO dbo.TBDespesa (Id, Descricao, Data, Valor, FormaPagamento)
            VALUES (@Id, @Descricao, @Data, @Valor, @FormaPagamento)
        """;

        List<(string, object?)> comandos = [(sqlQuery, despesa)];

        sqlQuery = """
            INSERT INTO dbo.TBCategoriaDespesa (CategoriaId, DespesaId)
            VALUES (@CategoriaId, @DespesaId)
        """;

        foreach (var c in despesa.Categorias)
            comandos.Add((sqlQuery, new { CategoriaId = c.Id, DespesaId = despesa.Id }));

        return Execute([.. comandos]);
    }

    public bool Editar(Guid id, Despesa despesaEditada)
    {
        despesaEditada.Id = id;

        string sqlDespesa = """
            UPDATE dbo.TBDespesa
            SET
                Descricao = @Descricao,
                Data = @Data,
                Valor = @Valor,
                FormaPagamento = @FormaPagamento
            WHERE Id = @Id;
        """;

        string sqlCategoria = """
            DELETE FROM dbo.TBCategoriaDespesa
            WHERE DespesaId = @Id;
        """;

        List<(string, object?)> comandos = [(sqlDespesa, despesaEditada), (sqlCategoria, new { Id = id })];

        sqlCategoria = """
            INSERT INTO dbo.TBCategoriaDespesa (CategoriaId, DespesaId)
            VALUES (@CategoriaId, @DespesaId)
        """;

        foreach (var c in despesaEditada.Categorias)
            comandos.Add((sqlCategoria, new { CategoriaId = c.Id, DespesaId = id }));

        return Execute([.. comandos]);
    }

    public bool Excluir(Guid id)
    {
        string sqlCategorias = """
            DELETE FROM dbo.TBCategoriaDespesa
            WHERE DespesaId = @Id;
        """;

        string sqlDespesa = """
            DELETE FROM dbo.TBDespesa
            WHERE Id = @Id;
        """;

        return Execute((sqlCategorias, new { Id = id }), (sqlDespesa, new { Id = id }));
    }

    public Despesa? Selecionar(Guid id)
    {
        string sqlQuery = """
            SELECT  Id, Descricao, Data, Valor, FormaPagamento
            FROM dbo.TBDespesa
            WHERE Id = @Id
        """;

        var despesa = QuerySingle(sqlQuery, id);

        despesa?.Categorias = SelecionarCategorias(despesa.Id);

        return despesa;
    }

    public List<Despesa> Selecionar(Func<Despesa, bool>? filtro = null)
    {
        string sqlQuery = """
            SELECT Id, Descricao, Data, Valor, FormaPagamento
            FROM dbo.TBDespesa
            ORDER BY Descricao;
        """;

        var despesas = Query(sqlQuery).ToList();

        foreach (var despesa in despesas)
            despesa.Categorias = SelecionarCategorias(despesa.Id);

        return [.. despesas.Where(filtro ?? (_ => true))];
    }

    private List<Categoria> SelecionarCategorias(Guid despesaId)
    {
        string sqlQuery = """
            SELECT CategoriaId
            FROM dbo.TBCategoriaDespesa
            WHERE DespesaId = @Id;
        """;

        var categoriaIds = repositorioGenerico.Query<Guid>(sqlQuery, despesaId);

        return repositorioCategoria.Selecionar(categoriaIds);
    }
}
