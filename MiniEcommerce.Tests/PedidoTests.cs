using MiniEcommerce.Dominio;

namespace MiniEcommerce.Tests;

public class PedidoTests
{
    [Fact]
    public void CheckoutBaixaEstoqueCalculaTotalELimpaCarrinho()
    {
        var livro = new Produto("Livro", 12.50m, 5);
        var caneta = new Produto("Caneta", 2m, 3);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(livro, 2);
        carrinho.Adicionar(caneta, 3);
        var pedido = Pedido.Finalizar(carrinho);
        Assert.Equal(31m, pedido.Total);
        Assert.Equal(3, livro.Estoque);
        Assert.Equal(0, caneta.Estoque);
        Assert.Equal(2, pedido.Itens.Count);
        Assert.Empty(carrinho.Itens);
        Assert.NotEqual(Guid.Empty, pedido.Id);
        Assert.InRange(pedido.CriadoEm, DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow);
    }

    [Fact]
    public void RejeitaCarrinhoVazio() =>
        Assert.Throws<InvalidOperationException>(() => Pedido.Finalizar(new CarrinhoDeCompras()));

    [Fact]
    public void RevalidaEstoqueSemBaixaParcial()
    {
        var livro = new Produto("Livro", 10m, 5);
        var caneta = new Produto("Caneta", 2m, 2);
        var primeiro = new CarrinhoDeCompras();
        primeiro.Adicionar(livro, 2);
        primeiro.Adicionar(caneta, 2);
        var segundo = new CarrinhoDeCompras();
        segundo.Adicionar(caneta, 1);
        Pedido.Finalizar(segundo);
        Assert.Throws<InvalidOperationException>(() => Pedido.Finalizar(primeiro));
        Assert.Equal(5, livro.Estoque);
        Assert.Equal(1, caneta.Estoque);
        Assert.Equal(2, primeiro.Itens.Count);
    }

    [Fact]
    public void ResumoPermaneceIgualDepoisDeNovasCompras()
    {
        var produto = new Produto("Livro", 10m, 5);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(produto, 2);
        var pedido = Pedido.Finalizar(carrinho);
        carrinho.Adicionar(produto, 1);
        Pedido.Finalizar(carrinho);
        var item = Assert.Single(pedido.Itens);
        Assert.Equal(produto.Id, item.ProdutoId);
        Assert.Equal("Livro", item.Nome);
        Assert.Equal(10m, item.PrecoUnitario);
        Assert.Equal(2, item.Quantidade);
        Assert.Equal(20m, item.Subtotal);
        Assert.Equal(20m, pedido.Total);
    }

    [Fact]
    public void NaoPermiteFinalizarDuasVezesOMesmoCarrinho()
    {
        var produto = new Produto("Livro", 10m, 5);
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(produto, 1);
        Pedido.Finalizar(carrinho);
        Assert.Throws<InvalidOperationException>(() => Pedido.Finalizar(carrinho));
        Assert.Equal(4, produto.Estoque);
    }

    [Fact]
    public void RejeitaCarrinhoNulo() =>
        Assert.Throws<ArgumentNullException>(() => Pedido.Finalizar(null!));
}
