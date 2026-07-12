using AutoMapper;
using eAgenda.WebApp.Compartilhado.Extensions;
using eAgenda.WebApp.ModuloCategoria.Aplicacao;
using eAgenda.WebApp.ModuloCategoria.Dominio;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.ModuloCategoria.Apresentacao
{
    public class CategoriaController(IServicoCategoria servicoCategoria, IMapper mapper) : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var dtos = servicoCategoria.Selecionar();

            var vms = mapper.Map<List<CategoriaViewModel>>(dtos);

            return View(vms);
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            var vm = new CategoriaViewModel(string.Empty, Guid.Empty);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Cadastrar(CategoriaViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = mapper.Map<CategoriaDto>(vm);
            var resultado = servicoCategoria.Cadastrar(dto);
            if (resultado.IsFailed)
            {
                ModelState.AddModelError(resultado);
                return View(vm);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Editar(Guid id)
        {
            var resultado = servicoCategoria.Selecionar(id);

            if (resultado.IsFailed)
            {
                TempData.AddErrorMessage(resultado);
                return RedirectToAction(nameof(Index));
            }

            var vm = mapper.Map<CategoriaViewModel>(resultado.Value);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Editar(CategoriaViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = mapper.Map<CategoriaDto>(vm);

            Result resultado = servicoCategoria.Editar(dto);

            if (resultado.IsFailed)
            {
                ModelState.AddModelError(resultado);
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Excluir(Guid id)
        {
            var resultado = servicoCategoria.Selecionar(id);

            if (resultado.IsFailed)
            {
                TempData.AddErrorMessage(resultado);
                return RedirectToAction(nameof(Index));
            }

            var vm = mapper.Map<CategoriaViewModel>(resultado.Value);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Excluir(CategoriaViewModel vm)
        {
            Result resultado = servicoCategoria.Excluir(vm.Id);

            if (resultado.IsFailed)
            {
                ModelState.AddModelError(resultado);
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Detalhes(Guid id)
        {
            var resultado = servicoCategoria.Selecionar(id);

            if (resultado.IsFailed)
            {
                TempData.AddErrorMessage(resultado);
                return RedirectToAction(nameof(Index));
            }

            var vm = mapper.Map<CategoriaViewModel>(resultado.Value);

            return View(vm);
        }
    }
}
