# Aula 09 

- Autenticação

# Materiais 

	-  https://github.com/drhamann/senai.pizzaria
	-  https://github.com/drhamann/senai.pizzaria/tree/main/ProjetoEmTresCamadas.Pizzaria.Mvc

## Nuget
```
 <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.1" />
  </ItemGroup>
 ```
 ## Alteração program

 ```
 // Adicionar serviços de criação do HttpClient 
builder.Services.AddHttpClient();

// Adicionar schema de autenticação
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Login/Index"; // Defenir página de login
    options.LogoutPath = "/Login/Logout"; // Defenir página logout
});

 ```

 ## Controlador de login
 ```
  public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public LoginController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Se já estiver autenticado redirecionar para controlar inicial
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Ponto");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page or another page after logout
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            // Make a request to your authentication API to validate user credentials
            var apiEndpoint = _configuration["AuthenticationApiEndpoint"];
            var requestBody = $"{{\"email\": \"{username}\", \"password\": \"{password}\"}}";
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            using (var response = await _httpClient.PostAsync(apiEndpoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Read the token from the API response
                    var tokenString = await response.Content.ReadAsStringAsync();

                    // Validate and decode the JWT token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadToken(tokenString) as JwtSecurityToken;

                    if (token != null)
                    {

                        // Extract claims from the JWT token
                        var jwtClaims = token.Claims.ToList();
                        var nameClaim = jwtClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                        if (nameClaim == null)
                        {
                            nameClaim = jwtClaims.FirstOrDefault(c => c.Type.Contains("name"));

                            jwtClaims.Add(new Claim(ClaimTypes.Name, nameClaim.Value));
                        }

                        var roleClaim = jwtClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                        if (roleClaim == null)
                        {
                            roleClaim = jwtClaims.FirstOrDefault(c => c.Type.Contains("role"));

                            jwtClaims.Add(new Claim(ClaimTypes.Role, roleClaim.Value));
                        }


                        // Create claims identity
                        var claimsIdentity = new ClaimsIdentity(jwtClaims, JwtBearerDefaults.AuthenticationScheme);

                        // Create claims principal
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        // Set the JWT token as a cookie
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true, // Set to true if using HTTPS
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTime.Now.AddHours(1) // Set the expiration time as needed
                        };

                        Response.Cookies.Append("JwtCookie", tokenString, cookieOptions);

                        var authProperties = new AuthenticationProperties
                        {
                            // You can set additional properties if needed
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                            IsPersistent = true,
                        };

                        // Sign in the user with the combined claims
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

                        // Redirect to another page or return success
                        return RedirectToAction("Index", "Pizzas");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid token received from the authentication API");
                        return View();
                    }
                }
                else
                {
                    // Authentication failed
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                    return View();
                }
            }


        }
    }
 ```

 ## Index login
 ```
 <div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header">
                    <h4 class="text-center">🚀 Login</h4>
                </div>
                <div class="card-body">
                    <form asp-controller="Login" asp-action="Index" method="post">
                        <div class="form-group">
                            <label for="username">Email:</label>
                            <input type="text" class="form-control" id="username" name="username" required>
                        </div>
                        <div class="form-group">
                            <label for="password">Senha:</label>
                            <input type="password" class="form-control" id="password" name="password" required>
                        </div>
                        <br />
                        <button type="submit" class="btn btn-primary btn-block">Login</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>
 ```

 ## Arquivo de configuração
 ```
 {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",

  "AuthenticationApiEndpoint": "https://localhost:56095/api/autenticador/login" // Altere pelo o endereço da sua api de autenticação

}
 ```

## Exercicio

- 01 Aplicar autenticação no seu projeto

 ## Próximos

- [próximo](aula9.md)