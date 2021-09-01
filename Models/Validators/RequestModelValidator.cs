
using FluentValidation;
using Models.Models;

namespace Models.Validators
{
    public static class RequestModelValidator
    {
        public class AddIn : AbstractValidator<RequestModel.AddIn>
        {
            public AddIn()
            {
                RuleFor(x => x.Text).MaximumLength(600).MinimumLength(1).WithMessage("Submission should be 1-600 characters length!");
                RuleFor(x => x.Author).MaximumLength(30).MinimumLength(1).WithMessage("Author name should be 1-30 characters length!");
            }
        }
    }
}
