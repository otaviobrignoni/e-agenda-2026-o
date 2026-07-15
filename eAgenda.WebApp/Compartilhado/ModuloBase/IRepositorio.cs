using System.Linq.Expressions;

namespace eAgenda.WebApp.Compartilhado.ModuloBase;

public interface IRepositorio<T> where T : EntidadeBase<T>
{
    bool Cadastrar(T registro);
    bool Editar(Guid id, T registroEditado);
    bool Excluir(Guid id);
    T? Selecionar(Guid id);
    List<T> Selecionar(Expression<Func<T, bool>>? filtro = null);
}
