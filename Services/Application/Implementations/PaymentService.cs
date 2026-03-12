using AutoMapper;
using Hei_Hei_Api.Data;
using Hei_Hei_Api.Enums;
using Hei_Hei_Api.Requests.Payments;
using Hei_Hei_Api.Responses.Orders;
using Hei_Hei_Api.Responses.Payments;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Hei_Hei_Api.Services.Application.Implementations;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public PaymentService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PaymentResponse>> GetAllPaymentsAsync()
    {
        var payments = await _context.Payments
            .Include(p => p.Order)
            .ToListAsync();

        return _mapper.Map<List<PaymentResponse>>(payments);
    }

    public async Task<PaymentResponse> GetPaymentByIdAsync(int id)
    {
        var payment = await _context.Payments
            .Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (payment == null)
        {
            throw new KeyNotFoundException("Payment not found.");
        }

        return _mapper.Map<PaymentResponse>(payment);
    }

    public async Task<PaymentResponse> UpdatePaymentStatusAsync(int id, UpdatePaymentStatusRequest request)
    {
        var payment = await _context.Payments
            .Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (payment == null)
        {
            throw new KeyNotFoundException("Payment not found.");
        }

        var status = Enum.Parse<PAYMENT_STATUS>(request.Status, true);

        payment.Status = status;
        payment.UpdatedAt = DateTime.UtcNow;

        if (status == PAYMENT_STATUS.Completed)
        {
            payment.PaymentDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<PaymentResponse>(payment);
    }

    public async Task<DeletePaymentResponse> DeletePaymentAsync(int id)
    {
        var payment = await _context.Payments.FindAsync(id);

        if (payment == null)
        {
            throw new KeyNotFoundException("Payment not found.");
        }

        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();

        return new DeletePaymentResponse
        {
            Id = id,
            Message = "Payment deleted successfully."
        };
    }
}