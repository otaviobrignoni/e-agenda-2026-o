using AutoMapper;
using FluentResults;

namespace eAgenda.WebApp.Compartilhado.ModuloBase;

public abstract class ServicoBase<TRegistro, TDto>(IRepositorio<TRegistro> repositorio, IMapper mapper, string mensagemNaoEncontrado) : IServico<TDto> where TRegistro : EntidadeBase<TRegistro>
{
    protected IRepositorio<TRegistro> Repositorio { get; } = repositorio;
    protected IMapper Mapper { get; } = mapper;

    public abstract Result Cadastrar(TDto dto);
    public abstract Result Editar(TDto dto);

    public virtual Result Excluir(Guid id)
    {
        if (!Repositorio.Excluir(id))
            return Result.Fail(mensagemNaoEncontrado);

        return Result.Ok();
    }

    public List<TDto> Selecionar()
    {
        return SelecionarDto<TDto>();
    }

    public Result<TDto> Selecionar(Guid id) => SelecionarDto<TDto>(id);

    protected List<T> SelecionarDto<T>()
    {
        return Mapper.Map<List<T>>(Repositorio.Selecionar());
    }

    protected Result<T> SelecionarDto<T>(Guid id)
    {
        var registro = Repositorio.Selecionar(id);

        if (registro is null)
            return Result.Fail(mensagemNaoEncontrado);

        return Result.Ok(Mapper.Map<T>(registro));
    }

    protected static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }

    protected static IError ErroDeCampo(string campo, string mensagem)
    {
        return new Error(mensagem).WithMetadata("Campo", campo);
    }
}
