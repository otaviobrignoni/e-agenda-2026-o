using AutoMapper;
using eAgenda.WebApp.Compartilhado.Extensions;
using eAgenda.WebApp.ModuloContato.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.ModuloContato.Apresentacao
{
    public class ContatoController(IServicoContato servicoContato, IMapper mapper) : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var dtos = servicoContato.Selecionar();

            var vms = mapper.Map<List<ContatoViewModel>>(dtos);

            return View(vms);
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            var vm = new ContatoViewModel(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Guid.Empty);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Cadastrar(ContatoViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = mapper.Map<ContatoDto>(vm);
            var resultado = servicoContato.Cadastrar(dto);
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
            var resultado = servicoContato.Selecionar(id);

            if (resultado.IsFailed)
            {
                TempData.AddErrorMessage(resultado);
                return RedirectToAction(nameof(Index));
            }

            var vm = mapper.Map<ContatoViewModel>(resultado.Value);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Editar(ContatoViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = mapper.Map<ContatoDto>(vm);

            Result resultado = servicoContato.Editar(dto);

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
            var resultado = servicoContato.Selecionar(id);

            if (resultado.IsFailed)
            {
                TempData.AddErrorMessage(resultado);
                return RedirectToAction(nameof(Index));
            }

            var vm = mapper.Map<ContatoViewModel>(resultado.Value);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Excluir(ContatoViewModel vm)
        {
            Result resultado = servicoContato.Excluir(vm.Id);

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
            var resultado = servicoContato.Selecionar(id);

            if (resultado.IsFailed)
            {
                TempData.AddErrorMessage(resultado);
                return RedirectToAction(nameof(Index));
            }

            var vm = mapper.Map<ContatoViewModel>(resultado.Value);

            return View(vm);
        }
    }
}
