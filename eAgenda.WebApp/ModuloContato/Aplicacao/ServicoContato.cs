using System.Diagnostics.Eventing.Reader;
using eAgenda.WebApp.ModuloContato.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloContato.Aplicacao;

public class ServicoContato(IRepositorioContato repositorioContato)
{
    public Result Cadastrar(ContatoDto dto)
    {
        string textoFalha = "Já existe um contato com esse ";

        if (repositorioContato.Selecionar().Any(c => c.Email == dto.Email && c.Telefone == dto.Telefone))
            return Falha("Email", textoFalha + "Email e Telefone");

        if (repositorioContato.Selecionar().Any(c => c.Email == dto.Email))
            return Falha("Email", textoFalha + "Email");

        if (repositorioContato.Selecionar().Any(c => c.Telefone == dto.Telefone))
            return Falha("Email", textoFalha + "Telefone");

        var contato = new Contato(dto.Nome, dto.Email, dto.Telefone, dto.Cargo, dto.Empresa);
        repositorioContato.Cadastrar(contato);
        return Result.Ok();
    }

    public Result Editar(ContatoDto dto)
    {
        string textoFalha = "Já existe um contato com esse ";

        if (repositorioContato.Selecionar(c => dto.Id != c.Id).Any(c => c.Email == dto.Email && c.Telefone == dto.Telefone))
            return Falha("Email", textoFalha + "Email e Telefone");

        if (repositorioContato.Selecionar(c => dto.Id != c.Id).Any(c => c.Email == dto.Email))
            return Falha("Email", textoFalha + "Email");

        if (repositorioContato.Selecionar(c => dto.Id != c.Id).Any(c => c.Telefone == dto.Telefone))
            return Falha("Email", textoFalha + "Telefone");

        var contatoEditado = new Contato(dto.Nome, dto.Email, dto.Telefone, dto.Cargo, dto.Empresa);
        if (!repositorioContato.Editar(dto.Id, contatoEditado))
            return Result.Fail("Contato não encontrado.");
        return Result.Ok();
    }
    public Result Excluir(Guid id)
    {
        if (!repositorioContato.Excluir(id))
            return Result.Fail("Contato não encontrado.");
        return Result.Ok();
    }

    public Result<ContatoDto> Selecionar(Guid id)
    {
        var contato = repositorioContato.Selecionar(id);
        if (contato is null)
            return Result.Fail("Contato não encontrada.");
        return Result.Ok(new ContatoDto(contato.Nome, contato.Email, contato.Telefone, contato.Cargo, contato.Empresa, contato.Id));
    }

    public List<ContatoDto> Selecionar()
    {
        return repositorioContato.Registros.Select(t =>
        {
            return new ContatoDto(t.Nome, t.Email, t.Telefone, t.Cargo, t.Empresa, t.Id);
        }).ToList();
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }
}
