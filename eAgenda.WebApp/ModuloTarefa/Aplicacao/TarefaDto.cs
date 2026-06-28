using eAgenda.WebApp.ModuloTarefa.Dominio;

namespace eAgenda.WebApp.ModuloTarefa.Aplicacao;

public abstract record TarefaDtoBase(
    string Titulo,
    PrioridadeTarefa Prioridade,
    Guid Id
);

public record class TarefaDto : TarefaDtoBase
{
    public TarefaDto(string titulo, PrioridadeTarefa prioridade, Guid id = default) : base(titulo, prioridade, id) { }
}

public record class MostrarTarefaDto : TarefaDtoBase
{
    public MostrarTarefaDto(string titulo, PrioridadeTarefa prioridade, DateTime dataCriacao, DateTime? dataConclusao, bool estaConcluida, float percentualConcluido, List<ItemTarefa> itens, Guid id = default) : base(titulo, prioridade, id)
    {
        DataCriacao = dataCriacao;
        DataConclusao = dataConclusao;
        EstaConcluida = estaConcluida;
        PercentualConcluido = percentualConcluido;
        Itens = itens;
    }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public bool EstaConcluida { get; set; }
    public float PercentualConcluido { get; set; }
    public List<ItemTarefa> Itens { get; set; }
}
