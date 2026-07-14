using AutoMapper;
using eAgenda.WebApp.Compartilhado.Extensions;
using eAgenda.WebApp.ModuloCompromisso.Aplicacao;
using eAgenda.WebApp.ModuloCompromisso.Apresentacao;
using eAgenda.WebApp.ModuloCompromisso.Dominio;
using eAgenda.WebApp.ModuloContato.Aplicacao;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgenda.WebApp.ModuloCompromisso;

public class CompromissoProfile : Profile
{
    public CompromissoProfile()
    {
        CreateMap<Compromisso, CompromissoDto>();
        CreateMap<CompromissoDto, Compromisso>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForCtorParam("contato", opt => opt.MapFromContext(nameof(Compromisso.Contato)));
        CreateMap<Compromisso, MostrarCompromissoDto>()
            .ForCtorParam(nameof(MostrarCompromissoDto.ContatoNome), opt => opt.MapFrom(src => src.Contato == null ? null : src.Contato.Nome));
        CreateMap<CompromissoViewModel, CompromissoDto>();
        CreateMap<CompromissoDto, CompromissoViewModel>()
            .ForCtorParam(nameof(CompromissoViewModel.ContatosSelecionaveis), opt => opt.MapFrom(_ => new List<SelectListItem>()));
        CreateMap<MostrarCompromissoDto, MostrarCompromissoViewModel>();

        CreateMap<ContatoDto, SelectListItem>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id.ToString()));
    }
}
