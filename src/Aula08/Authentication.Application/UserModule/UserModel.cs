using Authentication.Domain.Entities;
using AutoMapper;
using FluentValidation;

namespace Authentication.Application.UserModule
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(u => u).NotEmpty();// Regra para a 'propriedade'
            RuleFor(u => u.UserName).NotEmpty().MinimumLength(1).MaximumLength(12).WithName("Nome do usuario"); ;
            RuleFor(u => u.Role).NotEmpty();
            RuleFor(u => u.Password).NotEmpty();
            RuleFor(u => u.ConfirmPassword).NotEmpty().Equal(x => x.Password).WithErrorCode("1001001");
            RuleFor(u => u.Email).EmailAddress().WithMessage("Email não confere");            
        }
    }

    public class UserModelMapping : Profile
    {
        public UserModelMapping()
        {
            CreateMap<UserModel, User>().ReverseMap();
        }
    } 

}