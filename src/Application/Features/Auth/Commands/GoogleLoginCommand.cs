using Application.DTOs;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public class GoogleLoginCommand : IRequest<AuthResult>
    {
        public required string IdToken { get; set; }
    }
}
