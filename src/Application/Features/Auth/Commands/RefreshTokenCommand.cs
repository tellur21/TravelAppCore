using Application.DTOs;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public class RefreshTokenCommand : IRequest<AuthResult>
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}