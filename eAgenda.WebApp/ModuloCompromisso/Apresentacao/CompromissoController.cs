using AutoMapper;
using eAgenda.WebApp.Compartilhado.Extensions;
using eAgenda.WebApp.ModuloCompromisso.Aplicacao;
using eAgenda.WebApp.ModuloCompromisso.Dominio;
using eAgenda.WebApp.ModuloContato.Aplicacao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgenda.WebApp.ModuloCompromisso.Apresentacao;

public class CompromissoController(IServicoCompromisso servicoCompromisso, IServicoContato servicoContato, IMapper mapper) : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        var dtos = servicoCompromisso.Selecionar<MostrarCompromissoDto>();

        var vms = mapper.Map<List<MostrarCompromissoViewModel>>(dtos);

        return View(vms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        var vm = new CompromissoViewModel(
            Guid.Empty,
            string.Empty,
            DateOnly.FromDateTime(DateTime.Today),
            TimeOnly.FromDateTime(DateTime.Now),
            TimeOnly.FromDateTime(DateTime.Now).AddHours(1),
            TipoCompromisso.Presencial,
            string.Empty,
            null,
            SelecionarContatos()
        );

        return View(vm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CompromissoViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm with { ContatosSelecionaveis = SelecionarContatos() });

        var dto = mapper.Map<CompromissoDto>(vm);
        var resultado = servicoCompromisso.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(vm with { ContatosSelecionaveis = SelecionarContatos() });
        }

        return RedirectToAction(nameof(Index));
    }

    private List<SelectListItem> SelecionarContatos()
    {
        var dtos = servicoContato.Selecionar();
        return mapper.Map<List<SelectListItem>>(dtos);
    }
}
