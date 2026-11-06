namespace MiniEcommerce.Dominio;

public sealed class Catalogo
{
    private readonly List<Produto> produtos = [];
    public IReadOnlyList<Produto> Produtos => produtos.AsReadOnly();

    public Produto Cadastrar(string nome, decimal preco, int estoque)
    {
        var produto = new Produto(nome, preco, estoque);
        produtos.Add(produto);
        return produto;
    }

    public Produto Buscar(Guid id) => produtos.Find(p => p.Id == id)
        ?? throw new KeyNotFoundException("Produto não encontrado no catálogo.");
}
