using AutoMapper;
using Hei_Hei_Api.Data;
using Hei_Hei_Api.Requests.OrderAnimators;
using Hei_Hei_Api.Responses.OrderAnimators;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Hei_Hei_Api.Services.Application.Implementations;

public class OrderAnimatorService : IOrderAnimatorService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public OrderAnimatorService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<GetOrderAnimatorResponse>> GetAllByOrderIdAsync(int orderId)
    {
        var orderExists = await _context.Orders.AnyAsync(o => o.Id == orderId);

        if (!orderExists)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        var orderAnimators = await _context.OrderAnimators
            .Include(oa => oa.Animator).ThenInclude(a => a.User)
            .Include(oa => oa.Hero)
            .Where(oa => oa.OrderId == orderId)
            .ToListAsync();

        return _mapper.Map<List<GetOrderAnimatorResponse>>(orderAnimators);
    }

    public async Task<GetOrderAnimatorResponse> GetByIdAsync(int id)
    {
        var orderAnimator = await _context.OrderAnimators
            .Include(oa => oa.Animator).ThenInclude(a => a.User)
            .Include(oa => oa.Hero)
            .FirstOrDefaultAsync(oa => oa.Id == id);

        if (orderAnimator == null)
        {
            throw new KeyNotFoundException("OrderAnimator not found.");
        }

        return _mapper.Map<GetOrderAnimatorResponse>(orderAnimator);
    }

    public async Task<UpdateOrderAnimatorResponse> UpdateAsync(int id, UpdateOrderAnimatorRequest request)
    {
        var orderAnimator = await _context.OrderAnimators
            .FirstOrDefaultAsync(oa => oa.Id == id);

        if (orderAnimator == null)
        {
            throw new KeyNotFoundException("OrderAnimator not found.");
        }

        if (request.AssignedAmount != null)
        {
            orderAnimator.AssignedAmount = request.AssignedAmount.Value;
        }

        if (request.PaidToAnimator != null)
        {
            orderAnimator.PaidToAnimator = request.PaidToAnimator.Value;

            if (request.PaidToAnimator.Value && orderAnimator.PaidDate == null)
            {
                orderAnimator.PaidDate = DateTime.UtcNow;
            }
        }

        orderAnimator.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<UpdateOrderAnimatorResponse>(orderAnimator);
    }

    public async Task DeleteAsync(int id)
    {
        var orderAnimator = await _context.OrderAnimators
            .FirstOrDefaultAsync(oa => oa.Id == id);

        if (orderAnimator == null)
        {
            throw new KeyNotFoundException("OrderAnimator not found.");
        }

        _context.OrderAnimators.Remove(orderAnimator);
        await _context.SaveChangesAsync();
    }
}