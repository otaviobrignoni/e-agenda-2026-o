using AutoMapper;
using eAgenda.WebApp.ModuloCompromisso.Aplicacao;
using eAgenda.WebApp.ModuloCompromisso.Apresentacao;
using eAgenda.WebApp.ModuloCompromisso.Dominio;

namespace eAgenda.WebApp.ModuloCompromisso;

public class CompromissoProfile : Profile
{
    public CompromissoProfile()
    {
        CreateMap<Compromisso, CompromissoDto>();
        CreateMap<CompromissoDto, CompromissoViewModel>();
    }
}
