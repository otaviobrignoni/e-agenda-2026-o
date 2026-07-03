using AutoMapper;
using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloCategoria.Dominio;
using eAgenda.WebApp.ModuloDespesa.Dominio;

namespace eAgenda.WebApp.ModuloDespesa.Infra;

public class RepositorioDespesa(ISqlConnectionFactory connectionFactory, IMapper mapper) : RepositorioSql<Despesa, Despesa>(connectionFactory, mapper), IRepositorioDespesa
{
    public List<Despesa> Registros => Selecionar();

    public void Cadastrar(Despesa despesa)
    {
        string sqlQuery = """
            INSERT INTO dbo.TBDespesa (Id, Descricao, Data, Valor, FormaPagamento)
            VALUES (@Id, @Descricao, @Data, @Valor, @FormaPagamento)
        """;

        Execute(sqlQuery, despesa);

        sqlQuery = """
            INSERT INTO dbo.TBCategoriaDespesa (CategoriaId, DespesaId)
            VALUES (@CategoriaId, @DespesaId)
        """;

        foreach (var c in despesa.Categorias)
            Execute(sqlQuery, new { CategoriaId = c.Id, DespesaId = despesa.Id });
    }

    public bool Editar(Guid id, Despesa despesaEditada)
    {
        despesaEditada.Id = id;

        string sqlQuery = """
            UPDATE dbo.TBDespesa
            SET
                Descricao = @Descricao,
                Data = @Data,
                Valor = @Valor,
                FormaPagamento = @FormaPagamento
            WHERE Id = @Id;
        """;

        bool editou = Execute(sqlQuery, despesaEditada) == 1;

        if (!editou) return false;

        sqlQuery = """
            DELETE FROM dbo.TBCategoriaDespesa
            WHERE DespesaId = @Id;
        """;

        Execute(sqlQuery, id);

        sqlQuery = """
            INSERT INTO dbo.TBCategoriaDespesa (CategoriaId, DespesaId)
            VALUES (@CategoriaId, @DespesaId)
        """;

        foreach (var c in despesaEditada.Categorias)
            Execute(sqlQuery, new { CategoriaId = c.Id, DespesaId = id });

        return editou;
    }

    public bool Excluir(Guid id)
    {
        string sqlQuery = """
            DELETE FROM dbo.TBCategoriaDespesa
            WHERE DespesaId = @Id;
        """;

        Execute(sqlQuery, id);

        sqlQuery = """
            DELETE FROM dbo.TBDespesa
            WHERE Id = @Id;
        """;

        return Execute(sqlQuery, id) == 1;
    }

    public Despesa? Selecionar(Guid id)
    {
        string sqlQuery = """
            SELECT  Id, Descricao, Data, Valor, FormaPagamento
            FROM dbo.TBDespesa
            WHERE Id = @Id
        """;

        var despesa = QuerySingle(sqlQuery, id);

        if (despesa is null) return null;

        despesa.Categorias = SelecionarCategorias(despesa.Id);

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

        var categoriaIds = Query<Guid>(sqlQuery, despesaId);

        if (!categoriaIds.Any())
            return [];

        sqlQuery = """
            SELECT Id, Titulo
            FROM dbo.TBCategoria
            WHERE Id IN @Ids
            ORDER BY Titulo;
        """;

        return [.. Query<Categoria>(sqlQuery, new { Ids = categoriaIds })];
    }
}
