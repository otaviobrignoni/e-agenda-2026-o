(() => {
    const detalhesTarefa = document.querySelector('[data-url-alternar-conclusao]');

    if (!detalhesTarefa)
        return;

    const urlAlternarConclusao = detalhesTarefa.dataset.urlAlternarConclusao;
    const alertaErro = document.getElementById('mensagem-erro-item');
    const percentualConcluido = document.getElementById('percentual-concluido');
    const barraProgresso = document.getElementById('barra-progresso');
    const contadorItens = document.getElementById('contador-itens');
    const dataConclusao = document.getElementById('data-conclusao');

    const atualizarBotao = (botao, estaConcluido) => {
        const icone = botao.querySelector('i');
        const tituloItem = botao.parentElement.querySelector('[data-titulo-item]');
        const textoAcao = estaConcluido ? 'Reabrir item' : 'Concluir item';

        botao.classList.toggle('btn-success', estaConcluido);
        botao.classList.toggle('btn-outline-secondary', !estaConcluido);
        botao.setAttribute('aria-label', textoAcao);
        botao.setAttribute('title', textoAcao);

        icone.classList.toggle('bi-check-lg', estaConcluido);
        icone.classList.toggle('bi-circle', !estaConcluido);

        tituloItem.classList.toggle('text-decoration-line-through', estaConcluido);
        tituloItem.classList.toggle('text-muted', estaConcluido);
    };

    const mostrarErro = (mensagem) => {
        alertaErro.textContent = mensagem;
        alertaErro.classList.remove('d-none');
    };

    document.querySelectorAll('[data-botao-conclusao]').forEach((botao) => {
        botao.addEventListener('click', async () => {
            alertaErro.classList.add('d-none');
            botao.disabled = true;

            try {
                const resposta = await fetch(urlAlternarConclusao, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ titulo: botao.dataset.titulo })
                });

                const dados = await resposta.json();

                if (!resposta.ok) {
                    mostrarErro(dados.mensagem ?? 'Nao foi possivel atualizar o item.');
                    return;
                }

                const percentual = Number(dados.percentualConcluido).toLocaleString(undefined, { maximumFractionDigits: 0 });

                atualizarBotao(botao, dados.estaConcluido);
                percentualConcluido.textContent = `${percentual}%`;
                barraProgresso.style.width = `${dados.percentualConcluido}%`;
                barraProgresso.setAttribute('aria-valuenow', dados.percentualConcluido);
                contadorItens.textContent = `${dados.itensConcluidos} de ${dados.totalItens}`;
                dataConclusao.textContent = dados.dataConclusao;
            } catch {
                mostrarErro('Nao foi possivel atualizar o item.');
            } finally {
                botao.disabled = false;
            }
        });
    });
})();
