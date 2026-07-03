using eAgenda.WebApp.ModuloCategoria.Aplicacao;
using eAgenda.WebApp.ModuloDespesa.Dominio;

namespace eAgenda.WebApp.ModuloDespesa.Aplicacao;

public record class DespesaDto(
    Guid Id,
    string Descricao,
    DateTime Data,
    decimal Valor,
    FormaPagamento FormaPagamento,
    List<Guid> Categorias
);

public record class MostrarDespesaDto(
    Guid Id,
    string Descricao,
    DateTime Data,
    decimal Valor,
    FormaPagamento FormaPagamento,
    List<CategoriaDto> Categorias
);
