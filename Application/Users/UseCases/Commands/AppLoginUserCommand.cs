using Domain.Entities;
using MediatR;

namespace Application.Users.UseCases.Commands
{
    public class AppLoginUserCommand : IRequest<bool>
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
    }
}