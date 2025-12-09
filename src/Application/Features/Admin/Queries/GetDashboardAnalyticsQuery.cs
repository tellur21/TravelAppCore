using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Admin.Queries
{
    public class GetDashboardAnalyticsQuery : IRequest<ApiResponse<DashboardDto>>
    {
    }
}
