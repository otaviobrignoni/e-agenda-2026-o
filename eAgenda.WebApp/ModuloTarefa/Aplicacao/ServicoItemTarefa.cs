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

        if (tarefa.Itens.Any(i => i.Titulo.Equals(dto.Titulo, StringComparison.OrdinalIgnoreCase)))
            return Result.Fail("Já existe um item com este título nesta tarefa.");

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

        foreach (var dto in itens)
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
