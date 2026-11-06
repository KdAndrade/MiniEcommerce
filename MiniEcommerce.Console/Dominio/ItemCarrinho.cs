namespace MiniEcommerce.Dominio;

public sealed record ItemCarrinho(Produto Produto, int Quantidade)
{
    public decimal Subtotal => Produto.Preco * Quantidade;
}
