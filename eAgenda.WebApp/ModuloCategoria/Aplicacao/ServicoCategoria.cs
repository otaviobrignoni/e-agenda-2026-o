using AutoMapper;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloCategoria.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloCategoria.Aplicacao;

public class ServicoCategoria(IRepositorioCategoria repositorioCategoria, IMapper mapper) : ServicoBase<Categoria, CategoriaDto>(repositorioCategoria, mapper, "Categoria não encontrada."), IServicoCategoria
{
    public override Result Cadastrar(CategoriaDto dto)
    {
        if (repositorioCategoria.Selecionar().Any(c => c.Titulo == dto.Titulo))
            return Falha(nameof(dto.Titulo), "Já existe uma categoria com esse título.");

        var categoria = Mapper.Map<Categoria>(dto);
        if (!repositorioCategoria.Cadastrar(categoria))
            return Result.Fail("Não foi possível cadastrar a categoria.");

        return Result.Ok();
    }

    public override Result Editar(CategoriaDto dto)
    {
        if (repositorioCategoria.Selecionar(c => c.Id != dto.Id).Any(c => c.Titulo == dto.Titulo))
            return Falha(nameof(dto.Titulo), "Já existe uma categoria com esse título.");

        var categoriaEditada = Mapper.Map<Categoria>(dto);
        if (!repositorioCategoria.Editar(dto.Id, categoriaEditada))
            return Result.Fail("Categoria não encontrada.");
        return Result.Ok();
    }
    public override Result Excluir(Guid id)
    {
        if (repositorioCategoria.PossuiDespesas(id))
            return Result.Fail("A categoria está vinculada a uma ou mais despesas.");

        return base.Excluir(id);
    }
}
