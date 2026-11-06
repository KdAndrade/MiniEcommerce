using System.Globalization;
using System.Text;
using MiniEcommerce.Dominio;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");
var catalogo = new Catalogo();
var carrinho = new CarrinhoDeCompras();
while (true)
{
    Console.WriteLine("\n=== MiniEcommerce ===\n1 - Cadastrar produto\n2 - Listar catálogo\n3 - Adicionar ao carrinho\n4 - Ver carrinho\n5 - Remover item\n6 - Finalizar pedido\n0 - Sair");
    Console.Write("Opção: ");
    var opcao = Console.ReadLine();
    if (opcao is null or "0") break;
    try
    {
        switch (opcao)
        {
            case "1":
                var nome = LerTexto("Nome: ");
                var texto = LerTexto("Preço (ex.: 19,90): ");
                if (!decimal.TryParse(texto, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                    CultureInfo.CurrentCulture, out var preco))
                    throw new ArgumentException("Preço inválido. Use vírgula para os centavos, sem separador de milhar.");
                catalogo.Cadastrar(nome, preco, LerInteiro("Quantidade em estoque: "));
                Console.WriteLine("Produto cadastrado com sucesso.");
                break;
            case "2":
                MostrarCatalogo();
                break;
            case "3":
                MostrarCatalogo();
                if (catalogo.Produtos.Count == 0) break;
                var indice = LerInteiro("Número do produto: ") - 1;
                if (indice < 0 || indice >= catalogo.Produtos.Count)
                    throw new ArgumentException("Número de produto inválido.");
                carrinho.Adicionar(catalogo.Produtos[indice], LerInteiro("Quantidade desejada: "));
                Console.WriteLine("Produto adicionado ao carrinho.");
                break;
            case "4":
                MostrarCarrinho();
                break;
            case "5":
                MostrarCarrinho();
                if (carrinho.Itens.Count == 0) break;
                var itemIndice = LerInteiro("Número do item para remover: ") - 1;
                if (itemIndice < 0 || itemIndice >= carrinho.Itens.Count)
                    throw new ArgumentException("Número de item inválido.");
                carrinho.Remover(carrinho.Itens[itemIndice].Produto.Id);
                Console.WriteLine("Item removido.");
                break;
            case "6":
                MostrarCarrinho();
                if (carrinho.Itens.Count == 0) break;
                if (!LerTexto("Confirmar compra? (s/n): ").Equals("s", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Compra não confirmada. Carrinho mantido.");
                    break;
                }
                var pedido = Pedido.Finalizar(carrinho);
                Console.WriteLine($"Pedido {pedido.Id} confirmado em {pedido.CriadoEm.ToLocalTime():g}.");
                foreach (var item in pedido.Itens)
                    Console.WriteLine($"{item.Nome}: {item.Quantidade} x {item.PrecoUnitario:C2} = {item.Subtotal:C2}");
                Console.WriteLine($"Total do pedido: {pedido.Total:C2}");
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
    catch (EndOfStreamException) { break; }
    catch (Exception erro) when (erro is ArgumentException or InvalidOperationException or KeyNotFoundException or OverflowException)
    {
        Console.WriteLine($"Não foi possível concluir: {erro.Message}");
    }
}
Console.WriteLine("Até logo!");

string LerTexto(string mensagem)
{
    Console.Write(mensagem);
    return Console.ReadLine()?.Trim() ?? throw new EndOfStreamException();
}

int LerInteiro(string mensagem) => int.TryParse(LerTexto(mensagem), out var valor)
    ? valor : throw new ArgumentException("Informe um número inteiro válido.");

void MostrarCatalogo()
{
    if (catalogo.Produtos.Count == 0) Console.WriteLine("Catálogo vazio.");
    for (var i = 0; i < catalogo.Produtos.Count; i++)
    {
        var produto = catalogo.Produtos[i];
        Console.WriteLine($"{i + 1} - {produto.Nome} | {produto.Preco:C2} | Estoque: {produto.Estoque}");
    }
}

void MostrarCarrinho()
{
    if (carrinho.Itens.Count == 0) Console.WriteLine("Carrinho vazio.");
    for (var i = 0; i < carrinho.Itens.Count; i++)
    {
        var item = carrinho.Itens[i];
        Console.WriteLine($"{i + 1} - {item.Produto.Nome} | Quantidade: {item.Quantidade} | Subtotal: {item.Subtotal:C2}");
    }
    Console.WriteLine($"Subtotal do carrinho: {carrinho.Subtotal:C2}");
}
