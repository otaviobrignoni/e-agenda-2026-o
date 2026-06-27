using Serilog;

namespace eAgenda.WebApp.Compartilhado.Logging;

public static class SerilogLoggerFactory
{
    public static void AddSerilogLogger(this IServiceCollection services, IConfiguration config, ILoggingBuilder logging)
    {
        Log.Logger = SerilogFactory.Create(config);

        logging.ClearProviders();
        
        services.AddSerilog(Log.Logger);
    }
}
