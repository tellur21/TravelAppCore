using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries
{
    public class GetUsersQuery : IRequest<ApiResponse<PagedResult<UserProfileDto>>>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? Role { get; set; }
        public string? Status { get; set; }
    }
}
