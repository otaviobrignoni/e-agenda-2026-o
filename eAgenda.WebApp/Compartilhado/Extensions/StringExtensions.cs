using System.Globalization;

namespace eAgenda.WebApp.Compartilhado.Extensions;

public static class StringExtensions
{
    public static string Capitalizar(this string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return texto;

        return char.ToUpper(texto[0], CultureInfo.CurrentCulture) + texto[1..];
    }

    public static string Truncar(this string texto, int largura)
    {
        if (largura <= 0) return string.Empty;
        if (largura == 1) return "…";

        if (texto.Length <= largura)
            return texto;

        return texto[..(largura - 1)] + "…";
    }
}
