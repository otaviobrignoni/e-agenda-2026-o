using FluentResults;

namespace eAgenda.WebApp.Compartilhado.ModuloBase;

public interface IServico<TDto>
{
    Result Cadastrar(TDto dto);
    Result Editar(TDto dto);
    Result Excluir(Guid id);
    Result<TDto> Selecionar(Guid id);
    List<TDto> Selecionar();
}
