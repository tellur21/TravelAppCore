using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Payments.Queries
{
    public class GetPaymentHistoryQuery : IRequest<ApiResponse<List<PaymentDto>>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
