using eAgenda.WebApp.Compartilhado.ModuloBase;

namespace eAgenda.WebApp.ModuloTarefa.Dominio;

public interface IRepositorioTarefa : IRepositorio<Tarefa>
{
    List<ItemTarefa> SelecionarItens(Guid id);
    bool AdicionarItem(ItemTarefa item);
    bool RemoverItem(Guid tarefaId, string titulo);
    bool AlterarConclusaoItem(Guid tarefaId, string titulo, bool estaConcluido);
    bool AtualizarDataConclusao(Guid id, DateTime? dataConclusao);
}
