using Application.DTOs;
using MediatR;
using System;

namespace Application.Features.Payments.Commands
{
    public class CreatePaymentIntentCommand : IRequest<ApiResponse<PaymentIntentResult>>
    {
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "usd";
    }
}