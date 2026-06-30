using eAgenda.WebApp.Compartilhado.Infra;
using eAgenda.WebApp.Compartilhado.Logging;
using eAgenda.WebApp.Compartilhado.Mapping;
<<<<<<< HEAD
using eAgenda.WebApp.ModuloCategoria.Aplicacao;
using eAgenda.WebApp.ModuloCategoria.Dominio;
using eAgenda.WebApp.ModuloCategoria.Infra;
=======
>>>>>>> modulo-contato
using eAgenda.WebApp.ModuloContato.Aplicacao;
using eAgenda.WebApp.ModuloContato.Dominio;
using eAgenda.WebApp.ModuloContato.Infra;

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
<<<<<<< HEAD
        services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
=======
>>>>>>> modulo-contato
        services.AddScoped<IRepositorioContato, RepositorioContato>();
    }

    // Camada de Aplicação
    public static void AddServicesConfig(this IServiceCollection services, IConfiguration config, ILoggingBuilder logging)
    {
        services.AddSerilogLogger(config, logging);

        //services.AddScoped<Servico(*)>();
<<<<<<< HEAD
        services.AddScoped<ServicoCategoria>();
=======
>>>>>>> modulo-contato
        services.AddScoped<ServicoContato>();
    }
}
