using eAgenda.WebApp.Compartilhado.ModuloBase;

namespace eAgenda.WebApp.ModuloContato.Dominio;

public class Contato : EntidadeBase<Contato>
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Cargo { get; set; } = null;
    public string? Empresa { get; set; } = null;

    public Contato() { }

    public Contato(string nome, string email, string telefone, string? cargo = null, string? empresa = null)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Cargo = cargo;
        Empresa = empresa;
    }

    public Contato(Guid id, string nome, string email, string telefone, string? cargo = null, string? empresa = null) : this(nome, email, telefone, cargo, empresa)
    {
        Id = id;
    }
    
    public override void Atualizar(Contato entidadeAtualizada)
    {
        Nome = entidadeAtualizada.Nome;
        Email = entidadeAtualizada.Email;
        Telefone = entidadeAtualizada.Telefone;
        Cargo = entidadeAtualizada.Cargo;
        Empresa = entidadeAtualizada.Empresa;
    }
}
