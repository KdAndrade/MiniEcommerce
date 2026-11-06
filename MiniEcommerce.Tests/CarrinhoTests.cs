using MiniEcommerce.Dominio;

namespace MiniEcommerce.Tests;

public class CarrinhoTests
{
    [Fact]
    public void AdicionaEAgrupaSemReservarEstoque()
    {
        var produto = new Produto("Livro", 12.50m, 5);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(produto, 2);
        carrinho.Adicionar(produto, 1);
        Assert.Equal(3, Assert.Single(carrinho.Itens).Quantidade);
        Assert.Equal(37.50m, carrinho.Subtotal);
        Assert.Equal(5, produto.Estoque);
    }

    [Fact]
    public void SomaSubtotaisDeProdutosDiferentes()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(new Produto("Livro", 10m, 2), 2);
        carrinho.Adicionar(new Produto("Caneta", 2.50m, 1), 1);
        Assert.Equal(22.50m, carrinho.Subtotal);
    }

    [Fact]
    public void RejeitaQuantidadeAcumuladaAcimaDoEstoque()
    {
        var produto = new Produto("Livro", 10m, 3);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(produto, 2);
        Assert.Throws<InvalidOperationException>(() => carrinho.Adicionar(produto, 2));
        Assert.Equal(2, Assert.Single(carrinho.Itens).Quantidade);
        Assert.Equal(20m, carrinho.Subtotal);
    }

    [Fact]
    public void RejeitaProdutoSemEstoque()
    {
        var carrinho = new CarrinhoDeCompras();
        Assert.Throws<InvalidOperationException>(() => carrinho.Adicionar(new Produto("Livro", 1m, 0), 1));
        Assert.Empty(carrinho.Itens);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void RejeitaQuantidadeInvalida(int quantidade)
    {
        var carrinho = new CarrinhoDeCompras();
        Assert.Throws<ArgumentOutOfRangeException>(() => carrinho.Adicionar(new Produto("Livro", 1m, 3), quantidade));
        Assert.Empty(carrinho.Itens);
    }

    [Fact]
    public void RemoveItemEAtualizaSubtotal()
    {
        var produto = new Produto("Livro", 10m, 3);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(produto, 2);
        carrinho.Remover(produto.Id);
        Assert.Empty(carrinho.Itens);
        Assert.Equal(0m, carrinho.Subtotal);
        Assert.Equal(3, produto.Estoque);
        Assert.Throws<KeyNotFoundException>(() => carrinho.Remover(produto.Id));
    }

    [Fact]
    public void RejeitaProdutoNulo() =>
        Assert.Throws<ArgumentNullException>(() => new CarrinhoDeCompras().Adicionar(null!, 1));
}
