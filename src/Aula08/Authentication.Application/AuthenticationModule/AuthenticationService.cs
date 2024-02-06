using Authentication.Application.Services;
using Authentication.Domain.Repositories;
using Authentication.Infra;
using Microsoft.Extensions.Options;

namespace Authentication.Application.AuthenticationModule
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateAsync(AuthenticateModel model);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        public string _secret { get; private set; }

        public AuthenticationService(
            IUserRepository userRepository,
            IOptions<Settings> optionSettings)
        {
            _userRepository = userRepository;
            _secret = optionSettings.Value.Secret;
        }

        public async Task<string> AuthenticateAsync(AuthenticateModel model)
        {
           var user = await _userRepository.Get(model.Email, model.Password);

            if (user == null)
                return String.Empty;

            var token = TokenService.GenerateToken(user, _secret);

            return token;
        }
    }
}
