
using Authentication.Domain.Entities;
using Authentication.Domain.Repositories;
using AutoMapper;
using FluentValidation;

namespace Authentication.Application.UserModule
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetAll();
        Task<string> Create(UserModel user);      
        Task<string> Delete(Guid id);
        Task<string> Update(UserModel user);
    }
    public class UserService : IUserService
    {
        private IUserRepository _userRepository { get; }
        private ILogger<UserService> _logger { get; }
        private IValidator<UserModel> _userValidator { get; }
        public IMapper _mapper { get; }

        public UserService(
            IUserRepository userRepository,
            ILogger<UserService> logger,
            IValidator<UserModel> userValidator,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _userValidator = userValidator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
           var users = await _userRepository.GetAll();
                
            var usersModel = _mapper.Map<List<UserModel>>(users);
           
            return usersModel;                
        }

        public async Task<string> Create(UserModel user)
        {
            var validationUserResult = await _userValidator.ValidateAsync(user);

            string error = string.Empty;
            if(validationUserResult.IsValid == false)
            {
                foreach (var resultError in validationUserResult.Errors)
                {
                    error += resultError + "\r\n";
                }
            }
            if(await _userRepository.Check(user.Email))
            {
                error += "\r\n Dados invalidos";
            }
            if (String.IsNullOrEmpty(error))
            {
                var userVo = _mapper.Map<User>(user);
                error = await _userRepository.Create(userVo);
                if (String.IsNullOrEmpty(error))
                {
                    return string.Empty;
                }
            }
            return error;
        }

        public async Task<string> Delete(Guid id)
        {
            var user = await _userRepository.GetById(id);
            string error = string.Empty;
            if (user == null)
            {
                error = ("Não encontrado");
            }

            await _userRepository.Delete(id);

            return error;
        }

        public async Task<string> Update(UserModel usermodel)
        {
            var user = _mapper.Map<User>(usermodel);
            string error = string.Empty;
            if (user == null)
            {
                error = ("Não encontrado");
                return error;
            }
            return await _userRepository.Update(user);

        }           
    }
}
