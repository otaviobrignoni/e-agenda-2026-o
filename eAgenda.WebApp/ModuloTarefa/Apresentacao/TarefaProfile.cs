using AutoMapper;
using eAgenda.WebApp.ModuloTarefa.Aplicacao;
using eAgenda.WebApp.ModuloTarefa.Dominio;

namespace eAgenda.WebApp.ModuloTarefa.Apresentacao;

public class TarefaProfile : Profile
{
    public TarefaProfile()
    {   
        CreateMap<TarefaViewModel, TarefaDto>();
        CreateMap<TarefaDto, TarefaViewModel>();
        CreateMap<ItemTarefa, ItemTarefaViewModel>();
        CreateMap<MostrarTarefaDto, MostrarTarefaViewModel>();
    }
}
