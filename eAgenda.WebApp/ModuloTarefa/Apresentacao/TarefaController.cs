using AutoMapper;
using eAgenda.WebApp.Compartilhado.Extensions;
using eAgenda.WebApp.ModuloTarefa.Aplicacao;
using eAgenda.WebApp.ModuloTarefa.Dominio;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApp.ModuloTarefa.Apresentacao
{
    public class TarefaController(ServicoTarefa servicoTarefa, ServicoItemTarefa servicoItemTarefa, IMapper mapper) : Controller
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

        [HttpGet]
        public ActionResult Excluir(Guid id)
        {
            var resultado = servicoTarefa.SelecionarMostrar(id);

            if (resultado.IsFailed)
            {
                TempData.AddErrorMessage(resultado);
                return RedirectToAction(nameof(Index));
            }

            var vm = mapper.Map<MostrarTarefaViewModel>(resultado.Value);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Excluir(MostrarTarefaViewModel vm)
        {
            Result resultado = servicoTarefa.Excluir(vm.Id);

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
            var resultado = servicoTarefa.SelecionarMostrar(id);

            if (resultado.IsFailed)
            {
                TempData.AddErrorMessage(resultado);
                return RedirectToAction(nameof(Index));
            }

            var vm = mapper.Map<MostrarTarefaViewModel>(resultado.Value);

            return View(vm);
        }

        [HttpPost]
        public ActionResult AdicionarItem(ItemTarefaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData.AddErrorMessage(Result.Fail(ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault(e => !string.IsNullOrWhiteSpace(e))
                    ?? "Item da tarefa não encontrado."));
                return RedirectToAction(nameof(Detalhes), new { id = vm.TarefaId });
            }

            var resultado = servicoItemTarefa.Cadastrar(mapper.Map<ItemTarefaDto>(vm));

            if (resultado.IsFailed)
                TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Detalhes), new { id = vm.TarefaId });
        }

        [HttpPost]
        public ActionResult RemoverItem(ItemTarefaViewModel vm)
        {
            var resultado = servicoItemTarefa.Excluir(mapper.Map<ItemTarefaDto>(vm));

            if (resultado.IsFailed)
                TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Detalhes), new { id = vm.TarefaId });
        }

        [HttpPost]
        public ActionResult EditarItens(EditarItensViewModel vm)
        {
            var resultado = servicoItemTarefa.EditarItens(mapper.Map<List<ItemTarefaDto>>(vm.Itens), vm.TarefaId);

            if (resultado.IsFailed)
                TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Detalhes), new { id = vm.TarefaId });
        }
    }
}
