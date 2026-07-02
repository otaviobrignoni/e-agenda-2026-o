using eAgenda.WebApp.ModuloTarefa.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloTarefa.Aplicacao;

public class ServicoItemTarefa(IRepositorioTarefa repositorioTarefa, IRepositorioItemTarefa repositorioItemTarefa)
{
    public Result Cadastrar(ItemTarefaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Titulo))
            return Result.Fail("O título do item é obrigatório.");

        var tarefa = repositorioTarefa.Selecionar(dto.TarefaId);

        if (tarefa is null)
            return Result.Fail("Tarefa não encontrada.");

        var item = new ItemTarefa(dto.Titulo, tarefa);

        repositorioItemTarefa.Cadastrar(item);

        tarefa.AdicionarItem(item);
        repositorioTarefa.Editar(tarefa.Id, tarefa);

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

        if (!repositorioItemTarefa.Excluir(item))
            return Result.Fail("Não foi possível remover o item.");

        tarefa.RemoverItem(item);
        repositorioTarefa.Editar(tarefa.Id, tarefa);

        return Result.Ok();
    }

    public Result EditarItens(List<ItemTarefaDto> itens, Guid tarefaId)
    {
        var tarefa = repositorioTarefa.Selecionar(tarefaId);

        if (tarefa is null)
            return Result.Fail("Tarefa não encontrada.");

        var idsSubmetidos = itens.Select(i => i.Id).Where(id => id != Guid.Empty).ToHashSet();

        foreach (var item in tarefa.Itens.Where(i => !idsSubmetidos.Contains(i.Id)).ToList())
        {
            repositorioItemTarefa.Excluir(item);
            tarefa.RemoverItem(item);
        }

        foreach (var dto in itens.Where(i => i.Id == Guid.Empty))
        {
            var novoItem = new ItemTarefa(dto.Titulo, tarefa, dto.EstaConcluido);
            repositorioItemTarefa.Cadastrar(novoItem);
            tarefa.AdicionarItem(novoItem);
        }

        foreach (var dto in itens.Where(i => i.Id != Guid.Empty))
        {
            var item = tarefa.Itens.FirstOrDefault(i => i.Id == dto.Id);
            if (item is null) continue;
            item.Atualizar(new ItemTarefa(item.Titulo, item.Tarefa, dto.EstaConcluido));
            repositorioItemTarefa.Editar(item);
        }

        repositorioTarefa.Editar(tarefa.Id, tarefa);

        return Result.Ok();
    }
}
