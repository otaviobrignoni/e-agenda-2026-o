using AutoMapper;
using eAgenda.WebApp.ModuloTarefa.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloTarefa.Aplicacao;

public class ServicoTarefa(IRepositorioTarefa repositorioTarefa, IMapper mapper)
{
    public Result Cadastrar(TarefaDto dto)
    {
        var tarefa = mapper.Map<Tarefa>(dto);
        repositorioTarefa.Cadastrar(tarefa);
        return Result.Ok();
    }

    public Result Editar(TarefaDto dto)
    {
        var tarefaEditada = mapper.Map<Tarefa>(dto);
        if (!repositorioTarefa.Editar(dto.Id, tarefaEditada))
            return Result.Fail("Tarefa não encontrada.");
        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        if (!repositorioTarefa.Excluir(id))
            return Result.Fail("Tarefa não encontrada.");
        return Result.Ok();
    }

    public Result<TarefaDto> Selecionar(Guid id)
    {
        var tarefa = repositorioTarefa.Selecionar(id);
        if (tarefa is null)
            return Result.Fail("Tarefa não encontrada.");
        return Result.Ok(mapper.Map<TarefaDto>(tarefa));
    }

    public Result<MostrarTarefaDto> SelecionarMostrar(Guid id)
    {
        var tarefa = repositorioTarefa.Selecionar(id);
        if (tarefa is null)
            return Result.Fail("Tarefa não encontrada.");
        return Result.Ok(mapper.Map<MostrarTarefaDto>(tarefa));
    }

    public List<MostrarTarefaDto> Selecionar()
    {
        return mapper.Map<List<MostrarTarefaDto>>(repositorioTarefa.Registros);
    }
}
