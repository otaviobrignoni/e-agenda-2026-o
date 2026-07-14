using eAgenda.WebApp.ModuloCompromisso.Dominio;

namespace eAgenda.WebApp.ModuloCompromisso.Aplicacao;

public abstract record class CompromissoDtoBase(
    Guid Id,
    string Assunto,
    DateOnly Data,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo,
    string LocalOuLink
);

public record class CompromissoDto(
    Guid Id,
    string Assunto,
    DateOnly Data,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo,
    string LocalOuLink,
    Guid? ContatoId
) : CompromissoDtoBase(Id, Assunto, Data, HoraInicio, HoraTermino, Tipo, LocalOuLink);

public record class MostrarCompromissoDto(
    Guid Id,
    string Assunto,
    DateOnly Data,
    TimeOnly HoraInicio,
    TimeOnly HoraTermino,
    TipoCompromisso Tipo,
    string LocalOuLink,
    string? ContatoNome
) : CompromissoDtoBase(Id, Assunto, Data, HoraInicio, HoraTermino, Tipo, LocalOuLink);
