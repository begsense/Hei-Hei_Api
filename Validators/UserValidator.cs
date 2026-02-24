using FluentValidation;
using Hei_Hei_Api.Requests.Users;

namespace Hei_Hei_Api.Validators;

public class UserValidator : AbstractValidator<CreateUserRequest>
{
    public UserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required.")
            .MinimumLength(3).WithMessage("UserName must be at least 3 characters long.")
            .MaximumLength(20).WithMessage("UserName must be at most 20 characters long.");
        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("Password is required.")
            .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("FullName is required.")
            .MinimumLength(3).WithMessage("Last name must be at least 3 characters long.")
            .MaximumLength(100).WithMessage("Last name must be less than 100 characters long.");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required.");
    }
}
