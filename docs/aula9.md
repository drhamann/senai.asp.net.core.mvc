# Aula 09 

- Consumir API

# Materiais 

- https://github.com/reactiveui/refit
- https://balta.io/blog/aspnet-refit2
- Postmnan
	
## Exemplo código

```
//Adicionar injeção de IHttpClientFactory
// Adicionar serviços de criação do HttpClient 
builder.Services.AddHttpClient();

//Uso em construtor 
 public PizzasController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            Configuration = configuration;
            PizzaApiEndpoint = Configuration["PizzaApiEndpoint"] + "/api/pizza";
        }

```

```
// Post
 var requestBody = $"{{ \"id\": 0,\"Sabor\": \"{pizza.Sabor}\"," +
              $" \"TamanhoDePizza\": {(int)pizza.TamanhoDePizza}," +
              $" \"Descricao\": \"{pizza.Descricao}\"," +
              $" \"Valor\": {pizza.Valor}" +
              $"}}";
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            using (var response = await _httpClient.PostAsync(PizzaApiEndpoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");

             using (var response = await _httpClient.PostAsJsonAsync<Pizza>(PizzaApiEndpoint, pizza))
            {
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

```

```
// Get
 //var pizzas = await _httpClient.GetFromJsonAsync<Pizza[]>(PizzaApiEndpoint);

 using (var response = await _httpClient.GetAsync(PizzaApiEndpoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var jsonPizzas = await response.Content.ReadAsStringAsync();
                    Pizza[] pizzas = JsonSerializer.Deserialize<Pizza[]>(jsonPizzas);
                    pizzaViewModel.Pizzas.AddRange(pizzas);
                }
            }

```

```
Postman 
var client = new HttpClient();
var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5122/api/Pizza");
request.Headers.Add("accept", "text/plain");
var content = new StringContent("\r\n{ \"id\": 0,\"Sabor\": \"Calabresa\", \"TamanhoDePizza\": Media, \"Descricao\": \"Calabresa sem cebola\", \"Valor\": 25}", null, "application/json");
request.Content = content;
var response = await client.SendAsync(request);
response.EnsureSuccessStatusCode();
Console.WriteLine(await response.Content.ReadAsStringAsync());

```

## Exercicio

- 01 Aplicar tecnica no projeto

 ## Próximos

- [próximo](aula10.md)