using Authentication.Application.AuthenticationModule;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("api/autenticador")]
    [Authorize]
    public class AutenticadorController : ControllerBase
    {
        private readonly Application.AuthenticationModule.IAuthenticationService _authenticationService;

        public AutenticadorController(Application.AuthenticationModule.IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Authenticate(AuthenticateModel model)
        {
            // Recupera o usuário
            var token = await _authenticationService.AuthenticateAsync(model);

            // Verifica se o usuário existe
            if (token == string.Empty)
                return BadRequest(new { message = "Usuário ou senha inválidos" });

            return Ok(token);
        }
    }
}