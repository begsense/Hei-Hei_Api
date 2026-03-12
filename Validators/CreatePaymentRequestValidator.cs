using FluentValidation;
using Hei_Hei_Api.Enums;
using Hei_Hei_Api.Requests.Orders;

namespace Hei_Hei_Api.Validators;

public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a 3-letter code (e.g. USD).");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required.")
            .Must(m => Enum.TryParse<PaymentMethod>(m, true, out _))
            .WithMessage("Invalid payment method.");

        RuleFor(x => x.TransactionId)
            .NotEmpty().WithMessage("TransactionId is required.");
    }
}
