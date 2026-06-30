using eAgenda.WebApp.ModuloCategoria.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloCategoria.Aplicacao;

public class ServicoCategoria(IRepositorioCategoria repositorioCategoria)
{
    public Result Cadastrar(CategoriaDto dto)
    {
        if (repositorioCategoria.Selecionar().Any(c => c.Titulo == dto.Titulo))
            return Falha("Titulo", "Já existe uma categoria com esse titulo");

        var categoria = new Categoria(dto.Titulo);
        repositorioCategoria.Cadastrar(categoria);
        return Result.Ok();
    }

    public Result Editar(CategoriaDto dto)
    {
        var CategoriaEditado = new Categoria(dto.Titulo);
        if (!repositorioCategoria.Editar(dto.Id, CategoriaEditado))
            return Result.Fail("Categoria não encontrada.");
        return Result.Ok();
    }
    public Result Excluir(Guid id)
    {
        if (!repositorioCategoria.Excluir(id))
            return Result.Fail("Categoria não encontrado.");
        return Result.Ok();
    }

    public Result<CategoriaDto> Selecionar(Guid id)
    {
        var contato = repositorioCategoria.Selecionar(id);
        if (contato is null)
            return Result.Fail("Contato não encontrada.");
        return Result.Ok(new CategoriaDto(contato.Titulo, contato.Id));
    }

    public List<CategoriaDto> Selecionar()
    {
        return repositorioCategoria.Registros.Select(t =>
        {
            return new CategoriaDto(t.Titulo, t.Id);
        }).ToList();
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }
}
