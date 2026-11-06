namespace MiniEcommerce.Dominio;

public sealed record ItemPedido(Guid ProdutoId, string Nome, decimal PrecoUnitario, int Quantidade)
{
    public decimal Subtotal => PrecoUnitario * Quantidade;
}

public sealed class Pedido
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTimeOffset CriadoEm { get; } = DateTimeOffset.UtcNow;
    public IReadOnlyList<ItemPedido> Itens { get; }
    public decimal Total { get; }

    private Pedido(IEnumerable<ItemCarrinho> itens)
    {
        Itens = Array.AsReadOnly(itens.Select(i => new ItemPedido(
            i.Produto.Id, i.Produto.Nome, i.Produto.Preco, i.Quantidade)).ToArray());
        Total = Itens.Sum(i => i.Subtotal);
    }

    public static Pedido Finalizar(CarrinhoDeCompras carrinho)
    {
        ArgumentNullException.ThrowIfNull(carrinho);
        if (carrinho.Itens.Count == 0)
            throw new InvalidOperationException("O carrinho está vazio.");

        // Valida todos os itens antes de alterar qualquer estoque.
        foreach (var item in carrinho.Itens)
            if (item.Quantidade > item.Produto.Estoque)
                throw new InvalidOperationException($"Estoque insuficiente para {item.Produto.Nome}.");

        var pedido = new Pedido(carrinho.Itens);
        foreach (var item in carrinho.Itens)
            item.Produto.BaixarEstoque(item.Quantidade);
        carrinho.Limpar();
        return pedido;
    }
}
