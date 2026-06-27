using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace eAgenda.WebApp.Compartilhado.Logging;

public static class SerilogFactory
{
    public static Logger Create(IConfiguration config)
    {
        string caminhoAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        string caminhoDiretorio = Path.Combine(caminhoAppData, "eAgenda");

        Directory.CreateDirectory(caminhoDiretorio);

        string caminhoLogs = Path.Combine(caminhoDiretorio, "erro.log");

        var newRelicOptions = config.GetSection(NewRelicOptions.SectionName).Get<NewRelicOptions>() ?? new NewRelicOptions();

        if (string.IsNullOrWhiteSpace(newRelicOptions.LicenseKey))
            throw new InvalidOperationException("Chave de licença do NewRelic não encontrada. Configure NewRelic: LicenseKey.");

        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(caminhoLogs, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error)
            .WriteTo.NewRelicLogs(endpointUrl: newRelicOptions.EndpointUrl, applicationName: newRelicOptions.ApplicationName, licenseKey: newRelicOptions.LicenseKey);

        return loggerConfiguration.CreateLogger();
    }
}
