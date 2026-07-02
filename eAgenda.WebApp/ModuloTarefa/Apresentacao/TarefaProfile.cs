using AutoMapper;
using eAgenda.WebApp.ModuloTarefa.Aplicacao;
using eAgenda.WebApp.ModuloTarefa.Dominio;

namespace eAgenda.WebApp.ModuloTarefa.Apresentacao;

public class TarefaProfile : Profile
{
    public TarefaProfile()
    {   
        CreateMap<TarefaDto, Tarefa>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Tarefa, TarefaDto>();
        CreateMap<Tarefa, MostrarTarefaDto>();
        CreateMap<ItemTarefa, ItemTarefaDto>();

        CreateMap<TarefaViewModel, TarefaDto>();
        CreateMap<TarefaDto, TarefaViewModel>();
        CreateMap<ItemTarefaViewModel, ItemTarefaDto>();
        CreateMap<ItemTarefaDto, ItemTarefaViewModel>();
        CreateMap<MostrarTarefaDto, MostrarTarefaViewModel>();
    }
}
