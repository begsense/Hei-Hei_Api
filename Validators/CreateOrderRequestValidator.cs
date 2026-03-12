using FluentValidation;
using Hei_Hei_Api.Requests.Orders;

namespace Hei_Hei_Api.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.PackageId)
            .GreaterThan(0).WithMessage("PackageId is required.");

        RuleFor(x => x.EventDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Event date must be in the future.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(200).WithMessage("Address must be at most 200 characters.");
    }
}
