# Aula 10

- Role
- Redirecionar páginas
- Layout
- partial view


# Materiais 


## Exemplo código

```
// Não usar layout 
@{
    Layout = null;
}

```

```
// Usar layout diferente
@{
    Layout = "_LayoutAdmin";
}

```

```
// Filtro com perfil no controller
[Authorize(Roles = "manager")]

```

```
// Configuração para redirecionar quando não tem permissão
builder.Services.AddAuthentication(options =>
    ...

}).AddCookie(options =>
{
    ...
    options.AccessDeniedPath = "/Login/Denied";
});

```

```
//Exemplo de verificar perfil no inicio da autenticação
[HttpGet]       
        public IActionResult Index()
        {

            // If user is already authenticated, redirect to another page
            if (User.Identity.IsAuthenticated)
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                if (role.Value == "simples")
                {
                    return RedirectToAction("Index", "Pizzas");
                }
                if (role.Value == "manager")
                {
                    return RedirectToAction("Index", "Admin");
                }
            }

            return View();
        }
```
## Exercicio

- 01 Criar no pro

 ## Próximos

- [próximo](aula2.md)