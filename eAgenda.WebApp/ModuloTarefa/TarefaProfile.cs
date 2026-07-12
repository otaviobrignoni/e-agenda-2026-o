using AutoMapper;
using eAgenda.WebApp.Compartilhado.Extensions;
using eAgenda.WebApp.ModuloTarefa.Aplicacao;
using eAgenda.WebApp.ModuloTarefa.Dominio;
using eAgenda.WebApp.ModuloTarefa.Infra;

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
        CreateMap<ItemTarefaRow, ItemTarefa>()
            .ForCtorParam("titulo", opt => opt.MapFrom(src => src.Titulo))
            .ForCtorParam("tarefa", opt => opt.MapFromContext(nameof(ItemTarefa.Tarefa)))
            .ForCtorParam("estaConcluido", opt => opt.MapFrom(src => src.EstaConcluido));

        CreateMap<TarefaViewModel, TarefaDto>();
        CreateMap<TarefaDto, TarefaViewModel>();
        CreateMap<ItemTarefaViewModel, ItemTarefaDto>();
        CreateMap<ItemTarefaDto, ItemTarefaViewModel>();
        CreateMap<MostrarTarefaDto, MostrarTarefaViewModel>();
    }
}
