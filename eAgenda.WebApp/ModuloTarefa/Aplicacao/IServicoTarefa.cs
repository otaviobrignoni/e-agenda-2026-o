using eAgenda.WebApp.Compartilhado.ModuloBase;
using FluentResults;

namespace eAgenda.WebApp.ModuloTarefa.Aplicacao;

public interface IServicoTarefa : IServico<TarefaDto>
{
    Result<TDto> Selecionar<TDto>(Guid id) where TDto : TarefaDtoBase;
    List<TDto> Selecionar<TDto>() where TDto : TarefaDtoBase;
}
