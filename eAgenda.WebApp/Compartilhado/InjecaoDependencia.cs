using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.Infra.Orm;
using eAgenda.WebApp.Compartilhado.Infra.Sql;
using eAgenda.WebApp.Compartilhado.Logging;
using eAgenda.WebApp.Compartilhado.Mapping;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloCategoria.Aplicacao;
using eAgenda.WebApp.ModuloCategoria.Dominio;
using eAgenda.WebApp.ModuloCategoria.Infra;
using eAgenda.WebApp.ModuloCompromisso.Aplicacao;
using eAgenda.WebApp.ModuloCompromisso.Dominio;
using eAgenda.WebApp.ModuloCompromisso.Infra;
using eAgenda.WebApp.ModuloContato.Aplicacao;
using eAgenda.WebApp.ModuloContato.Dominio;
using eAgenda.WebApp.ModuloContato.Infra;
using eAgenda.WebApp.ModuloDespesa.Aplicacao;
using eAgenda.WebApp.ModuloDespesa.Dominio;
using eAgenda.WebApp.ModuloDespesa.Infra;
using eAgenda.WebApp.ModuloTarefa.Aplicacao;
using eAgenda.WebApp.ModuloTarefa.Dominio;
using eAgenda.WebApp.ModuloTarefa.Infra;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.WebApp.Compartilhado;

public static class InjecaoDependencia
{
    // Camada de Apresentação
    public static void AddPresentationConfig(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllersWithViews().AddRazorOptions(options =>
        {
            // Resetar a configuração padrão do MVC
            options.ViewLocationFormats.Clear();

            // Views dos módulos: /ModuloContato/Views/Index.cshtml
            // Views dos módulos: /ModuloContato/Apresentacao/Views/Index.cshtml
            options.ViewLocationFormats.Add("/Modulo{1}/Views/{0}.cshtml");
            options.ViewLocationFormats.Add("/Modulo{1}/Apresentacao/Views/{0}.cshtml");

            // Views compartilhadas: /Compartilhado/Views/_Layout.cshtml
            options.ViewLocationFormats.Add("/Compartilhado/Views/{0}.cshtml");
        });

        services.AddAutoMapper(mapperCfg =>
        {
            var opt = config.GetSection(AutoMapperOptions.SectionName).Get<AutoMapperOptions>();

            if (!string.IsNullOrWhiteSpace(opt?.LicenseKey))
                mapperCfg.LicenseKey = opt.LicenseKey;

            mapperCfg.AddMaps(typeof(Program));
        });
    }

    // Camada de Infraestrutura
    public static void AddRepositoriesConfig(this IServiceCollection services, IConfiguration config)
    {
        string? value = config["Infra:TipoPersistencia"];

        string enumValues = string.Join(", ", Enum.GetNames<TipoPersistencia>());

        string notValidMsg = $"'{value}' não é um tipo de persistência válido. Valores aceitos: {enumValues}.";

        string notFoundMsg = $"Chave \"Infra:TipoPersistencia\" não encontrada.";

        if (Enum.TryParse(value, true, out TipoPersistencia tipo))
            switch (tipo)
            {
                case TipoPersistencia.EFCore:
                    AddEFCoreConfig(services, config);
                    break;
                case TipoPersistencia.Dapper:
                    AddDapperConfig(services);
                    break;
                default:
                    throw new InvalidOperationException(notValidMsg);
            }
        else
            throw new InvalidOperationException(notFoundMsg);
    }

    public static void AddEFCoreConfig(IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<EAgendaDbContext>(options =>
        {
            string connectionStringName = "SqlServerEFCore";
            string? connectionString = config.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException($"ConnectionString \"{connectionStringName}\" não encontrada");

            options.UseSqlServer(connectionString);
        });

        //services.AddScoped<IRepositorio(*), Repositorio(*)Orm>();
        services.AddScoped<IRepositorioTarefa, RepositorioTarefaOrm>();
        services.AddScoped<IRepositorioItemTarefa, RepositorioItemTarefaOrm>();
        services.AddScoped<IRepositorioCategoria, RepositorioCategoriaOrm>();
        services.AddScoped<IRepositorioContato, RepositorioContatoOrm>();
        services.AddScoped<IRepositorioDespesa, RepositorioDespesaOrm>();
        services.AddScoped<IRepositorioCompromisso, RepositorioCompromissoOrm>();
    }

    public static void AddDapperConfig(IServiceCollection services)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IRepositorioGenerico, RepositorioGenerico>();

        //services.AddScoped<IRepositorio(*), Repositorio(*)>();
        services.AddScoped<IRepositorioTarefa, RepositorioTarefa>();
        services.AddScoped<IRepositorioItemTarefa, RepositorioItemTarefa>();
        services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
        services.AddScoped<IRepositorioContato, RepositorioContato>();
        services.AddScoped<IRepositorioDespesa, RepositorioDespesa>();
        services.AddScoped<IRepositorioCompromisso, RepositorioCompromisso>();
    }

    // Camada de Aplicação
    public static void AddServicesConfig(this IServiceCollection services, IConfiguration config, ILoggingBuilder logging)
    {
        services.AddSerilogLogger(config, logging);

        //services.AddScoped<Servico(*)>();
        services.AddScoped<IServicoTarefa, ServicoTarefa>();
        services.AddScoped<IServicoItemTarefa, ServicoItemTarefa>();
        services.AddScoped<IServicoCategoria, ServicoCategoria>();
        services.AddScoped<IServicoContato, ServicoContato>();
        services.AddScoped<IServicoDespesa, ServicoDespesa>();
        services.AddScoped<IServicoCompromisso, ServicoCompromisso>();
    }
}
