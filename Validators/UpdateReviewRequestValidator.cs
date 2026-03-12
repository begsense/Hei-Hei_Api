using FluentValidation;
using Hei_Hei_Api.Requests.Reviews;

namespace Hei_Hei_Api.Validators;

public class UpdateReviewRequestValidator : AbstractValidator<UpdateReviewRequest>
{
    public UpdateReviewRequestValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.")
            .When(x => x.Rating != null);

        RuleFor(x => x.Comment)
            .MaximumLength(1000).WithMessage("Comment must be at most 1000 characters.")
            .When(x => x.Comment != null);

        RuleFor(x => x)
            .Must(x => x.Rating != null || x.Comment != null)
            .WithMessage("At least one field must be provided.");
    }
}
