# Aula 14 

- Sessão
- Cache


## Exemplo código

```
//Adicionar nos serviços a capacidade session

builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true; // make the session cookie essential
});

```

```
//Exemplo para coletar dados da sessão pelo HttpContext

 public static List<int> GetPizzas(HttpContext context)
        {
            List<int> pizzas = new List<int>();
            var data = context.Session.GetString("Pedidos");

            if (!string.IsNullOrEmpty(data))
            {
                pizzas.AddRange(JsonSerializer.Deserialize<int[]>(data));
            }
            return pizzas;
        }
///Exemplo para serializar dados na sessão

[HttpPost]
        public IActionResult AdicionarAoCarrinho(int pizzaId)
        {
            string returnUrl = Request.Headers["Referer"].ToString();
            List<int> pizzas = GetPizzas(HttpContext);
            pizzas.Add(pizzaId);
            
            HttpContext.Session.SetString("Pedidos", JsonSerializer.Serialize(pizzas.ToArray()));
            return Redirect(returnUrl);
        }

```

```
//Exemplo de uso no html para fazer o contador do carrinho
<li class="nav-item">
    <a class="nav-link text-dark" asp-area="" asp-controller="Carrinho" asp-action="Index">
    🛒 Carrinho <span id="totalItens">@CarrinhoController.GetPizzas(Context).Count</span>
    </a>
</li>
```
## Exercicio

- 01 Usar sessão para guardar dados, como pedidos, horarios.

 ## Próximos

- [próximo](aula15.md)