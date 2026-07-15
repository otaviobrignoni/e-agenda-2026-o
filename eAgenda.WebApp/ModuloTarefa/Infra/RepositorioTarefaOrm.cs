using System.Linq.Expressions;
using eAgenda.WebApp.Compartilhado.Infra.Orm;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloTarefa.Dominio;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.WebApp.ModuloTarefa.Infra;

public class RepositorioTarefaOrm(EAgendaDbContext dbContext, ILogger<RepositorioOrm<Tarefa>> logger) : RepositorioOrm<Tarefa>(dbContext, logger), IRepositorioTarefa
{
    public bool AtualizarDataConclusao(Tarefa tarefa)
    {
        try
        {
            tarefa.AtualizarDataConclusao();
            DbContext.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao atualizar data de conclusão da tarefa.");
            return false;
        }
    }
    public override Tarefa? Selecionar(Guid id)
    {
        return registros
            .Include(t => t.Itens)
            .SingleOrDefault(t => t.Id == id);
    }
    public override List<Tarefa> Selecionar(Expression<Func<Tarefa, bool>>? filtro = null)
    {
        return [.. registros
            .Include(t => t.Itens)
            .Where(filtro ?? (_ => true))
            .OrderByDescending(t => t.DataCriacao)
            .ThenBy(t => t.Titulo)
        ];
    }
}
