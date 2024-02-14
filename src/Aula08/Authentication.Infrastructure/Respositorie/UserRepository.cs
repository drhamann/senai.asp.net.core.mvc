using Authentication.Domain.Entities;
using Authentication.Domain.Repositories;
using Authentication.Infrastructure.Respositorie;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infra
{
    public class UserRepository : IUserRepository
    {
        public AppDbContext AppDbContext { get; }

        public UserRepository(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;

            if (AppDbContext.Database.EnsureCreated())
            {

                if (AppDbContext.Users != null && AppDbContext.Users.Count<User>() == 0)
                {
                    AppDbContext.Users.Add(new User { Id = Guid.NewGuid(), UserName = "batman", Password = "batman123456", Role = "simples", Email = "batman@test.com" });
                    AppDbContext.Users.Add(new User { Id = Guid.NewGuid(), UserName = "robin", Password = "robin123456", Role = "simples", Email = "robin@test.com" });
                    AppDbContext.Users.Add(new User { Id = Guid.NewGuid(), UserName = "admin", Password = "admin123456", Role = "manager", Email = "admin@test.com" });

                    AppDbContext.SaveChanges();
                }
            }
        }

        public async Task<User> Get(string email, string password)
        {
            return AppDbContext.Users.FirstOrDefault(x => x.Email.Equals(email) && x.Password.Equals(password));
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return AppDbContext.Users.ToList();
        }

        public async Task<bool> Check(string email)
        {
            return AppDbContext.Users.Where(x => x.Email.ToLower() == email.ToLower()).ToList() == null;
        }

        public async Task<string> Create(User user)
        {
            try
            {
                await AppDbContext.Users.AddAsync(user);
                await AppDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return string.Empty;
        }

        public async Task<string> Update(User user)
        {
            try
            {
                AppDbContext.Users.Update(user);
                await AppDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "Usuario não encontrado";
        }

        public async Task<string> CheckIfIdExist(Guid id)
        {
            var user = AppDbContext.Users.FirstOrDefault(x => x.Id.Equals(id));
            if (user == null)
            {
                return "Usuario não encontrado";
            }
            return string.Empty;
        }

        public async Task<string> Delete(Guid id)
        {
            try
            {
                var user = AppDbContext.Users.FirstOrDefault(x => x.Id.Equals(id));
                if (user != null)
                {
                    AppDbContext.Users.Remove(user);
                    await AppDbContext.SaveChangesAsync();
                    return string.Empty;
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "Usuario não encontrado";
        }

        public async Task<User> GetById(Guid id)
        {
            return await AppDbContext.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));

        }
    }
}
