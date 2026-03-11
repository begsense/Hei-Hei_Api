using FluentValidation;
using Hei_Hei_Api.Enums;
using Hei_Hei_Api.Requests.Heroes;

namespace Hei_Hei_Api.Validators;

public class CreateHeroRequestValidator : AbstractValidator<CreateHeroRequest>
{
    public CreateHeroRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Category)
            .Must(BeValidCategory)
            .WithMessage("Invalid Category.");

        RuleFor(x => x.Role)
            .Must(BeValidRole)
            .WithMessage("Invalid Role.");
    }

    private bool BeValidCategory(string category)
    {
        return Enum.TryParse<HERO_CATEGORY>(category, true, out _);
    }

    private bool BeValidRole(string role)
    {
        return Enum.TryParse<HERO_ROLE>(role, true, out _);
    }
}
