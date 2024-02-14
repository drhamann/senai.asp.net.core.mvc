using Authentication.Domain.Entities;
using Authentication.Domain.Repositories;
using Authentication.Infra;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Authentication.Infrastructure.Respositorie
{
    public class UserRepositoryInMemory : IUserRepository
    {
        private readonly UserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;

        public UserRepositoryInMemory(UserRepository userRepository, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        public Task<bool> Check(string email)
        {
            return _userRepository.Check(email);
        }

        public Task<string> CheckIfIdExist(Guid id)
        {
           return _userRepository.CheckIfIdExist(id);
        }

        public Task<string> Create(User user)
        {
            return _userRepository.Create(user);
        }

        public Task<string> Delete(Guid id)
        {
            return _userRepository.Delete(id);
        }

        public async Task<User> Get(string email, string password)
        {
            User user;
            var key = $"{email}_{password}";
            _memoryCache.TryGetValue(key, out user);
            if(user == null)
            {
                user = await _userRepository.Get(email, password);
                if(user == null)
                {
                    return null;
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .AddExpirationToken(new CancellationChangeToken
                    (new CancellationTokenSource(TimeSpan.FromMinutes(60)).Token));
                _memoryCache.Set(key, user, cacheEntryOptions);
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            IEnumerable<User> users;
            var key = $"users";
            _memoryCache.TryGetValue(key, out users);
            if (users == null)
            {
                users = await _userRepository.GetAll();
                if (users == null)
                {
                    return null;
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .AddExpirationToken(new CancellationChangeToken
                    (new CancellationTokenSource(TimeSpan.FromMinutes(60)).Token));
                _memoryCache.Set(key, users, cacheEntryOptions);
            }
            return users;
        }

        public async Task<User> GetById(Guid id)
        {
           return await _userRepository.GetById(id);
        }

        public Task<string> Update(User user)
        {
           return _userRepository.Update(user);
        }
    }
}
