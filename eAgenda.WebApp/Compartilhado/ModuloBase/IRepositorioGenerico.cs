namespace eAgenda.WebApp.Compartilhado.ModuloBase;

public interface IRepositorioGenerico
{
    IEnumerable<T> Query<T>(string sqlQuery, Guid id);
    IEnumerable<T> Query<T>(string sqlQuery, object? parametros = null);
    T QuerySingle<T>(string sqlQuery, Guid id);
    T QuerySingle<T>(string sqlQuery, object parametros);
}
