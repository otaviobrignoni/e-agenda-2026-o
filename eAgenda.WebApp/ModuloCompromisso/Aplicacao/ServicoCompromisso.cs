using AutoMapper;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloCompromisso.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloCompromisso.Aplicacao;

public class ServicoCompromisso(IRepositorioCompromisso repositorioCompromisso, IMapper mapper) : ServicoBase<Compromisso, CompromissoDto>(repositorioCompromisso, mapper, "Compromisso não encontrado"), IServicoCompromisso
{
    public override Result Cadastrar(CompromissoDto dto)
    {
        throw new NotImplementedException();
    }

    public override Result Editar(CompromissoDto dto)
    {
        throw new NotImplementedException();
    }
}
