using System.Linq.Expressions;
using eAgenda.WebApp.Compartilhado.Infra.Orm;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloDespesa.Dominio;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.WebApp.ModuloDespesa.Infra;

public class RepositorioDespesaOrm(EAgendaDbContext dbContext, ILogger<RepositorioOrm<Despesa>> logger) : RepositorioOrm<Despesa>(dbContext, logger), IRepositorioDespesa
{
    public override Despesa? Selecionar(Guid id)
    {
        return registros
            .Include(d => d.Categorias)
            .SingleOrDefault(d => d.Id == id);
    }
    public override List<Despesa> Selecionar(Expression<Func<Despesa, bool>>? filtro = null)
    {
        return [.. registros
            .Include(d=> d.Categorias)
            .Where(filtro ?? (_ => true))
            .OrderByDescending(d => d.Data)
            .ThenBy(d => d.Descricao)
        ];
    }
}
