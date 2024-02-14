using Authentication.Application.UserModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Route("api/Users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            this._userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserModel[]),200)]
        public async Task<IActionResult> GetAllAsync()
        {
            _logger.LogInformation("Retornando usuarios");
            var users = await _userService.GetAll();
            if (users != null)
            {
                _logger.LogDebug($"Usuarios", users);
                return Ok(users);
            }
            _logger.LogWarning("Falha ao retornar usuarios");
            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(UserModel userCreateRequest)
        {
            string error =  await _userService.Create(userCreateRequest);
            if (String.IsNullOrEmpty(error))
            {
                    return Ok();                
            }
            return BadRequest(error);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserModel userUpdateRequest)
        {
            string error = await _userService.Update(userUpdateRequest);
            if (String.IsNullOrEmpty(error))
            {
                //UserModel user = _userService.UserMap(userUpdateRequest);
                //error = await _userService.Update(user);
                if (String.IsNullOrEmpty(error))
                {
                    return Ok();
                }
            }

            return BadRequest(error);
        }       

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            string error = await _userService.Delete(id);
            if (String.IsNullOrEmpty(error))
            {
                return Ok();
            }
            return BadRequest(error);
        }
    }
}
