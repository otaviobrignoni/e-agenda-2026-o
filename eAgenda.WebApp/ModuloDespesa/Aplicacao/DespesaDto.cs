using eAgenda.WebApp.ModuloCategoria.Aplicacao;
using eAgenda.WebApp.ModuloDespesa.Dominio;

namespace eAgenda.WebApp.ModuloDespesa.Aplicacao;

public abstract record DespesaDtoBase(
    Guid Id,
    string Descricao,
    DateTime Data,
    decimal Valor,
    FormaPagamento FormaPagamento
);

public record DespesaDto(
    Guid Id,
    string Descricao,
    DateTime Data,
    decimal Valor,
    FormaPagamento FormaPagamento,
    List<Guid> Categorias
) : DespesaDtoBase(Id, Descricao, Data, Valor, FormaPagamento);

public record MostrarDespesaDto(
    Guid Id,
    string Descricao,
    DateTime Data,
    decimal Valor,
    FormaPagamento FormaPagamento,
    List<CategoriaDto> Categorias
) : DespesaDtoBase(Id, Descricao, Data, Valor, FormaPagamento);
