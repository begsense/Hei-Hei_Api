using AutoMapper;
using Hei_Hei_Api.Data;
using Hei_Hei_Api.Enums;
using Hei_Hei_Api.Helpers;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Orders;
using Hei_Hei_Api.Responses.Orders;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hei_Hei_Api.Services.Application.Implementations;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public OrderService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, ClaimsPrincipal userClaims)
    {
        var userId = GetUserHelper.GetUserId(userClaims);

        var package = await _context.Packages.FindAsync(request.PackageId);

        if (package == null)
        {
            throw new KeyNotFoundException("Package not found.");
        }

        var order = new Order
        {
            UserId = userId,
            PackageId = request.PackageId,
            EventDate = request.EventDate,
            Address = request.Address,
            Status = ORDER_STATUS.Pending
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return _mapper.Map<CreateOrderResponse>(order);
    }

    public async Task<GetOrderResponse> GetOrderByIdAsync(int id, ClaimsPrincipal userClaims)
    {
        var order = await GetOrderWithIncludes()
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        if (!userClaims.IsInRole("Admin"))
        {
            var userId = GetUserHelper.GetUserId(userClaims);

            if (order.UserId != userId)
            {
                throw new UnauthorizedAccessException();
            }
        }

        return _mapper.Map<GetOrderResponse>(order);
    }

    public async Task<List<GetOrderResponse>> GetAllOrdersAsync()
    {
        var orders = await GetOrderWithIncludes().ToListAsync();

        return _mapper.Map<List<GetOrderResponse>>(orders);
    }

    public async Task<List<GetOrderResponse>> GetMyOrdersAsync(ClaimsPrincipal userClaims)
    {
        var userId = GetUserHelper.GetUserId(userClaims);

        var orders = await GetOrderWithIncludes()
            .Where(o => o.UserId == userId)
            .ToListAsync();

        return _mapper.Map<List<GetOrderResponse>>(orders);
    }

    public async Task<GetOrderResponse> UpdateOrderStatusAsync(int id, UpdateOrderStatusRequest request)
    {
        var order = await GetOrderWithIncludes()
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        var status = Enum.Parse<ORDER_STATUS>(request.Status, true);

        order.Status = status;
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<GetOrderResponse>(order);
    }

    public async Task<GetOrderResponse> AssignAnimatorAsync(int id, AssignAnimatorRequest request)
    {
        var order = await GetOrderWithIncludes()
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        var animator = await _context.Animators.FindAsync(request.AnimatorId);
        if (animator == null)
        {
            throw new KeyNotFoundException("Animator not found.");
        }

        var hero = await _context.Heroes.FindAsync(request.HeroId);
        if (hero == null)
        {
            throw new KeyNotFoundException("Hero not found.");
        }

        var alreadyAssigned = order.OrderAnimators
            .Any(oa => oa.AnimatorId == request.AnimatorId);

        if (alreadyAssigned)
        {
            throw new InvalidOperationException("This animator is already assigned to this order.");
        }

        var orderAnimator = new OrderAnimator
        {
            OrderId = id,
            AnimatorId = request.AnimatorId,
            HeroId = request.HeroId,
            AssignedAmount = request.AssignedAmount,
            PaidToAnimator = false
        };

        _context.OrderAnimators.Add(orderAnimator);
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<GetOrderResponse>(
            await GetOrderWithIncludes().FirstOrDefaultAsync(o => o.Id == id)
        );
    }

    public async Task<GetOrderResponse> CreatePaymentAsync(int id, CreatePaymentRequest request)
    {
        var order = await GetOrderWithIncludes()
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        if (order.Payment != null)
        {
            throw new InvalidOperationException("Payment already exists for this order.");
        }

        var paymentMethod = Enum.Parse<PAYMENT_METHOD>(request.PaymentMethod, true);

        var payment = new Payment
        {
            OrderId = id,
            Amount = request.Amount,
            Currency = request.Currency,
            PaymentMethod = paymentMethod,
            TransactionId = request.TransactionId,
            Status = PAYMENT_STATUS.Pending,
            PaymentDate = DateTime.UtcNow
        };

        _context.Payments.Add(payment);
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<GetOrderResponse>(
            await GetOrderWithIncludes().FirstOrDefaultAsync(o => o.Id == id)
        );
    }

    public async Task<DeleteOrderResponse> DeleteOrderAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return new DeleteOrderResponse
        {
            Id = id,
            Message = "Order deleted successfully."
        };
    }

    private IQueryable<Order> GetOrderWithIncludes()
    {
        return _context.Orders
            .Include(o => o.User)
            .Include(o => o.Package)
            .Include(o => o.Payment)
            .Include(o => o.OrderAnimators)
                .ThenInclude(oa => oa.Animator)
                    .ThenInclude(a => a.User)
            .Include(o => o.OrderAnimators)
                .ThenInclude(oa => oa.Hero);
    }
}