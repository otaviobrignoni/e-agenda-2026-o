using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.Logging;
using eAgenda.WebApp.Compartilhado.Mapping;
using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloTarefa.Aplicacao;
using eAgenda.WebApp.ModuloTarefa.Dominio;
using eAgenda.WebApp.ModuloTarefa.Infra;
using eAgenda.WebApp.ModuloCategoria.Aplicacao;
using eAgenda.WebApp.ModuloCategoria.Dominio;
using eAgenda.WebApp.ModuloCategoria.Infra;
using eAgenda.WebApp.ModuloContato.Aplicacao;
using eAgenda.WebApp.ModuloContato.Dominio;
using eAgenda.WebApp.ModuloContato.Infra;
using eAgenda.WebApp.ModuloDespesa.Aplicacao;
using eAgenda.WebApp.ModuloDespesa.Dominio;
using eAgenda.WebApp.ModuloDespesa.Infra;

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
    public static void AddRepositoriesConfig(this IServiceCollection services)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IRepositorioGenerico, RepositorioGenerico>();

        //services.AddScoped<IRepositorio(*), Repositorio(*)>();
        services.AddScoped<IRepositorioTarefa, RepositorioTarefa>();
        services.AddScoped<IRepositorioItemTarefa, RepositorioItemTarefa>();
        services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
        services.AddScoped<IRepositorioContato, RepositorioContato>();
        services.AddScoped<IRepositorioDespesa, RepositorioDespesa>();
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
    }
}
