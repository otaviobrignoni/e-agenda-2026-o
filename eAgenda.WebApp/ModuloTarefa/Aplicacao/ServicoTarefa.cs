using eAgenda.WebApp.ModuloTarefa.Dominio;
using FluentResults;

namespace eAgenda.WebApp.ModuloTarefa.Aplicacao;

public class ServicoTarefa(IRepositorioTarefa repositorioTarefa)
{
    public List<MostrarTarefaDto> Selecionar()
    {
        return repositorioTarefa.Registros.Select(t =>
        {
            t.Itens = repositorioTarefa.SelecionarItens(t.Id);

            return new MostrarTarefaDto(t.Titulo, t.Prioridade, t.DataCriacao, t.DataConclusao, t.EstaConcluida, t.PercentualConcluido, t.Itens, t.Id);
        }).ToList();
    }
}

