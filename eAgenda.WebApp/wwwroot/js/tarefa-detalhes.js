(() => {
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

  document.querySelectorAll('[data-botao-conclusao]').forEach(botao => {
    botao.addEventListener('click', () => {
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
    });
  });
})();

