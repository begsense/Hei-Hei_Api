using FluentValidation;
using Hei_Hei_Api.Enums;
using Hei_Hei_Api.Requests.Orders;

namespace Hei_Hei_Api.Validators;

public class UpdateOrderStatusRequestValidator : AbstractValidator<UpdateOrderStatusRequest>
{
    public UpdateOrderStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(s => Enum.TryParse<ORDER_STATUS>(s, true, out _))
            .WithMessage("Invalid status.");
    }
}
