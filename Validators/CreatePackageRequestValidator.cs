using FluentValidation;
using Hei_Hei_Api.Requests.Packages;

namespace Hei_Hei_Api.Validators;

public class CreatePackageRequestValidator : AbstractValidator<CreatePackageRequest>
{
    public CreatePackageRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must be at most 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.HeroIds)
            .NotEmpty().WithMessage("At least one hero is required.");
    }
}