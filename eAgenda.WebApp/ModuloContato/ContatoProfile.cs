using AutoMapper;
using eAgenda.WebApp.ModuloContato.Aplicacao;
using eAgenda.WebApp.ModuloContato.Apresentacao;
using eAgenda.WebApp.ModuloContato.Dominio;

namespace eAgenda.WebApp.ModuloContato;

public class ContatoProfile : Profile
{
    public ContatoProfile()
    {
        CreateMap<ContatoViewModel, ContatoDto>();
        CreateMap<ContatoDto, ContatoViewModel>();
        CreateMap<Contato, ContatoDto>();
        CreateMap<ContatoDto, Contato>();
    }
}
