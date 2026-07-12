using eAgenda.WebApp.ModuloTarefa.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloTarefa.Aplicacao;

public class ServicoItemTarefa(IRepositorioTarefa repositorioTarefa, IRepositorioItemTarefa repositorioItemTarefa) :  IServicoItemTarefa
{
    public Result Cadastrar(ItemTarefaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Titulo))
            return Result.Fail("O título do item é obrigatório.");

        var tarefa = repositorioTarefa.Selecionar(dto.TarefaId);

        if (tarefa is null)
            return Result.Fail("Tarefa não encontrada.");

        var item = new ItemTarefa(dto.Titulo.Trim(), tarefa);

        tarefa.AdicionarItem(item);

        if (!repositorioItemTarefa.Cadastrar(item))
            return Result.Fail("Não foi possível adicionar o item.");

        if (!repositorioTarefa.AtualizarDataConclusao(tarefa))
            return Result.Fail("Não foi possível atualizar a tarefa.");

        return Result.Ok();
    }

    public Result Excluir(ItemTarefaDto dto)
    {
        var tarefa = repositorioTarefa.Selecionar(dto.TarefaId);

        if (tarefa is null)
            return Result.Fail("Tarefa não encontrada.");

        var item = tarefa.Itens.FirstOrDefault(i => i.Id == dto.Id);

        if (item is null)
            return Result.Fail("Item da tarefa não encontrado.");

        tarefa.RemoverItem(item);

        if (!repositorioItemTarefa.Excluir(item))
            return Result.Fail("Não foi possível remover o item.");

        if (!repositorioTarefa.AtualizarDataConclusao(tarefa))
            return Result.Fail("Não foi possível atualizar a tarefa.");

        return Result.Ok();
    }

    public Result Editar(List<ItemTarefaDto> itens, Guid tarefaId)
    {
        var tarefa = repositorioTarefa.Selecionar(tarefaId);

        if (tarefa is null)
            return Result.Fail("Tarefa não encontrada.");

        var idsSubmetidos = itens.Select(i => i.Id).Where(id => id != Guid.Empty);
        var itensExcluidos = tarefa.Itens.Where(i => !idsSubmetidos.Contains(i.Id));
        var itensAdicionados = new List<ItemTarefa>();
        var itensEditados = new List<ItemTarefa>();

        foreach (var item in itensExcluidos)
            tarefa.RemoverItem(item);

        foreach (var dto in itens.Where(i => i.Id == Guid.Empty))
        {
            var novoItem = new ItemTarefa(dto.Titulo.Trim(), tarefa, dto.EstaConcluido);
            tarefa.AdicionarItem(novoItem);
            itensAdicionados.Add(novoItem);
        }

        foreach (var dto in itens.Where(i => i.Id != Guid.Empty))
        {
            var item = tarefa.Itens.FirstOrDefault(i => i.Id == dto.Id);
            if (item is null)
                return Result.Fail("Item da tarefa não encontrado.");

            if (item.EstaConcluido == dto.EstaConcluido)
                continue;

            item.Atualizar(new ItemTarefa(item.Titulo, item.Tarefa, dto.EstaConcluido));
            itensEditados.Add(item);
        }

        if (!repositorioItemTarefa.Editar(itensExcluidos, itensAdicionados, itensEditados))
            return Result.Fail("Não foi possível atualizar os itens da tarefa.");

        if (!repositorioTarefa.AtualizarDataConclusao(tarefa))
            return Result.Fail("Não foi possível atualizar a tarefa.");

        return Result.Ok();
    }

}
