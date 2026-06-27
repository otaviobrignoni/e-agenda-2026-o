using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.Logging;
using eAgenda.WebApp.Compartilhado.Mapping;

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

            // Views dos módulos: /ModuloCaixa/Views/Listar.cshtml
            // Views dos módulos: /ModuloCaixa/Apresentacao/Views/Listar.cshtml
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

        //services.AddScoped<IRepositorio(*), Repositorio(*)>();
    }

    // Camada de Aplicação
    public static void AddServicesConfig(this IServiceCollection services, IConfiguration config, ILoggingBuilder logging)
    {
        services.AddSerilogLogger(config, logging);

        //services.AddScoped<Servico(*)>();
    }
}
