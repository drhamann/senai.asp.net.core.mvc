using Authentication.Domain.Repositories;
using Authentication.Infra;
using Authentication.Infrastructure.Respositorie;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Extension
{
    public static class InfrastructureExtension
    {
        public static void AddInfraExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddScoped<UserRepository>();
            services.AddScoped<IUserRepository, UserRepositoryInMemory>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.Configure<Settings>(configuration.GetSection("Settings"));

            var connectionStrings = configuration.GetValue<string>("ConnectionStringMaster");
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionStrings));
        }
    }
}


