using AutoMapper;
using eAgenda.WebApp.ModuloTarefa.Aplicacao;

namespace eAgenda.WebApp.ModuloTarefa.Apresentacao;

public class TarefaProfile : Profile
{
    public TarefaProfile()
    {   
        CreateMap<TarefaViewModel, TarefaDto>();
        CreateMap<TarefaDto, TarefaViewModel>();
        CreateMap<MostrarTarefaDto, MostrarTarefaViewModel>();
    }
}
