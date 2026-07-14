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
            .ForCtorParam(nameof(DespesaDto.Categorias), opt => opt.MapFrom(src => src.Categorias.Select(c => c.Id).ToList()));
        CreateMap<DespesaDto, Despesa>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Categorias, opt => opt.MapFromContext(nameof(Despesa.Categorias)));
        CreateMap<Despesa, MostrarDespesaDto>();
        CreateMap<DespesaViewModel, DespesaDto>();
        CreateMap<DespesaDto, DespesaViewModel>()
            .ForCtorParam(nameof(DespesaViewModel.CategoriasSelecionaveis), opt => opt.MapFrom(_ => new List<SelectListItem>()));
        CreateMap<MostrarDespesaDto, MostrarDespesaViewModel>();

        CreateMap<CategoriaDto, SelectListItem>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Titulo))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id.ToString()));
    }
}
