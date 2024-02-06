using Authentication.Application.AuthenticationModule;
using Authentication.Application.UserModule;
using FluentValidation;

namespace Authentication.Application.Extensions
{
    public static class AuthenticationApplicationExtensions
    {
        public static void AddApplicationExtensions(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddValidatorsFromAssemblyContaining<UserModelValidator>(ServiceLifetime.Transient);
            services.AddAutoMapper(typeof(UserModel));
        }
    }
}
