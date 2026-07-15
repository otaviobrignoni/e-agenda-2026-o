using System.Linq.Expressions;
using eAgenda.WebApp.Compartilhado.Infra.Orm;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloCategoria.Dominio;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.WebApp.ModuloCategoria.Infra;

public class RepositorioCategoriaOrm(EAgendaDbContext dbContext, ILogger<RepositorioOrm<Categoria>> logger) : RepositorioOrm<Categoria>(dbContext, logger), IRepositorioCategoria
{
    public bool PossuiDespesas(Guid id)
    {
        var categoria = Selecionar(id);

        if (categoria is null)
            return false;

        return categoria.Despesas.Count != 0;
    }

    public override Categoria? Selecionar(Guid id)
    {
        return registros
            .Include(c => c.Despesas)
            .SingleOrDefault(c => c.Id == id);
    }
    public override List<Categoria> Selecionar(Expression<Func<Categoria, bool>>? filtro = null)
    {
        return [.. registros
            .Include(c=> c.Despesas)
            .Where(filtro ?? (_ => true))
            .OrderBy(c=> c.Titulo)
        ];
    }
    public List<Categoria> Selecionar(IEnumerable<Guid> ids) => [.. Selecionar(c => ids.Contains(c.Id))];
}
