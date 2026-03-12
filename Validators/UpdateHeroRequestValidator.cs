using FluentValidation;
using Hei_Hei_Api.Enums;
using Hei_Hei_Api.Requests.Heroes;

namespace Hei_Hei_Api.Validators;

public class UpdateHeroRequestValidator : AbstractValidator<UpdateHeroRequest>
{
    public UpdateHeroRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => x.Name != null);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .When(x => x.Price != null);

        RuleFor(x => x.Category)
            .Must(c => Enum.TryParse<HERO_CATEGORY>(c, true, out _))
            .WithMessage("Invalid Category.")
            .When(x => x.Category != null);

        RuleFor(x => x.Role)
            .Must(r => Enum.TryParse<HERO_ROLE>(r, true, out _))
            .WithMessage("Invalid Role.")
            .When(x => x.Role != null);
    }
}
