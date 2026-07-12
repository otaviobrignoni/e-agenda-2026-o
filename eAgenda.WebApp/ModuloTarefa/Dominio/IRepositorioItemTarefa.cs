namespace eAgenda.WebApp.ModuloTarefa.Dominio;

public interface IRepositorioItemTarefa
{
    List<ItemTarefa> Selecionar(Tarefa tarefa);
    bool Cadastrar(ItemTarefa item);
    bool Excluir(ItemTarefa item);
    bool Editar(IEnumerable<ItemTarefa> itensExcluidos, IEnumerable<ItemTarefa> itensAdicionados, IEnumerable<ItemTarefa> itensEditados);
}
