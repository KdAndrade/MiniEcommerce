namespace MiniEcommerce.Dominio;

public sealed class CarrinhoDeCompras
{
    private readonly List<ItemCarrinho> itens = [];
    public IReadOnlyList<ItemCarrinho> Itens => itens.AsReadOnly();
    public decimal Subtotal => itens.Sum(i => i.Subtotal);

    public void Adicionar(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantidade);
        var indice = itens.FindIndex(i => i.Produto.Id == produto.Id);
        var atual = indice >= 0 ? itens[indice].Quantidade : 0;
        if (quantidade > produto.Estoque - atual)
            throw new InvalidOperationException($"Estoque insuficiente para {produto.Nome}.");
        var item = new ItemCarrinho(produto, atual + quantidade);
        if (indice >= 0) itens[indice] = item;
        else itens.Add(item);
    }

    public void Remover(Guid produtoId)
    {
        if (itens.RemoveAll(i => i.Produto.Id == produtoId) == 0)
            throw new KeyNotFoundException("Produto não encontrado no carrinho.");
    }

    internal void Limpar() => itens.Clear();
}
