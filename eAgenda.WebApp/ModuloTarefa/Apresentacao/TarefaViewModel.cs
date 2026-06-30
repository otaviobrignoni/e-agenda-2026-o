using System.ComponentModel.DataAnnotations;
using eAgenda.WebApp.ModuloTarefa.Dominio;

namespace eAgenda.WebApp.ModuloTarefa.Apresentacao;

public record TarefaViewModel(
    [Required(ErrorMessage = "O campo \"Título\" deve ser preenchido")]
    [StringLength(100, MinimumLength = 2, ErrorMessage =" O campo \"Título\" deve conter entre 2 e 100 caracteres.")]
    string Titulo,
    [Required(ErrorMessage = "O campo \"Prioridade\" deve ser preenchido")]
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
