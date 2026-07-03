using AutoMapper;
using eAgenda.WebApp.ModuloCategoria.Dominio;
using eAgenda.WebApp.ModuloDespesa.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloDespesa.Aplicacao;

public class ServicoDespesa(IRepositorioDespesa repositorioDespesa, IRepositorioCategoria repositorioCategoria, IMapper mapper)
{
    public Result Cadastrar(DespesaDto dto)
    {
        var resultadoCategorias = SelecionarCategorias(dto.Categorias);

        if (resultadoCategorias.IsFailed)
            return Result.Fail(resultadoCategorias.Errors);

        repositorioDespesa.Cadastrar(new Despesa(dto.Descricao, dto.Data, dto.Valor, dto.FormaPagamento, resultadoCategorias.Value));

        return Result.Ok();
    }

    public Result Editar(DespesaDto dto)
    {
        var resultadoCategorias = SelecionarCategorias(dto.Categorias);

        if (resultadoCategorias.IsFailed)
            return Result.Fail(resultadoCategorias.Errors);

        var despesaEditada = new Despesa(dto.Descricao, dto.Data, dto.Valor, dto.FormaPagamento, resultadoCategorias.Value);

        if (!repositorioDespesa.Editar(dto.Id, despesaEditada))
            return Result.Fail("Despesa não encontrada.");

        return Result.Ok();
    }

    public Result<DespesaDto> Selecionar(Guid id)
    {
        var despesa = repositorioDespesa.Selecionar(id);

        if (despesa is null)
            return Result.Fail("Despesa não encontrada.");

        return Result.Ok(mapper.Map<DespesaDto>(despesa));
    }

    public List<MostrarDespesaDto> Selecionar()
    {
        return mapper.Map<List<MostrarDespesaDto>>(repositorioDespesa.Registros);
    }

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

    private static Result Falha(string campo, string mensagem)
    {
        IError erro = new Error(mensagem).WithMetadata("Campo", campo);

        return Result.Fail(erro);
    }
}
