using AutoMapper;
using eAgenda.WebApp.ModuloCompromisso.Aplicacao;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.ModuloCompromisso.Apresentacao;

public class CompromissoController(IServicoCompromisso servicoCompromisso, IMapper mapper) : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        var dtos = servicoCompromisso.Selecionar();

        var vms = mapper.Map<List<CompromissoViewModel>>(dtos);

        return View(vms);
    }
}
