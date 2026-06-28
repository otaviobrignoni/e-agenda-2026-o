using eAgenda.WebApp.ModuloTarefa.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloTarefa.Aplicacao;

public class ServicoTarefa(IRepositorioTarefa repositorioTarefa)
{
    public Result Cadastrar(TarefaDto dto)
    {
        var tarefa = new Tarefa(dto.Titulo, dto.Prioridade);
        repositorioTarefa.Cadastrar(tarefa);
        return Result.Ok();
    }

    public Result Editar(TarefaDto dto)
    {
        var tarefaEditada = new Tarefa(dto.Titulo, dto.Prioridade);
        if (!repositorioTarefa.Editar(dto.Id, tarefaEditada))
            return Result.Fail("Tarefa não encontrada.");
        return Result.Ok();
    }

    public Result<TarefaDto> Selecionar(Guid id)
    {
        var tarefa = repositorioTarefa.Selecionar(id);
        if (tarefa is null)
            return Result.Fail("Tarefa não encontrada.");
        return Result.Ok(new TarefaDto(tarefa.Titulo, tarefa.Prioridade, tarefa.Id));
    }

    public List<MostrarTarefaDto> Selecionar()
    {
        return repositorioTarefa.Registros.Select(t =>
        {
            t.Itens = repositorioTarefa.SelecionarItens(t.Id);

            return new MostrarTarefaDto(t.Titulo, t.Prioridade, t.DataCriacao, t.DataConclusao, t.EstaConcluida, t.PercentualConcluido, t.Itens, t.Id);
        }).ToList();
    }
}
