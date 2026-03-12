using FluentValidation;
using Hei_Hei_Api.Requests.OrderAnimators;

namespace Hei_Hei_Api.Validators;

public class UpdateOrderAnimatorRequestValidator : AbstractValidator<UpdateOrderAnimatorRequest>
{
    public UpdateOrderAnimatorRequestValidator()
    {
        RuleFor(x => x.AssignedAmount)
            .GreaterThan(0).WithMessage("AssignedAmount must be greater than 0.")
            .When(x => x.AssignedAmount != null);

        RuleFor(x => x)
            .Must(x => x.AssignedAmount != null || x.PaidToAnimator != null)
            .WithMessage("At least one field must be provided.");
    }
}
