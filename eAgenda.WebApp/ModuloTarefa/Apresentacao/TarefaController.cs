using AutoMapper;
using eAgenda.WebApp.ModuloTarefa.Aplicacao;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.ModuloTarefa.Apresentacao
{
    public class TarefaController(ServicoTarefa servicoTarefa, IMapper mapper) : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var dtos = servicoTarefa.Selecionar();

            var vms = mapper.Map<List<MostrarTarefaViewModel>>(dtos);

            return View(vms);
        }

    }
}
