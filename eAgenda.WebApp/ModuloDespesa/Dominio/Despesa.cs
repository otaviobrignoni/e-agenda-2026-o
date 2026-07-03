using eAgenda.WebApp.Compartilhado.ModuloBase;
using eAgenda.WebApp.ModuloCategoria.Dominio;

namespace eAgenda.WebApp.ModuloDespesa.Dominio;

public class Despesa : EntidadeBase<Despesa>
{
    public string Descricao { get; set; } = string.Empty;
    public DateTime Data { get; set; } = DateTime.Now;
    public decimal Valor { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public List<Categoria> Categorias { get; set; } = [];

    public Despesa() { }

    public Despesa(string descricao, decimal valor, FormaPagamento formaPagamento)
    {
        Descricao = descricao;
        Valor = valor;
        FormaPagamento = formaPagamento;
    }

    public Despesa(string descricao, decimal valor, FormaPagamento formaPagamento, IEnumerable<Categoria> categorias) : this(descricao, valor, formaPagamento)
    {
        Categorias = [.. categorias.OrderBy(c => c.Titulo)];
    }

    public Despesa(string descricao, DateTime data, decimal valor, FormaPagamento formaPagamento) : this(descricao, valor, formaPagamento)
    {
        Data = data;
    }

    public Despesa(string descricao, DateTime data, decimal valor, FormaPagamento formaPagamento, IEnumerable<Categoria> categorias) : this(descricao, data, valor, formaPagamento)
    {
        Categorias = [.. categorias.OrderBy(c => c.Titulo)];
    }

    public override void Atualizar(Despesa despesaEditada)
    {
        Descricao = despesaEditada.Descricao;
        Data = despesaEditada.Data;
        Valor = despesaEditada.Valor;
        FormaPagamento = despesaEditada.FormaPagamento;
        Categorias = despesaEditada.Categorias;
    }
}
