using eAgenda.WebApp.Compartilhado;

namespace eAgenda.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddPresentationConfig(builder.Configuration);
        builder.Services.AddServicesConfig(builder.Configuration, builder.Logging);
        builder.Services.AddRepositoriesConfig();

        var app = builder.Build();

        app.UseStaticFiles();
        app.UseRouting();
        app.MapDefaultControllerRoute();

        app.Run();  
    }
}
