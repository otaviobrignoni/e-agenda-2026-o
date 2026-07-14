using AutoMapper;
using eAgenda.WebApp.Compartilhado.Extensions;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloCompromisso.Dominio;
using eAgenda.WebApp.ModuloContato.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloCompromisso.Aplicacao;

public class ServicoCompromisso(IRepositorioCompromisso repositorioCompromisso, IRepositorioContato repositorioContato, IMapper mapper) : ServicoBase<Compromisso, CompromissoDto>(repositorioCompromisso, mapper, "Compromisso não encontrado"), IServicoCompromisso
{
    public override Result Cadastrar(CompromissoDto dto)
    {
        if (dto.HoraTermino <= dto.HoraInicio)
            return Falha(nameof(dto.HoraTermino), "A hora de término deve ser posterior à hora de início.");

        var temConflito = repositorioCompromisso.Selecionar(c => c.Data == dto.Data && dto.HoraInicio < c.HoraTermino && dto.HoraTermino > c.HoraInicio).Count != 0;

        if (temConflito)
            return Falha(nameof(dto.HoraInicio), "Já existe um compromisso cadastrado nesse período.");

        Contato? contato = null;

        if (dto.ContatoId.HasValue)
        {
            contato = repositorioContato.Selecionar(dto.ContatoId.Value);

            if (contato is null)
                return Falha(nameof(dto.ContatoId), "O contato selecionado não foi encontrado.");
        }

        var compromisso = Mapper.MapWith<Compromisso>(dto, (nameof(Compromisso.Contato), contato!));

        if (!repositorioCompromisso.Cadastrar(compromisso))
            return Result.Fail("Não foi possível cadastrar o compromisso.");

        return Result.Ok();
    }

    public override Result Editar(CompromissoDto dto)
    {
        throw new NotImplementedException();
    }

    public Result<TDto> Selecionar<TDto>(Guid id) where TDto : CompromissoDtoBase => SelecionarDto<TDto>(id);

    public List<TDto> Selecionar<TDto>() where TDto : CompromissoDtoBase => SelecionarDto<TDto>();
}
