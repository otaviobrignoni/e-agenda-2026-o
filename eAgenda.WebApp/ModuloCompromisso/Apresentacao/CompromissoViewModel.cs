using System.ComponentModel.DataAnnotations;
using eAgenda.WebApp.ModuloCompromisso.Dominio;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgenda.WebApp.ModuloCompromisso.Apresentacao;

public record class CompromissoViewModel(
    Guid Id,
    [Required(ErrorMessage = "O campo \"Assunto\" é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Assunto\" deve conter entre 2 e 100 caracteres.")]
    string Assunto,
    DateOnly Data,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo,
    [Required(ErrorMessage = "O campo \"Local ou link\" é obrigatório.")]
    [StringLength(255, ErrorMessage = "O campo \"Local ou link\" deve conter no máximo 255 caracteres.")]
    string LocalOuLink,
    Guid? ContatoId,
    [ValidateNever]
    List<SelectListItem> ContatosSelecionaveis
);

public record class MostrarCompromissoViewModel(
    Guid Id,
    string Assunto,
    DateOnly Data,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo,
    string LocalOuLink,
    string? ContatoNome
);
