using eAgenda.WebApp.Compartilhado.ModuloBase;

namespace eAgenda.WebApp.ModuloTarefa.Dominio;

public interface IRepositorioTarefa : IRepositorio<Tarefa>
{
    List<ItemTarefa> SelecionarItens(Guid id);
}
