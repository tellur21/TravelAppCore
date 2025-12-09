using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, AuthResult>
    {
        private readonly IIdentityService _identityService;

        public GoogleLoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthResult> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.LoginWithGoogleAsync(request.IdToken);
        }
    }
}
