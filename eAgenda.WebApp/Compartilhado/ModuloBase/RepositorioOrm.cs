using System.Linq.Expressions;
using eAgenda.WebApp.Compartilhado.Infra.Orm;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.WebApp.Compartilhado.ModuloBase;

public class RepositorioOrm<T>(EAgendaDbContext dbContext, ILogger<RepositorioOrm<T>> logger) : IRepositorio<T> where T : EntidadeBase<T>
{
    protected readonly DbSet<T> registros = dbContext.Set<T>();
    protected EAgendaDbContext DbContext { get; } = dbContext;
    protected ILogger<RepositorioOrm<T>> Logger { get; } = logger;

    public bool Cadastrar(T registro)
    {
        try
        {
            registros.Add(registro);
            DbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao cadastrar {Registro}.", typeof(T).Name);
            return false;
        }
        return true;
    }

    public bool Editar(Guid id, T registroEditado)
    {
        var registro = Selecionar(id);

        if (registro is null)
            return false;

        registro.Atualizar(registroEditado);

        try
        {
            DbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao editar {Registro}.", typeof(T).Name);
            return false;
        }
        return true;
    }

    public bool Excluir(Guid id)
    {
        var registro = Selecionar(id);

        if (registro is null)
            return false;

        try
        {
            registros.Remove(registro);
            DbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao excluir {Registro}.", typeof(T).Name);
            return false;
        }

        return true;
    }

    public virtual T? Selecionar(Guid id) => registros.SingleOrDefault(r => r.Id == id);

    public virtual List<T> Selecionar(Expression<Func<T, bool>>? filtro = null) => [.. registros.Where(filtro ?? (_ => true))];
}
