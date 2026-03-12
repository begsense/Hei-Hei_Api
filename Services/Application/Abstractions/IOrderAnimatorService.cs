using Hei_Hei_Api.Requests.OrderAnimators;
using Hei_Hei_Api.Responses.OrderAnimators;

namespace Hei_Hei_Api.Services.Application.Abstractions;

public interface IOrderAnimatorService
{
    Task<List<GetOrderAnimatorResponse>> GetAllByOrderIdAsync(int orderId);
    Task<GetOrderAnimatorResponse> GetByIdAsync(int id);
    Task<UpdateOrderAnimatorResponse> UpdateAsync(int id, UpdateOrderAnimatorRequest request);
    Task DeleteAsync(int id);
}
