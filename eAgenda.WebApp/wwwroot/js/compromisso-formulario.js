(() => {
    const tipo = document.getElementById("Tipo");
    const label = document.querySelector('label[for="LocalOuLink"]');

    if (!tipo || !label)
        return;

    const atualizarLabel = () => {
        label.textContent = tipo.value === "Remoto" ? "Link" : "Local";
    };

    tipo.addEventListener("change", atualizarLabel);
    atualizarLabel();
})();
