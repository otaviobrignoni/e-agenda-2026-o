using eAgenda.WebApp.Compartilhado.ModuloBase;
using FluentResults;

namespace eAgenda.WebApp.ModuloCompromisso.Aplicacao;

public interface IServicoCompromisso : IServico<CompromissoDto>
{
    Result<TDto> Selecionar<TDto>(Guid id) where TDto : CompromissoDtoBase;
    List<TDto> Selecionar<TDto>() where TDto : CompromissoDtoBase;
}
