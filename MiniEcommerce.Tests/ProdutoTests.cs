using MiniEcommerce.Dominio;

namespace MiniEcommerce.Tests;

public class ProdutoTests
{
    [Theory]
    [InlineData(-0.01)]
    [InlineData(-100)]
    public void RejeitaPrecoNegativo(decimal preco) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new Produto("Livro", preco, 1));

    [Fact]
    public void RejeitaEstoqueNegativo() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new Produto("Livro", 10m, -1));

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void RejeitaNomeVazio(string? nome) =>
        Assert.Throws<ArgumentException>(() => new Produto(nome!, 10m, 1));

    [Fact]
    public void AceitaValoresZeroEOrganizaNome()
    {
        var produto = new Produto(" Livro ", 0m, 0);
        Assert.Equal("Livro", produto.Nome);
        Assert.Equal(0m, produto.Preco);
        Assert.Equal(0, produto.Estoque);
        Assert.NotEqual(Guid.Empty, produto.Id);
    }

    [Fact]
    public void CatalogaEConsultaProdutos()
    {
        var catalogo = new Catalogo();
        var produto = catalogo.Cadastrar("Livro", 20m, 4);
        Assert.Same(produto, catalogo.Buscar(produto.Id));
        Assert.Same(produto, Assert.Single(catalogo.Produtos));
    }

    [Fact]
    public void CadastroInvalidoNaoAlteraCatalogo()
    {
        var catalogo = new Catalogo();
        Assert.Throws<ArgumentOutOfRangeException>(() => catalogo.Cadastrar("Livro", -1m, 0));
        Assert.Empty(catalogo.Produtos);
        Assert.Throws<KeyNotFoundException>(() => catalogo.Buscar(Guid.NewGuid()));
    }
}
