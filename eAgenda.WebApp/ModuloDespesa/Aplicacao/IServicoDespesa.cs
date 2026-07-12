using eAgenda.WebApp.Compartilhado.ModuloBase;
using FluentResults;

namespace eAgenda.WebApp.ModuloDespesa.Aplicacao;

public interface IServicoDespesa : IServico<DespesaDto>
{
    Result<TDto> Selecionar<TDto>(Guid id) where TDto : DespesaDtoBase;
    List<TDto> Selecionar<TDto>() where TDto : DespesaDtoBase;
}
