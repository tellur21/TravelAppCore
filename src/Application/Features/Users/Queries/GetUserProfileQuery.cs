using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries
{
    public class GetUserProfileQuery : IRequest<ApiResponse<UserProfileDto>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
