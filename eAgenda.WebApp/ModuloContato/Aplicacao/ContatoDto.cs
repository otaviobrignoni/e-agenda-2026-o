namespace eAgenda.WebApp.ModuloContato.Aplicacao;

public record ContatoDto
(
    string Nome,
    string Email,
    string Telefone,
    string? Cargo,
    string? Empresa,
    Guid Id
);
