using AutoMapper;
using eAgenda.WebApp.Compartilhado.Extensions;
using eAgenda.WebApp.ModuloTarefa.Aplicacao;
using eAgenda.WebApp.ModuloTarefa.Dominio;
using FluentResults;
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

        [HttpGet]
        public ActionResult Cadastrar()
        {
            var vm = new TarefaViewModel(string.Empty, PrioridadeTarefa.Normal);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Cadastrar(TarefaViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = mapper.Map<TarefaDto>(vm);
            var resultado = servicoTarefa.Cadastrar(dto);
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
            var resultado = servicoTarefa.Selecionar(id);

            if (resultado.IsFailed)
            {
                TempData.AddErrorMessage(resultado);
                return RedirectToAction(nameof(Index));
            }

            var vm = mapper.Map<TarefaViewModel>(resultado.Value);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Editar(TarefaViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = mapper.Map<TarefaDto>(vm);

            Result resultado = servicoTarefa.Editar(dto);

            if (resultado.IsFailed)
            {
                ModelState.AddModelError(resultado);
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
