using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Payments.Commands
{
    public class CreatePaymentIntentCommandHandler : IRequestHandler<CreatePaymentIntentCommand, ApiResponse<PaymentIntentResult>>
    {
        private readonly IPaymentService _paymentService;

        public CreatePaymentIntentCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ApiResponse<PaymentIntentResult>> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
        {
            var result = await _paymentService.CreatePaymentIntentAsync(request.BookingId, request.Amount, request.Currency);
            if (!string.IsNullOrEmpty(result?.ClientSecret))
            {
                return ApiResponse<PaymentIntentResult>.SuccessResult(result);
            }
            return ApiResponse<PaymentIntentResult>.FailureResult("Failed to create payment intent.");
        }
    }
}