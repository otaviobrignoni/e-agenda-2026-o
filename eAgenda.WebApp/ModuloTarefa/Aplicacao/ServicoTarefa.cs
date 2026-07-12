using AutoMapper;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloTarefa.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloTarefa.Aplicacao;

public class ServicoTarefa(IRepositorioTarefa repositorioTarefa, IMapper mapper)
    : ServicoBase<Tarefa, TarefaDto>(repositorioTarefa, mapper, "Tarefa não encontrada."), IServicoTarefa
{
    public override Result Cadastrar(TarefaDto dto)
    {
        var tarefa = Mapper.Map<Tarefa>(dto);
        if (!repositorioTarefa.Cadastrar(tarefa))
            return Result.Fail("Não foi possível cadastrar a tarefa.");

        return Result.Ok();
    }

    public override Result Editar(TarefaDto dto)
    {
        var tarefaEditada = Mapper.Map<Tarefa>(dto);
        if (!repositorioTarefa.Editar(dto.Id, tarefaEditada))
            return Result.Fail("Tarefa não encontrada.");
        return Result.Ok();
    }

    public Result<TDto> Selecionar<TDto>(Guid id) where TDto : TarefaDtoBase => SelecionarDto<TDto>(id);

    public List<TDto> Selecionar<TDto>() where TDto : TarefaDtoBase => SelecionarDto<TDto>();
}
