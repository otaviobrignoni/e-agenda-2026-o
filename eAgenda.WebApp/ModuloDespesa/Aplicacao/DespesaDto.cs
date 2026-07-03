using eAgenda.WebApp.ModuloCategoria.Aplicacao;
using eAgenda.WebApp.ModuloDespesa.Dominio;

namespace eAgenda.WebApp.ModuloDespesa.Aplicacao;

public record DespesaDtoBase<T>(
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
) : DespesaDtoBase<DespesaDto>(Id, Descricao, Data, Valor, FormaPagamento);

public record MostrarDespesaDto(
    Guid Id,
    string Descricao,
    DateTime Data,
    decimal Valor,
    FormaPagamento FormaPagamento,
    List<CategoriaDto> Categorias
) : DespesaDtoBase<MostrarDespesaDto>(Id, Descricao, Data, Valor, FormaPagamento);
