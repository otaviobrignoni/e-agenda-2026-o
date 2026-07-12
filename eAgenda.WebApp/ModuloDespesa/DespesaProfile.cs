using AutoMapper;
using eAgenda.WebApp.Compartilhado.Extensions;
using eAgenda.WebApp.ModuloCategoria.Aplicacao;
using eAgenda.WebApp.ModuloDespesa.Aplicacao;
using eAgenda.WebApp.ModuloDespesa.Apresentacao;
using eAgenda.WebApp.ModuloDespesa.Dominio;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgenda.WebApp.ModuloDespesa;

public class DespesaProfile : Profile
{
    public DespesaProfile()
    {
        CreateMap<Despesa, DespesaDto>()
            .ForCtorParam(nameof(DespesaDto.Categorias), config => config.MapFrom(src => src.Categorias.Select(c => c.Id).ToList()));
        CreateMap<DespesaDto, Despesa>()
            .ForMember(dest => dest.Categorias, config => config.MapFromContext(nameof(Despesa.Categorias)));
        CreateMap<Despesa, MostrarDespesaDto>();
        CreateMap<DespesaViewModel, DespesaDto>();
        CreateMap<DespesaDto, DespesaViewModel>()
            .ForCtorParam(nameof(DespesaViewModel.CategoriasSelecionaveis), config => config.MapFrom(_ => new List<SelectListItem>()));
        CreateMap<MostrarDespesaDto, MostrarDespesaViewModel>();

        CreateMap<CategoriaDto, SelectListItem>()
            .ForMember(dest => dest.Text, config => config.MapFrom(src => src.Titulo))
            .ForMember(dest => dest.Value, config => config.MapFrom(src => src.Id.ToString()));
    }
}
