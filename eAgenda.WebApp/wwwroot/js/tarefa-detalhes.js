(() => {
  const botaoSalvar = document.querySelector('button[form="form-editar-itens"]');
  const lista = document.getElementById('lista-itens');
  const inputNovoItem = document.getElementById('Titulo');
  const tarefaId = document.querySelector('#form-editar-itens [name="TarefaId"]')?.value;

  const estadoInicial = new Map(
    [...document.querySelectorAll('[data-item-concluido]')].map(i => [i, i.value])
  );

  const temAlteracoes = () => {
    const atuais = [...document.querySelectorAll('[data-item-concluido]')];
    return [...estadoInicial.entries()].some(([el, val]) => !document.contains(el) || el.value !== val)
        || atuais.some(i => !estadoInicial.has(i));
  };

  const atualizarBotaoSalvar = () => {
    if (botaoSalvar) botaoSalvar.classList.toggle('d-none', !temAlteracoes());
  };

  const atualizarProgresso = () => {
    const inputs = [...document.querySelectorAll('[data-item-concluido]')];
    const total = inputs.length;
    const concluidos = inputs.filter(i => i.value === 'true').length;
    const percentual = total === 0 ? 0 : Math.round(concluidos / total * 100);

    document.getElementById('percentual-concluido').textContent = `${percentual}%`;
    document.getElementById('barra-progresso').style.width = `${percentual}%`;
    document.getElementById('barra-progresso').setAttribute('aria-valuenow', percentual);
    document.getElementById('contador-itens').textContent = `${concluidos} de ${total}`;
  };

  document.addEventListener('click', e => {
    const botao = e.target.closest('[data-botao-conclusao]');
    if (!botao) return;

    const item = botao.closest('[data-item]');
    const input = item.querySelector('[data-item-concluido]');
    const concluido = input.value !== 'true';

    input.value = concluido.toString();
    botao.classList.toggle('btn-success', concluido);
    botao.classList.toggle('btn-outline-secondary', !concluido);
    botao.querySelector('i').classList.toggle('bi-check-lg', concluido);
    botao.querySelector('i').classList.toggle('bi-circle', !concluido);
    item.querySelector('[data-titulo-item]').classList.toggle('text-decoration-line-through', concluido);
    item.querySelector('[data-titulo-item]').classList.toggle('text-muted', concluido);

    atualizarProgresso();
    atualizarBotaoSalvar();
  });

  document.addEventListener('click', e => {
    const botao = e.target.closest('[data-botao-remover]');
    if (!botao) return;

    botao.closest('[data-item]').remove();
    atualizarProgresso();
    atualizarBotaoSalvar();
  });

  document.getElementById('btn-adicionar')?.addEventListener('click', () => {
    const titulo = inputNovoItem?.value.trim();
    if (!titulo) return;

    const key = crypto.randomUUID();
    const inp = (name, value, extra = '') =>
      `<input type="hidden" name="${name}" value="${value}" form="form-editar-itens" ${extra}/>`;

    const row = document.createElement('div');
    row.className = 'list-group-item d-flex align-items-center gap-3 py-3';
    row.dataset.item = '';
    row.innerHTML = [
      inp('Itens.Index', key),
      inp(`Itens[${key}].Id`, '00000000-0000-0000-0000-000000000000'),
      inp(`Itens[${key}].Titulo`, titulo),
      inp(`Itens[${key}].EstaConcluido`, 'false', 'data-item-concluido'),
      inp(`Itens[${key}].TarefaId`, tarefaId),
      `<button class="btn btn-outline-secondary btn-sm rounded-circle" type="button" data-botao-conclusao aria-label="Concluir item" title="Concluir item"><i class="bi bi-circle" aria-hidden="true"></i></button>`,
      `<div class="flex-grow-1" data-titulo-item>${titulo}</div>`,
      `<button class="btn btn-outline-danger btn-sm rounded-pill" type="button" data-botao-remover aria-label="Remover item" title="Remover item"><i class="bi bi-trash" aria-hidden="true"></i></button>`,
    ].join('');

    lista?.appendChild(row);
    if (inputNovoItem) inputNovoItem.value = '';
    atualizarProgresso();
    atualizarBotaoSalvar();
  });
})();


