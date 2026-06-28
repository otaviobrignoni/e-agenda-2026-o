namespace eAgenda.WebApp.Compartilhado.Models;

public record DisplayFieldViewModel(
    string Label, 
    object? Value
) : IViewModel;
