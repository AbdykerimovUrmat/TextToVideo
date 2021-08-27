using Common.Extensions;
using FluentValidation;
using Models.Models;

namespace Models.Validators
{
    public class AddIn : AbstractValidator<UserModel.AddIn>
    {
        public AddIn()
        {
            RuleFor(x => x.Email).Must(x => x.IsEmail()).WithMessage("Provided email is not valid");
            RuleFor(x => x.PasswordConfirmation).Equal(x => x.Password).WithMessage("Password and confirmation should match");
        }
    }
}
