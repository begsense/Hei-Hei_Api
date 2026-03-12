using Hei_Hei_Api.Responses.Orders;
using Hei_Hei_Api.Responses.Payments;
using Hei_Hei_Api.Requests.Payments;

namespace Hei_Hei_Api.Services.Application.Abstractions;

public interface IPaymentService
{
    Task<List<PaymentResponse>> GetAllPaymentsAsync();
    Task<PaymentResponse> GetPaymentByIdAsync(int id);
    Task<PaymentResponse> UpdatePaymentStatusAsync(int id, UpdatePaymentStatusRequest request);
    Task<DeletePaymentResponse> DeletePaymentAsync(int id);
}
