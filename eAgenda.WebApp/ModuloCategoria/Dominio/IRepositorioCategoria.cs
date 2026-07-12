using eAgenda.WebApp.Compartilhado.ModuloBase;

namespace eAgenda.WebApp.ModuloCategoria.Dominio;

public interface IRepositorioCategoria : IRepositorio<Categoria>
{
    bool PossuiDespesas(Guid id);
    List<Categoria> Selecionar(IEnumerable<Guid> ids);
}
