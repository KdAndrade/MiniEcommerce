namespace MiniEcommerce.Dominio;

public sealed class Produto
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Nome { get; }
    public decimal Preco { get; }
    public int Estoque { get; private set; }

    public Produto(string nome, decimal preco, int estoque)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Informe o nome do produto.", nameof(nome));
        ArgumentOutOfRangeException.ThrowIfNegative(preco);
        ArgumentOutOfRangeException.ThrowIfNegative(estoque);
        Nome = nome.Trim();
        Preco = preco;
        Estoque = estoque;
    }

    internal void BaixarEstoque(int quantidade)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantidade);
        if (quantidade > Estoque)
            throw new InvalidOperationException($"Estoque insuficiente para {Nome}.");
        Estoque -= quantidade;
    }
}
