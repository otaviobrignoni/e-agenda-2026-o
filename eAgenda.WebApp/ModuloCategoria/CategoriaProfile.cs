using AutoMapper;
using eAgenda.WebApp.ModuloCategoria.Aplicacao;
using eAgenda.WebApp.ModuloCategoria.Dominio;
namespace eAgenda.WebApp.ModuloCategoria.Apresentacao;

public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        CreateMap<CategoriaViewModel, CategoriaDto>();
        CreateMap<CategoriaDto, CategoriaViewModel>();
        CreateMap<Categoria, CategoriaDto>();
        CreateMap<CategoriaDto, Categoria>();
    }
}
