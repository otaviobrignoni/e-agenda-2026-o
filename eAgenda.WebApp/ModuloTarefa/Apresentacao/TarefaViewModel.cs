using eAgenda.WebApp.ModuloTarefa.Dominio;

namespace eAgenda.WebApp.ModuloTarefa.Apresentacao;

public record TarefaViewModel(
    string Titulo,
    PrioridadeTarefa Prioridade,
    Guid Id = default
);
public record MostrarTarefaViewModel(
    string Titulo,
    PrioridadeTarefa Prioridade,
    DateTime DataCriacao,
    DateTime? DataConclusao,
    bool EstaConcluida,
    float PercentualConcluido,
    List<ItemTarefaViewModel> Itens,
    Guid Id
)
{
    public string ClassePrioridade => Prioridade switch
    {
        PrioridadeTarefa.Alta => "bg-danger",
        PrioridadeTarefa.Normal => "bg-primary",
        PrioridadeTarefa.Baixa => "bg-success",
        _ => "bg-secondary"
    };
};

public record ItemTarefaViewModel(
    string Titulo,
    bool EstaConcluido
);

public record AlternarConclusaoItemViewModel(
    string Titulo
);
