using AutoMapper;
using eAgenda.WebApp.Compartilhado.Extensions;
using eAgenda.WebApp.ModuloTarefa.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloTarefa.Aplicacao;

public class ServicoItemTarefa(IRepositorioTarefa repositorioTarefa, IRepositorioItemTarefa repositorioItemTarefa, IMapper mapper) :  IServicoItemTarefa
{
    public Result Cadastrar(ItemTarefaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Titulo))
            return Result.Fail("O título do item é obrigatório.");

        var tarefa = repositorioTarefa.Selecionar(dto.TarefaId);

        if (tarefa is null)
            return Result.Fail("Tarefa não encontrada.");

        var item = mapper.MapWith<ItemTarefa>(dto, (nameof(ItemTarefa.Tarefa), tarefa));

        tarefa.AdicionarItem(item);
        tarefa.AtualizarDataConclusao();

        if (!repositorioItemTarefa.Cadastrar(item))
            return Result.Fail("Não foi possível adicionar o item.");

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
        tarefa.AtualizarDataConclusao();

        if (!repositorioItemTarefa.Excluir(item))
            return Result.Fail("Não foi possível remover o item.");

        return Result.Ok();
    }

    public Result Editar(List<ItemTarefaDto> itens, Guid tarefaId)
    {
        var tarefa = repositorioTarefa.Selecionar(tarefaId);

        if (tarefa is null)
            return Result.Fail("Tarefa não encontrada.");

        var idsSubmetidos = itens
            .Select(i => i.Id)
            .Where(id => id != Guid.Empty)
            .ToHashSet();

        var itensExcluidos = tarefa.Itens
            .Where(i => !idsSubmetidos.Contains(i.Id))
            .ToList();

        var itensAdicionados = mapper.MapWith<List<ItemTarefa>>(itens.Where(i => i.Id == Guid.Empty).ToList(), (nameof(ItemTarefa.Tarefa), tarefa));

        var itensDict = tarefa.Itens.ToDictionary(i => i.Id);
        var itensEditados = new List<ItemTarefa>();

        tarefa.RemoverItens(itensExcluidos);
        tarefa.AdicionarItens(itensAdicionados);

        foreach (var dto in itens.Where(i => i.Id != Guid.Empty))
        {
            if (!itensDict.TryGetValue(dto.Id, out var item))
                return Result.Fail("Item da tarefa não encontrado.");

            if (item.EstaConcluido == dto.EstaConcluido)
                continue;

            mapper.Map(dto, item);
            itensEditados.Add(item);
        }

        tarefa.AtualizarDataConclusao();

        if (!repositorioItemTarefa.Editar(itensExcluidos, itensAdicionados, itensEditados))
            return Result.Fail("Não foi possível atualizar os itens da tarefa.");

        return Result.Ok();
    }

}
