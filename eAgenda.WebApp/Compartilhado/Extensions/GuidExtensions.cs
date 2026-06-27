namespace eAgenda.WebApp.Compartilhado.Extensions;

public static class GuidExtensions
{
    public static string ToShortString(this Guid guid)
    {
        return guid.ToString()[..8];      
    }
}
