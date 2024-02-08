# Aula 11 

- Uso de memoria
- Midleware
- Partial

# Materiais 

- https://learn.microsoft.com/pt-br/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0


## Exemplo código

```
Exemplo de uso simulando banco em memoria

namespace ProjetoEmTresCamadas.Pizzaria.DAO.Dao.Memory
{
    public class ClienteDaoInMemory : IClienteDao
    {
        public List<ClienteVo> Clientes { get; set; }
        public ClienteDaoInMemory()
        {
            Clientes = new ();

            ClienteVo clienteVo = new ClienteVo()
            {
                Id = 1,
                Nome = "Exemplo de cliente para memoria",
                UserId = Guid.NewGuid()
            };
            Clientes.Add(clienteVo);
        }
        public Task AtualizarRegistro(ClienteVo objetoParaAtualizar)
        {
            var idAtualizar = Clientes.Find(cliente => cliente.Id.Equals(objetoParaAtualizar.Id));
            Clientes.Remove(idAtualizar);
            Clientes.Add(objetoParaAtualizar );
            return Task.CompletedTask;
        }

        public int CriarRegistroAsync(ClienteVo objetoVo)
        {
            objetoVo.Id = Clientes.Count + 1;
            Clientes.Add(objetoVo);
            return objetoVo.Id;

        }

        public Task DeletarRegistro(int ID)
        {
            var idAtualizar = Clientes.Find(cliente => cliente.Id.Equals(ID));
            Clientes.Remove(idAtualizar);
            return Task.CompletedTask;
        }

        public Task<ClienteVo> ObterRegistro(int ID)
        {
            var cliente = Clientes.Find(cliente => cliente.Id.Equals(ID));
            return Task.FromResult(cliente);
        }

        public List<ClienteVo> ObterRegistros(int ID)
        {
            var clientes = Clientes.FindAll(cliente => cliente.Id.ToString().Contains(ID.ToString()));
            return clientes;
        }

        public List<ClienteVo> ObterRegistrosAsync()
        {
            return Clientes;
        }
    }
}

```

```
//Classe program ou Startup 
app.UseMiddleware<ValidarTokenMiddleware>();

//Criar uma nova classepublic class ValidarTokenMiddleware
    {
        private readonly RequestDelegate _next; //Vai receber pela injeção de dependencia

        public ValidarTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //Aqui irá interceptar a chamada para validar o token
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                context.Request.Cookies.TryGetValue(LoginController.TOKEN_KEY, out var token);


                if (token == null)
                {
                    //Chamar api de autenticação para validar token
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync("Token invalido");
                    return;
                }
            }
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
```

```
//Criar um html na pasta shared

//Objeto para passar via parametro
@model ProjetoEmTresCamadas.Pizzaria.RegraDeNegocio.Entidades.Pizza

<div class="card">
    <img src="/images/margherita.jpg" class="card-img-top custom-image" alt="Imagem da Pizza">
    <div class="card-body">
        <h5 class="card-title">Sabor: @Model.Sabor </h5>
        <p class="card-text">Descrição: @Model.Descricao</p>
        <p class="card-text">Tamanho: @Model.TamanhoDePizza</p>
        <p class="card-text">Valor: R$@Model.Valor.ToString("0.00")</p>
        <input type="number" class="form-control" placeholder="Quantidade">
        <button class="btn btn-primary mt-2" onclick="adicionarAoCarrinho(@Model.Id)">Adicionar ao Carrinho</button>

    </div>
</div>


// Em outro html que deseja usar o conteudo parcial

//Onde está nome passar o nome do arquivo, e no model o objeto para uso de referencia
<partial name="PizzaCard" model="pizza" />

```

## Exercicio

- 01 Organizar projetos e pensar nas regras

 ## Próximos

- [próximo](aula11.md)