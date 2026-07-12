using AutoMapper;
using eAgenda.WebApp.Compartilhado.Extensions;
using eAgenda.WebApp.ModuloCategoria.Aplicacao;
using eAgenda.WebApp.ModuloDespesa.Aplicacao;
using eAgenda.WebApp.ModuloDespesa.Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgenda.WebApp.ModuloDespesa.Apresentacao;

public class DespesaController(IServicoDespesa servicoDespesa, IServicoCategoria servicoCategoria, IMapper mapper) : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        var dtos = servicoDespesa.Selecionar<MostrarDespesaDto>();
        ViewBag.TemCategorias = servicoCategoria.Selecionar().Count > 0;
        var vms = mapper.Map<List<MostrarDespesaViewModel>>(dtos);
        return View(vms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        var categoriasSelecionaveis = SelecionarCategorias();
        var primeiraCategoriaId = Guid.Parse(categoriasSelecionaveis[0].Value!);

        var vm = new DespesaViewModel(default, string.Empty, DateTime.Now, 0.01m, FormaPagamento.Vista, [primeiraCategoriaId], categoriasSelecionaveis);

        return View(vm);
    }

    [HttpPost]
    public ActionResult Cadastrar(DespesaViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm with { CategoriasSelecionaveis = SelecionarCategorias() });

        var dto = mapper.Map<DespesaDto>(vm);
        var resultado = servicoDespesa.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(vm with { CategoriasSelecionaveis = SelecionarCategorias() });
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        var resultado = servicoDespesa.Selecionar<DespesaDto>(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Index));
        }

        var vm = mapper.Map<DespesaViewModel>(resultado.Value);

        return View(vm with { CategoriasSelecionaveis = SelecionarCategorias() });
    }

    [HttpPost]
    public ActionResult Editar(DespesaViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm with { CategoriasSelecionaveis = SelecionarCategorias() });

        var dto = mapper.Map<DespesaDto>(vm);
        var resultado = servicoDespesa.Editar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(vm with { CategoriasSelecionaveis = SelecionarCategorias() });
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        var resultado = servicoDespesa.Selecionar<MostrarDespesaDto>(id);
        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Index));
        }
        var vm = mapper.Map<MostrarDespesaViewModel>(resultado.Value);
        return View(vm);
    }

    [HttpPost]
    public ActionResult Excluir(MostrarDespesaViewModel vm)
    {
        var resultado = servicoDespesa.Excluir(vm.Id);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(vm);
        }

        return RedirectToAction(nameof(Index));
    }

    private List<SelectListItem> SelecionarCategorias()
    {
        var dtos = servicoCategoria.Selecionar();
        return mapper.Map<List<SelectListItem>>(dtos);
    }

}
