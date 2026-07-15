using System.Linq.Expressions;
using AutoMapper;
using eAgenda.WebApp.Compartilhado.Infra.Sql;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloCompromisso.Dominio;
using eAgenda.WebApp.ModuloContato.Dominio;

namespace eAgenda.WebApp.ModuloCompromisso.Infra;

public class RepositorioCompromisso(ISqlConnectionFactory connectionFactory, IMapper mapper) : RepositorioSql<Compromisso, CompromissoRow>(connectionFactory, mapper), IRepositorioCompromisso
{
    public bool Cadastrar(Compromisso compromisso)
    {
        string sqlQuery = """
            INSERT INTO dbo.TBCompromisso (Id, Assunto, Data, HoraInicio, HoraTermino, Tipo, LocalOuLink, ContatoId)
            VALUES (@Id, @Assunto, @Data, @HoraInicio, @HoraTermino, @Tipo, @LocalOuLink, @ContatoId)
        """;

        var row = Mapper.Map<CompromissoRow>(compromisso);

        return Execute(sqlQuery, row);
    }

    public bool Editar(Guid id, Compromisso compromissoEditado)
    {
        string sqlQuery = """
            UPDATE dbo.TBCompromisso
            SET
                Assunto = @Assunto,
                Data = @Data,
                HoraInicio = @HoraInicio,
                HoraTermino = @HoraTermino,
                Tipo = @Tipo,
                LocalOuLink = @LocalOuLink,
                ContatoId = @ContatoId
            WHERE Id = @Id;
        """;

        var row = Mapper.Map<CompromissoRow>(compromissoEditado);
        row.Id = id;

        return Execute(sqlQuery, row);
    }

    public bool Excluir(Guid id)
    {
        string sqlQuery = """
            DELETE FROM dbo.TBCompromisso
            WHERE Id = @Id;
        """;

        return Execute(sqlQuery, id);
    }

    public Compromisso? Selecionar(Guid id)
    {
        string sqlQuery = """
            SELECT
                compromisso.Id,
                compromisso.Assunto,
                compromisso.Data,
                compromisso.HoraInicio,
                compromisso.HoraTermino,
                compromisso.Tipo,
                compromisso.LocalOuLink,
                contato.Id AS ContatoId,
                contato.Nome AS ContatoNome,
                contato.Email AS ContatoEmail,
                contato.Telefone AS ContatoTelefone,
                contato.Cargo AS ContatoCargo,
                contato.Empresa AS ContatoEmpresa
            FROM dbo.TBCompromisso AS compromisso
            LEFT JOIN dbo.TBContato AS contato
                ON contato.Id = compromisso.ContatoId
            Where compromisso.Id = @Id;
        """;

        return QuerySingle(sqlQuery, id);
    }

    public List<Compromisso> Selecionar(Expression<Func<Compromisso, bool>>? filtro = null)
    {
        string sqlQuery = """
            SELECT
                compromisso.Id,
                compromisso.Assunto,
                compromisso.Data,
                compromisso.HoraInicio,
                compromisso.HoraTermino,
                compromisso.Tipo,
                compromisso.LocalOuLink,
                contato.Id AS ContatoId,
                contato.Nome AS ContatoNome,
                contato.Email AS ContatoEmail,
                contato.Telefone AS ContatoTelefone,
                contato.Cargo AS ContatoCargo,
                contato.Empresa AS ContatoEmpresa
            FROM dbo.TBCompromisso AS compromisso
            LEFT JOIN dbo.TBContato AS contato
                ON contato.Id = compromisso.ContatoId
            ORDER BY compromisso.Assunto;
        """;

        return [.. Query(sqlQuery).Where(filtro?.Compile() ?? (_ => true))];
    }
}

public sealed class CompromissoRow
{
    public Guid Id { get; set; }
    public string Assunto { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraTermino { get; set; }
    public TipoCompromisso Tipo { get; set; }
    public string LocalOuLink { get; set; } = string.Empty;
    public Guid? ContatoId { get; set; }
    public string? ContatoNome { get; set; }
    public string? ContatoEmail { get; set; }
    public string? ContatoTelefone { get; set; }
    public string? ContatoCargo { get; set; }
    public string? ContatoEmpresa { get; set; }

    public Contato? ExtrairContato()
    {
        return ContatoId is null ? null : new Contato(ContatoId.Value, ContatoNome!, ContatoEmail!, ContatoTelefone!, ContatoCargo, ContatoEmpresa);
    }

    public CompromissoRow() { }
}

public class CompromissoSqlProfile : Profile
{
    public CompromissoSqlProfile()
    {
        CreateMap<Compromisso, CompromissoRow>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data.ToDateTime(TimeOnly.MinValue)))
            .ForMember(dest => dest.HoraInicio, opt => opt.MapFrom(src => src.HoraInicio.ToTimeSpan()))
            .ForMember(dest => dest.HoraTermino, opt => opt.MapFrom(src => src.HoraTermino.ToTimeSpan()));
        CreateMap<CompromissoRow, Compromisso>()
            .ForCtorParam("data", opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Data)))
            .ForCtorParam("horaInicio", opt => opt.MapFrom(src => TimeOnly.FromTimeSpan(src.HoraInicio)))
            .ForCtorParam("horaTermino", opt => opt.MapFrom(src => TimeOnly.FromTimeSpan(src.HoraTermino)))
            .ForCtorParam("contato", opt => opt.MapFrom(src => src.ExtrairContato()));
    }
}
