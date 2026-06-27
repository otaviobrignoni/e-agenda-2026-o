using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace eAgenda.WebApp.Compartilhado.Extensions;

public static class ModelStateExtensions
{
    public static void AddModelError(this ModelStateDictionary modelState, ResultBase result)
    {
        foreach (var e in result.Errors)
        {
            e.Metadata.TryGetValue("Campo", out object? campo);

            string c = campo as string ?? string.Empty;
            modelState.AddModelError(c, e.Message);
        }
    }
}
