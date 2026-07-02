using eAgenda.WebApp.Compartilhado.ModuloBase;

namespace eAgenda.WebApp.ModuloTarefa.Dominio;

public class Tarefa : EntidadeBase<Tarefa>
{
    public string Titulo { get; set; } = string.Empty;
    public PrioridadeTarefa Prioridade { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime? DataConclusao { get; set; }
    public bool EstaConcluida => Itens.Count != 0 && Itens.All(it => it.EstaConcluido);
    public float PercentualConcluido => Itens.Count == 0 ? 0 : Itens.Count(it => it.EstaConcluido) / (float)Itens.Count;
    public List<ItemTarefa> Itens { get; set; } = [];

    public Tarefa() {}
    public Tarefa(string titulo, PrioridadeTarefa prioridade)
    {
        Titulo = titulo;
        Prioridade = prioridade;
    }

    public override void Atualizar(Tarefa tarefaEditada)
    {
        Titulo = tarefaEditada.Titulo;
        Prioridade = tarefaEditada.Prioridade;
    }

    public void AdicionarItem(ItemTarefa item)
    {
        Itens.Add(item);
        AtualizarDataConclusao();
    }

    public void RemoverItem(ItemTarefa item)
    {
        Itens.Remove(item);
        AtualizarDataConclusao();
    }

    public void AtualizarDataConclusao()
    {
        if (EstaConcluida && DataConclusao is null)
            DataConclusao = DateTime.Now;

        else if (!EstaConcluida)
            DataConclusao = null;
    }
}
