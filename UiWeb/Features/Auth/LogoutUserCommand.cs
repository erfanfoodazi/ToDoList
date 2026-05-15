// UiWeb/Features/Auth/LogoutUserCommand.cs
using MediatR;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;

namespace UiWeb.Features.Auth
{
    public class LogoutUserCommand : IRequest<bool>
    {
    }

    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, bool>
    {
        private readonly SignInManager<User> _signInManager;


        public LogoutUserCommandHandler(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<bool> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}