namespace eAgenda.WebApp.Compartilhado.Models;

public record FormFieldViewModel(
    string Name,
    string Label,
    object? Value,
    string Type
);
