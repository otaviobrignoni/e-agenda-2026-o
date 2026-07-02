namespace eAgenda.WebApp.ModuloTarefa.Dominio;

public interface IRepositorioItemTarefa
{
    List<ItemTarefa> Selecionar(Tarefa tarefa);
    void Cadastrar(ItemTarefa item);
    bool Excluir(ItemTarefa item);
    bool Editar(ItemTarefa item);
}
