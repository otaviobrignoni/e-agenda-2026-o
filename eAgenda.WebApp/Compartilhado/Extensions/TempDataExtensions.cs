using FluentResults;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace eAgenda.WebApp.Compartilhado.Extensions;
// Apresentação
public static class TempDataExtensions
{
    public static void AddErrorMessage(this ITempDataDictionary tempData, ResultBase result)
    {
        tempData["MensagemErro"] = result.Errors[0].Message;
    }
}
