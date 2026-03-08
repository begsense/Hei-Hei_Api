using FluentValidation;
using Hei_Hei_Api.Requests.Animators;

namespace Hei_Hei_Api.Validators;

public class AnimatorValidator : AbstractValidator<AddAnimatorInfoRequest>
{
    public AnimatorValidator()
    {
        RuleFor(x => x.Bio)
            .NotEmpty().WithMessage("Bio is required.")
            .MaximumLength(500).WithMessage("Bio must be at most 500 characters.");
    }
}
