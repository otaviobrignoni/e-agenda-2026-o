using System.Linq.Expressions;
using eAgenda.WebApp.Compartilhado.Infra.Orm;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloCompromisso.Dominio;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.WebApp.ModuloCompromisso.Infra;

public class RepositorioCompromissoOrm(EAgendaDbContext dbContext, ILogger<RepositorioOrm<Compromisso>> logger) : RepositorioOrm<Compromisso>(dbContext, logger), IRepositorioCompromisso
{
    public override Compromisso? Selecionar(Guid id)
    {
        return registros
            .Include(c => c.Contato)
            .SingleOrDefault(c => c.Id == id);
    }
    public override List<Compromisso> Selecionar(Expression<Func<Compromisso, bool>>? filtro = null)
    {
        return [..
            registros
                .Include(c => c.Contato)
                .Where(filtro ?? (_ => true))
                .OrderBy(c => c.Data)
                .ThenBy(c => c.HoraInicio)
                .ThenBy(c => c.HoraTermino)
                .ThenBy(c => c.Assunto)
        ];
    }
}
