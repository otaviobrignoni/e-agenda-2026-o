using System.ComponentModel.DataAnnotations;

namespace eAgenda.WebApp.ModuloContato.Apresentacao;

public record ContatoViewModel
(
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Nome\" deve conter no máximo 100 caracteres e no minimo 2.")]
    string Nome,

    [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "\"Email\" deve ter o formato: xxxx@xxx.xxx")]
    string Email,

    [Required(ErrorMessage = "O campo \"Telefone\" é obrigatório.")]
    [RegularExpression(@"^\(\d{2}\)\s(9?\d{4})-\d{4}$", ErrorMessage = "\"Telefone\" deve ter o formato: (xx) (x)xxxx-xxxx")]
    string Telefone,

    string? Cargo,
    string? Empresa,

    Guid Id
);
