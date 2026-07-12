using AutoMapper;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloContato.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloContato.Aplicacao;

public class ServicoContato(IRepositorioContato repositorioContato, IMapper mapper) : ServicoBase<Contato, ContatoDto>(repositorioContato, mapper, "Contato não encontrado."), IServicoContato
{
    public override Result Cadastrar(ContatoDto dto)
    {
        string textoFalha = "Já existe um contato com esse ";
        var contatos = repositorioContato.Selecionar();
        var falhas = new List<IError>();

        if (contatos.Any(c => c.Email == dto.Email))
            falhas.Add(ErroDeCampo(nameof(dto.Email), textoFalha + "email"));

        if (contatos.Any(c => c.Telefone == dto.Telefone))
            falhas.Add(ErroDeCampo(nameof(dto.Telefone), textoFalha + "telefone"));

        if (falhas.Count > 0)
            return Result.Fail(falhas);

        var contato = Mapper.Map<Contato>(dto);
        if (!repositorioContato.Cadastrar(contato))
            return Result.Fail("Não foi possível cadastrar o contato.");

        return Result.Ok();
    }

    public override Result Editar(ContatoDto dto)
    {
        string textoFalha = "Já existe um contato com esse ";
        var outrosContatos = repositorioContato.Selecionar(c => dto.Id != c.Id);
        var falhas = new List<IError>();

        if (outrosContatos.Any(c => c.Email == dto.Email))
            falhas.Add(ErroDeCampo(nameof(dto.Email), textoFalha + "email"));

        if (outrosContatos.Any(c => c.Telefone == dto.Telefone))
            falhas.Add(ErroDeCampo(nameof(dto.Telefone), textoFalha + "telefone"));

        if (falhas.Count > 0)
            return Result.Fail(falhas);

        var contatoEditado = Mapper.Map<Contato>(dto);
        if (!repositorioContato.Editar(dto.Id, contatoEditado))
            return Result.Fail("Contato não encontrado.");
        return Result.Ok();
    }
}
