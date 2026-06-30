namespace eAgenda.WebApp.ModuloContato.Apresentacao;

public record ContatoViewModel
(
    string Nome,
    string Email,
    string Telefone,
    string Cargo,
    string Empresa,
    Guid Id
);
