namespace MiniEcommerce.Dominio;

public sealed record ItemPedido(Guid ProdutoId, string Nome, decimal PrecoUnitario, int Quantidade)
{
    public decimal Subtotal => PrecoUnitario * Quantidade;
}
