using AutoMapper;
using eAgenda.WebApp.Compartilhado.Extensions;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloCategoria.Dominio;
using eAgenda.WebApp.ModuloDespesa.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloDespesa.Aplicacao;

public class ServicoDespesa(IRepositorioDespesa repositorioDespesa, IRepositorioCategoria repositorioCategoria, IMapper mapper)    : ServicoBase<Despesa, DespesaDto>(repositorioDespesa, mapper, "Despesa não encontrada."), IServicoDespesa
{
    public override Result Cadastrar(DespesaDto dto)
    {
        var resultadoCategorias = SelecionarCategorias(dto.Categorias);

        if (resultadoCategorias.IsFailed)
            return Result.Fail(resultadoCategorias.Errors);

        var despesa = Mapper.MapWith<Despesa>(dto, (nameof(Despesa.Categorias), resultadoCategorias.Value.OrderBy(c => c.Titulo).ToList()));

        if (!repositorioDespesa.Cadastrar(despesa))
            return Result.Fail("Não foi possível cadastrar a despesa.");

        return Result.Ok();
    }

    public override Result Editar(DespesaDto dto)
    {
        var resultadoCategorias = SelecionarCategorias(dto.Categorias);

        if (resultadoCategorias.IsFailed)
            return Result.Fail(resultadoCategorias.Errors);

        var despesaEditada = Mapper.MapWith<Despesa>(dto, (nameof(Despesa.Categorias), resultadoCategorias.Value.OrderBy(c => c.Titulo).ToList()));

        if (!repositorioDespesa.Editar(dto.Id, despesaEditada))
            return Result.Fail("Despesa não encontrada.");

        return Result.Ok();
    }

    public Result<TDto> Selecionar<TDto>(Guid id) where TDto : DespesaDtoBase => SelecionarDto<TDto>(id);

    public List<TDto> Selecionar<TDto>() where TDto : DespesaDtoBase => SelecionarDto<TDto>();

    private Result<List<Categoria>> SelecionarCategorias(List<Guid> categoriaIds)
    {
        if (categoriaIds.Count == 0)
            return Falha(nameof(DespesaDto.Categorias), "Adicione pelo menos uma categoria.");

        var categorias = new List<Categoria>();

        foreach (var categoriaId in categoriaIds)
        {
            if (categoriaId == Guid.Empty)
                return Falha(nameof(DespesaDto.Categorias), "Selecione uma categoria válida.");

            var categoria = repositorioCategoria.Selecionar(categoriaId);

            if (categoria is null)
                return Falha(nameof(DespesaDto.Categorias), "Uma ou mais categorias não foram encontradas.");

            if (categorias.Any(c => c.Id == categoria.Id))
                return Falha(nameof(DespesaDto.Categorias), "Não adicione categorias repetidas.");

            categorias.Add(categoria);
        }

        return Result.Ok(categorias);
    }

}
