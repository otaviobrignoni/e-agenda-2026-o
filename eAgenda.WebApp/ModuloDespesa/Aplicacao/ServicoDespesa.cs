using AutoMapper;
using eAgenda.WebApp.ModuloCategoria.Dominio;
using eAgenda.WebApp.ModuloDespesa.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloDespesa.Aplicacao;

public class ServicoDespesa(IRepositorioDespesa repositorioDespesa, IRepositorioCategoria repositorioCategoria, IMapper mapper)
{
    public Result Cadastrar(DespesaDto dto)
    {
        if (dto.Categorias.Count == 0)
            return Falha(nameof(dto.Categorias), "Adicione pelo menos uma categoria.");

        var categorias = new List<Categoria>();

        foreach (var categoriaId in dto.Categorias)
        {
            if (categoriaId == Guid.Empty)
                return Falha(nameof(dto.Categorias), "Selecione uma categoria válida.");

            var categoria = repositorioCategoria.Selecionar(categoriaId);

            if (categoria is null)
                return Falha(nameof(dto.Categorias), "Uma ou mais categorias não foram encontradas.");

            if (categorias.Any(c => c.Id == categoria.Id))
                return Falha(nameof(dto.Categorias), "Não adicione categorias repetidas.");

            categorias.Add(categoria);
        }

        repositorioDespesa.Cadastrar(new Despesa(dto.Descricao, dto.Data, dto.Valor, dto.FormaPagamento, categorias));

        return Result.Ok();
    }

    public List<MostrarDespesaDto> Selecionar()
    {
        return mapper.Map<List<MostrarDespesaDto>>(repositorioDespesa.Registros);
    }

    private static Result Falha(string campo, string mensagem)
    {
        IError erro = new Error(mensagem).WithMetadata("Campo", campo);

        return Result.Fail(erro);
    }
}
