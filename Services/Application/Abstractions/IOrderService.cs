using Hei_Hei_Api.Requests.Orders;
using Hei_Hei_Api.Responses.Orders;
using System.Security.Claims;

namespace Hei_Hei_Api.Services.Application.Abstractions;

public interface IOrderService
{
    Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, ClaimsPrincipal userClaims);
    Task<GetOrderResponse> GetOrderByIdAsync(int id, ClaimsPrincipal userClaims);
    Task<List<GetOrderResponse>> GetAllOrdersAsync();
    Task<List<GetOrderResponse>> GetMyOrdersAsync(ClaimsPrincipal userClaims);
    Task<GetOrderResponse> UpdateOrderStatusAsync(int id, UpdateOrderStatusRequest request);
    Task<GetOrderResponse> AssignAnimatorAsync(int id, AssignAnimatorRequest request);
    Task<GetOrderResponse> CreatePaymentAsync(int id, CreatePaymentRequest request);
    Task<DeleteOrderResponse> DeleteOrderAsync(int id);
}
