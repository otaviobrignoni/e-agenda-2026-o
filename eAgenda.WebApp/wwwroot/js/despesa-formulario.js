const categorias = document.querySelector("#categorias");
const modeloCategoria = document.querySelector("#modelo-categoria");
const botaoAdicionarCategoria = document.querySelector("#adicionar-categoria");

function criarColunaRemover() {
  const colunaRemover = document.createElement("div");
  colunaRemover.className = "col-auto d-grid coluna-remover";

  const botaoRemover = document.createElement("button");
  botaoRemover.className = "btn btn-outline-danger remover-categoria";
  botaoRemover.type = "button";
  botaoRemover.textContent = "Remover";

  colunaRemover.append(botaoRemover);

  return colunaRemover;
}

function atualizarNomes() {
  categorias.querySelectorAll(".categoria-despesa").forEach((categoria, indice) => {
    const seletorCategoria = categoria.querySelector(".categoria");
    const validacaoCategoria = categoria.querySelector(".validacao-categoria");

    seletorCategoria.name = `Categorias[${indice}]`;
    seletorCategoria.id = `Categorias_${indice}_`;
    validacaoCategoria.dataset.valmsgFor = `Categorias[${indice}]`;
  });
}

function obterValoresCategorias() {
  return [...categorias.querySelectorAll(".categoria")]
    .map(select => select.value)
    .filter(valor => valor !== "");
}

function obterOpcoesCategoria() {
  const primeiroSelect = categorias.querySelector(".categoria");

  if (!primeiroSelect)
    return [];

  return [...primeiroSelect.options]
    .map(option => option.value)
    .filter(valor => valor !== "");
}

function selecionarPrimeiraCategoriaDisponivel(select) {
  const categoriasSelecionadas = [...categorias.querySelectorAll(".categoria")]
    .filter(outroSelect => outroSelect !== select)
    .map(outroSelect => outroSelect.value)
    .filter(valor => valor !== "");
  const primeiraOpcaoDisponivel = [...select.options].find(option => !categoriasSelecionadas.includes(option.value));

  if (primeiraOpcaoDisponivel)
    select.value = primeiraOpcaoDisponivel.value;
}

function atualizarEstadoBotaoAdicionar() {
  const quantidadeCategoriasSelecionadas = new Set(obterValoresCategorias()).size;
  const quantidadeCategoriasDisponiveis = obterOpcoesCategoria().length;

  botaoAdicionarCategoria.disabled = quantidadeCategoriasDisponiveis === 0 || quantidadeCategoriasSelecionadas >= quantidadeCategoriasDisponiveis;
}

function atualizarLayoutRemocao() {
  categorias.querySelectorAll(".categoria-despesa").forEach((categoria, indice) => {
    const campoCategoria = categoria.querySelector(".campo-categoria");
    const colunaRemover = categoria.querySelector(".coluna-remover");

    if (indice === 0) {
      campoCategoria.classList.remove("col");
      campoCategoria.classList.add("col-12");
      colunaRemover?.remove();
      return;
    }

    campoCategoria.classList.remove("col-12");
    campoCategoria.classList.add("col");

    if (!colunaRemover)
      categoria.append(criarColunaRemover());
  });
}

function atualizarOpcoesDisponiveis() {
  const categoriasSelecionadas = obterValoresCategorias();

  categorias.querySelectorAll(".categoria").forEach(select => {
    select.querySelectorAll("option").forEach(option => {
      option.disabled = option.value !== select.value &&
        categoriasSelecionadas.includes(option.value);
    });
  });

  atualizarEstadoBotaoAdicionar();
}

botaoAdicionarCategoria.addEventListener("click", () => {
  categorias.append(modeloCategoria.content.cloneNode(true));
  const novoSeletorCategoria = categorias.querySelector(".categoria-despesa:last-child .categoria");

  selecionarPrimeiraCategoriaDisponivel(novoSeletorCategoria);
  atualizarLayoutRemocao();
  atualizarNomes();
  atualizarOpcoesDisponiveis();
});

categorias.addEventListener("change", evento => {
  if (evento.target.classList.contains("categoria"))
    atualizarOpcoesDisponiveis();
});

categorias.addEventListener("click", evento => {
  if (!evento.target.classList.contains("remover-categoria"))
    return;

  evento.target.closest(".categoria-despesa").remove();
  atualizarLayoutRemocao();
  atualizarNomes();
  atualizarOpcoesDisponiveis();
});

atualizarLayoutRemocao();
atualizarNomes();
atualizarOpcoesDisponiveis();
