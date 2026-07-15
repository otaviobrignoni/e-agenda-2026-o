using System.Linq.Expressions;
using eAgenda.WebApp.Compartilhado.Infra.Orm;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloContato.Dominio;

namespace eAgenda.WebApp.ModuloContato.Infra;

public class RepositorioContatoOrm(EAgendaDbContext dbContext, ILogger<RepositorioOrm<Contato>> logger) : RepositorioOrm<Contato>(dbContext, logger), IRepositorioContato
{
    public bool PossuiCompromissos(Guid id) => DbContext.Compromissos.Any(c => c.Contato != null && c.Contato.Id == id);

    public override List<Contato> Selecionar(Expression<Func<Contato, bool>>? filtro = null)
    {
        return [.. registros
            .Where(filtro ?? (_ => true))
            .OrderBy(c => c.Nome)
        ];
    }
}
