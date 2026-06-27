using FluentResults;

namespace eAgenda.WebApp.Compartilhado.Extensions;

public static class ResultExtensions
{
    public static bool TemErroDeCampo(this ResultBase resultado)
    {
        return resultado.Errors.Any(e => e.Metadata.ContainsKey("Campo"));
    }
}
