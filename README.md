# MiniEcommerce

Atividade de Garantia da Qualidade de Software: aplicação de console em C# com testes xUnit, TDD básico e histórico em Conventional Commits.

## Requisitos

- SDK do .NET 10 (os projetos usam `net10.0`).
- Git para clonar o repositório.
- Acesso à internet na primeira restauração dos pacotes NuGet.

## Como executar

```sh
git clone https://github.com/KdAndrade/MiniEcommerce.git
cd MiniEcommerce
dotnet restore
dotnet build --no-restore
dotnet run --project MiniEcommerce.Console/MiniEcommerce.Console.csproj
```

## Como testar

```sh
dotnet test
```

São 23 casos de teste xUnit, incluindo os casos parametrizados. Para executar em Release:

```sh
dotnet test --configuration Release
```

## Funcionalidades e menu

| Opção | Ação |
| --- | --- |
| 1 | Cadastrar produto com nome, preço e estoque |
| 2 | Listar catálogo com preço e estoque atual |
| 3 | Adicionar produto e quantidade ao carrinho |
| 4 | Consultar itens e subtotal do carrinho |
| 5 | Remover um item completo do carrinho |
| 6 | Confirmar compra, gerar resumo e baixar estoque |
| 0 | Encerrar |

Selecione os produtos pelo número exibido no catálogo. Informe preços no formato brasileiro, como `12,50`, sem separador de milhar. Confirme o checkout com `s`; outra resposta mantém o carrinho sem efetuar a compra.

### Exemplo de uso

1. Cadastre **Livro**, preço **12,50**, estoque **5**.
2. Adicione o produto **1**, quantidade **2**, ao carrinho.
3. Consulte o carrinho: subtotal **R$ 25,00**.
4. Finalize e confirme com **s**: pedido de **R$ 25,00**.
5. Consulte o catálogo: estoque do Livro passa a **3**; o carrinho fica vazio.

## Arquitetura

```text
MiniEcommerce.sln
MiniEcommerce.Console/
  Program.cs                 Menu, leitura de entradas e apresentação
  Dominio/
    Produto.cs               Identidade, nome, preço e estoque
    Catalogo.cs              Cadastro, listagem e busca por identificador
    CarrinhoDeCompras.cs      Adição, agrupamento, remoção e subtotal
    ItemCarrinho.cs          Produto e quantidade selecionada
    Pedido.cs                Checkout, validação e baixa de estoque
    ItemPedido.cs            Registro dos dados comprados e subtotal
MiniEcommerce.Tests/
  ProdutoTests.cs            Validações e catálogo
  CarrinhoTests.cs           Quantidades, estoque, subtotal e remoção
  PedidoTests.cs             Checkout, total e integridade do estoque
```

O projeto de testes referencia o projeto de console, conforme o enunciado. As regras de negócio ficam no namespace `MiniEcommerce.Dominio`, sem dependência da entrada e saída do terminal. Coleções são expostas somente para leitura; os itens dos pedidos preservam nome, preço e quantidade no momento da compra.

## Regras de negócio

- Nome obrigatório; espaços nas extremidades são removidos.
- Preço e estoque negativos são rejeitados; zero é permitido.
- Quantidades no carrinho devem ser inteiros positivos.
- Adições repetidas do mesmo produto são agrupadas, respeitando o estoque acumulado.
- Adicionar ou remover itens do carrinho não altera o estoque.
- Carrinho vazio não pode gerar pedido.
- O checkout revalida todos os estoques antes de alterar qualquer produto. Se outro carrinho consumir o estoque, a tentativa inválida mantém os demais estoques e o carrinho original.
- O pedido calcula os valores com `decimal`, baixa o estoque e limpa o carrinho após o sucesso.
- Entradas inválidas são informadas no terminal, permitindo continuar a operação.

## Testes e TDD

Os testes de cada funcionalidade foram escritos e registrados antes da implementação correspondente. No estágio inicial, a falha esperada foi de compilação por ausência dos tipos a implementar. Depois, os testes passaram com a implementação.

| Grupo | Casos | Cobertura |
| --- | ---: | --- |
| Produto e catálogo | 9 | Preço e estoque negativos, nome inválido, valores zero, cadastro e consulta |
| Carrinho | 8 | Adição e agrupamento, subtotal, excesso de estoque, quantidades inválidas, remoção e produto nulo |
| Pedido | 6 | Baixa de estoque, total, carrinho vazio, revalidação sem baixa parcial, resumo estável, repetição e carrinho nulo |

Também foi verificado o fluxo de console com cadastro, consulta, carrinho, cancelamento, remoção, checkout, entradas inválidas e encerramento por fim da entrada.

## Histórico de commits

```sh
git log --oneline --reverse
```

Os commits desta atividade usam `chore`, `test`, `feat`, `refactor` e `docs`, com escopos por funcionalidade. O commit inicial já existente no repositório foi preservado. Não houve necessidade de um commit `fix` para completar a atividade.

## Limites do exercício

Os dados ficam em memória e são perdidos ao encerrar. O programa atende um usuário por execução; não oferece persistência, pagamentos, frete ou operações concorrentes. Essas funcionalidades não fazem parte do enunciado.

## Entrega

Repositório público: [KdAndrade/MiniEcommerce](https://github.com/KdAndrade/MiniEcommerce).

Conforme a lista de exercícios, cada integrante deve enviar o link do repositório individualmente no ambiente de entrega da disciplina.
