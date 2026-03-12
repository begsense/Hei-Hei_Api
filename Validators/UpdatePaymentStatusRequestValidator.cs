using FluentValidation;
using Hei_Hei_Api.Enums;
using Hei_Hei_Api.Requests.Payments;

namespace Hei_Hei_Api.Validators;

public class UpdatePaymentStatusRequestValidator : AbstractValidator<UpdatePaymentStatusRequest>
{
    public UpdatePaymentStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(status => Enum.TryParse<PAYMENT_STATUS>(status, true, out _))
            .WithMessage($"Invalid status. Allowed values: {string.Join(", ", Enum.GetNames<PAYMENT_STATUS>())}");
    }
}
