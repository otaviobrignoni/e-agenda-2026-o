using FluentResults;

namespace eAgenda.WebApp.ModuloTarefa.Aplicacao;

public interface IServicoItemTarefa
{
    Result Cadastrar(ItemTarefaDto dto);
    Result Excluir(ItemTarefaDto dto);
    Result Editar(List<ItemTarefaDto> itens, Guid tarefaId);
}
