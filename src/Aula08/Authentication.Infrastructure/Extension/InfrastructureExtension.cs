using Authentication.Domain.Repositories;
using Authentication.Infra;
using Authentication.Infrastructure.Respositorie;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Authentication.Infrastructure.Extension
{
    public static class InfrastructureExtension
    {
        public static void AddInfraExtensions(this IServiceCollection services, IConfiguration configuration )
        {
            services.AddMemoryCache();
            services.AddScoped<UserRepository>();
            services.AddScoped<IUserRepository, UserRepositoryInMemory>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.Configure<Settings>(configuration.GetSection("Settings"));           
            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));

            var connectionStrings = configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionStrings.ConnectionStringMaster));
        }
    }
}


