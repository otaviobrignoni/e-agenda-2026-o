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
        public ActionResult AdicionarItem(Guid id, string titulo)
        {
            Result resultado = servicoTarefa.AdicionarItem(id, titulo ?? string.Empty);

            if (resultado.IsFailed)
                TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Detalhes), new { id });
        }

        [HttpPost]
        public ActionResult RemoverItem(Guid id, string titulo)
        {
            Result resultado = servicoTarefa.RemoverItem(id, titulo);

            if (resultado.IsFailed)
                TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Detalhes), new { id });
        }

        [HttpPost]
        public ActionResult AlterarConclusaoItem(Guid id, string titulo, bool estaConcluido)
        {
            Result resultado = servicoTarefa.AlterarConclusaoItem(id, titulo, estaConcluido);

            if (resultado.IsFailed)
                TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Detalhes), new { id });
        }

        [HttpPost]
        public ActionResult AlternarConclusaoItem(Guid id, [FromBody] AlternarConclusaoItemViewModel vm)
        {
            if (vm is null || string.IsNullOrWhiteSpace(vm.Titulo))
                return BadRequest(new { mensagem = "Item da tarefa não encontrado." });

            var resultado = servicoTarefa.AlternarConclusaoItem(id, vm.Titulo);

            if (resultado.IsFailed)
                return BadRequest(new { mensagem = resultado.Errors[0].Message });

            var tarefa = resultado.Value;
            var item = tarefa.Itens.First(i => i.Titulo.Equals(vm.Titulo, StringComparison.OrdinalIgnoreCase));

            return Ok(new
            {
                item.EstaConcluido,
                PercentualConcluido = Math.Clamp(tarefa.PercentualConcluido * 100, 0, 100),
                ItensConcluidos = tarefa.Itens.Count(i => i.EstaConcluido),
                TotalItens = tarefa.Itens.Count,
                DataConclusao = tarefa.DataConclusao?.ToShortDateString() ?? "Pendente"
            });
        }

    }
}
