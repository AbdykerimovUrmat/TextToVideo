using Common.Extensions;
using FluentValidation;
using Models.Models;

namespace Models.Validators
{
    public class AddIn : AbstractValidator<UserModel.AddIn>
    {
        public AddIn()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().Must(x => x.IsEmail()).WithMessage("Provided email is not valid");
            RuleFor(x => x.Password).Equal(x => x.PasswordConfirmation).WithMessage("Password and confirmation should match").NotNull().NotEmpty().WithMessage("Password has to be not null");
        }
    }
}
