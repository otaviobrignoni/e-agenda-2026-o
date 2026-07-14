using eAgenda.WebApp.ModuloCompromisso.Dominio;
using eAgenda.WebApp.ModuloContato.Apresentacao;

namespace eAgenda.WebApp.ModuloCompromisso.Apresentacao;

public record class CompromissoViewModel(
    Guid Id,
    string Assunto,
    DateOnly Data,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo,
    string LocalOuLink,
    ContatoViewModel? Contato
);
