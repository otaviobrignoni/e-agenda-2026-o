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
        return Result.Ok(new TarefaDto(tarefa.Titulo, tarefa.Prioridade, tarefa.Id));
    }

    public Result<MostrarTarefaDto> SelecionarMostrar(Guid id)
    {
        var tarefa = repositorioTarefa.Selecionar(id);
        if (tarefa is null)
            return Result.Fail("Tarefa não encontrada.");

        tarefa.Itens = repositorioTarefa.SelecionarItens(tarefa.Id);

        return Result.Ok(new MostrarTarefaDto(tarefa.Titulo, tarefa.Prioridade, tarefa.DataCriacao, tarefa.DataConclusao, tarefa.EstaConcluida, tarefa.PercentualConcluido, tarefa.Itens, tarefa.Id));
    }

    public List<MostrarTarefaDto> Selecionar()
    {
        return repositorioTarefa.Registros.Select(t =>
        {
            t.Itens = repositorioTarefa.SelecionarItens(t.Id);
            return new MostrarTarefaDto(t.Titulo, t.Prioridade, t.DataCriacao, t.DataConclusao, t.EstaConcluida, t.PercentualConcluido, t.Itens, t.Id);
        }).ToList();
    }

    public Result AdicionarItem(Guid tarefaId, string titulo)
    {
        titulo = titulo.Trim();

        if (string.IsNullOrWhiteSpace(titulo))
            return Result.Fail("O título do item é obrigatório.");

        var resultadoTarefa = SelecionarTarefaComItens(tarefaId);

        if (resultadoTarefa.IsFailed)
            return resultadoTarefa.ToResult();

        var tarefa = resultadoTarefa.Value;

        if (tarefa.Itens.Any(i => i.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase)))
            return Result.Fail("Já existe um item com este título nesta tarefa.");

        var item = new ItemTarefa(titulo, tarefa);

        if (!repositorioTarefa.AdicionarItem(item))
            return Result.Fail("Não foi possível adicionar o item.");

        tarefa.AdicionarItem(item);
        SincronizarDataConclusao(tarefa);

        return Result.Ok();
    }

    public Result RemoverItem(Guid tarefaId, string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return Result.Fail("Item da tarefa não encontrado.");

        var resultadoTarefa = SelecionarTarefaComItens(tarefaId);

        if (resultadoTarefa.IsFailed)
            return resultadoTarefa.ToResult();

        var tarefa = resultadoTarefa.Value;
        var item = SelecionarItem(tarefa, titulo);

        if (item is null)
            return Result.Fail("Item da tarefa não encontrado.");

        if (!repositorioTarefa.RemoverItem(tarefaId, item.Titulo))
            return Result.Fail("Não foi possível remover o item.");

        tarefa.RemoverItem(item);
        SincronizarDataConclusao(tarefa);

        return Result.Ok();
    }

    public Result AlterarConclusaoItem(Guid tarefaId, string titulo, bool estaConcluido)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return Result.Fail("Item da tarefa não encontrado.");

        var resultadoTarefa = SelecionarTarefaComItens(tarefaId);

        if (resultadoTarefa.IsFailed)
            return resultadoTarefa.ToResult();

        var tarefa = resultadoTarefa.Value;
        var item = SelecionarItem(tarefa, titulo);

        if (item is null)
            return Result.Fail("Item da tarefa não encontrado.");

        if (!repositorioTarefa.AlterarConclusaoItem(tarefaId, item.Titulo, estaConcluido))
            return Result.Fail("Não foi possível atualizar o item.");

        item.EstaConcluido = estaConcluido;
        SincronizarDataConclusao(tarefa);

        return Result.Ok();
    }

    public Result<MostrarTarefaDto> AlternarConclusaoItem(Guid tarefaId, string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return Result.Fail("Item da tarefa não encontrado.");

        var resultadoTarefa = SelecionarTarefaComItens(tarefaId);

        if (resultadoTarefa.IsFailed)
            return Result.Fail(resultadoTarefa.Errors);

        var tarefa = resultadoTarefa.Value;
        var item = SelecionarItem(tarefa, titulo);

        if (item is null)
            return Result.Fail("Item da tarefa não encontrado.");

        item.EstaConcluido = !item.EstaConcluido;

        if (!repositorioTarefa.AlterarConclusaoItem(tarefaId, item.Titulo, item.EstaConcluido))
            return Result.Fail("Não foi possível atualizar o item.");

        SincronizarDataConclusao(tarefa);

        return SelecionarMostrar(tarefaId);
    }

    private Result<Tarefa> SelecionarTarefaComItens(Guid tarefaId)
    {
        var tarefa = repositorioTarefa.Selecionar(tarefaId);

        if (tarefa is null)
            return Result.Fail("Tarefa não encontrada.");

        tarefa.Itens = repositorioTarefa.SelecionarItens(tarefa.Id);

        return Result.Ok(tarefa);
    }

    private static ItemTarefa? SelecionarItem(Tarefa tarefa, string titulo)
    {
        return tarefa.Itens.FirstOrDefault(i => i.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase));
    }

    private void SincronizarDataConclusao(Tarefa tarefa)
    {
        tarefa.AtualizarDataConclusao();
        repositorioTarefa.AtualizarDataConclusao(tarefa.Id, tarefa.DataConclusao);
    }
}
