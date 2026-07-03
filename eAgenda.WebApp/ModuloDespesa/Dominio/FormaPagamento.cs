using System.ComponentModel.DataAnnotations;

namespace eAgenda.WebApp.ModuloDespesa.Dominio;

public enum FormaPagamento
{
    [Display(Name = "À Vista")]
    Vista,
    [Display(Name = "Crédito")]
    Credito,
    [Display(Name = "Débito")]
    Debito
}
