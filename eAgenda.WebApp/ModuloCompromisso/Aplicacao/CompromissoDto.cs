using eAgenda.WebApp.ModuloCompromisso.Dominio;
using eAgenda.WebApp.ModuloContato.Aplicacao;

namespace eAgenda.WebApp.ModuloCompromisso.Aplicacao;

public record class CompromissoDto(
    Guid Id,
    string Assunto,
    DateOnly Data,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo,
    string LocalOuLink,
    ContatoDto? Contato
);
