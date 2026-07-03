using System.ComponentModel.DataAnnotations;
using eAgenda.WebApp.ModuloCategoria.Apresentacao;
using eAgenda.WebApp.ModuloDespesa.Dominio;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgenda.WebApp.ModuloDespesa.Apresentacao;

public record class DespesaViewModel(
    Guid Id,
    [Required(ErrorMessage = "O campo \"Descrição\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Descrição\" deve conter entre 2 e 100 caracteres.")]
    string Descricao,
    DateTime Data,
    [Required(ErrorMessage = "O campo \"Valor\" deve ser preenchido.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O campo \"Valor\" deve ser maior que zero.")]
    decimal Valor,
    FormaPagamento FormaPagamento,
    [MinLength(1, ErrorMessage = "Adicione pelo menos uma categoria.")]
    List<Guid> Categorias,
    [ValidateNever]
    List<SelectListItem> CategoriasSelecionaveis 
);

public record class MostrarDespesaViewModel(
    Guid Id,
    string Descricao,
    DateTime Data,
    decimal Valor,
    FormaPagamento FormaPagamento,
    List<CategoriaViewModel> Categorias
);

