using FluentValidation;
using Hei_Hei_Api.Requests.Reviews;

namespace Hei_Hei_Api.Validators;

public class CreateReviewRequestValidator : AbstractValidator<CreateReviewRequest>
{
    public CreateReviewRequestValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0).WithMessage("OrderId is required.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required.")
            .MaximumLength(1000).WithMessage("Comment must be at most 1000 characters.");
    }
}
