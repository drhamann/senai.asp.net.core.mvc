# Aula 01 

- Introdução
- Conteudo
	• Configuração do ambiente de desenvolvimento
	• Conhecendo o Padrão MVC
	• Entity Framework
	• Caching
	• Roteamento
	• Model binding
	• Validação de modelos
	• Injeção de dependência
	• Utilização do sistema de templates Razor
	• Filtros
	• Autenticação e autorização
	• Identity Framework
	• Testes
	• Publicando a aplicação	
- Sobre
- Tutorial

# Materiais 

	-  https://learn.microsoft.com/pt-br/aspnet/core/mvc/overview?view=aspnetcore-8.0
	-  https://learn.microsoft.com/pt-br/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-8.0&tabs=visual-studio

## Exemplo código

```
-  https://learn.microsoft.com/pt-br/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-8.0&tabs=visual-studio
Add-Migration InitialCreate
Update-Database

builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcMovieContext")));

@model MvcMovie.Models.Movie

if (context.Movie.Any())
{
    return;  // DB has been seeded.
}

 <a asp-action="Details" asp-route-id="@item.Id">Details</a>
 @Html.ActionLink("Details", "Details", new { id = item.Id })

```
## Exercicio

- 01 Finalizar tutorial https://learn.microsoft.com/pt-br/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-8.0&tabs=visual-studio

 ## Próximos

- [voltar](../README.md)
- [próximo](aula2.md)