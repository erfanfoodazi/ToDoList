using Application.Interfaces;
using MediatR;
namespace Application.Users.UseCases.Commands
{
    public class AppLoginUserCommandHandler : IRequestHandler<AppLoginUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        public AppLoginUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(AppLoginUserCommand request, CancellationToken cancellationToken)
        {
            var isValid = await _userRepository.ValidateUserCredentialsAsync(
                request.UserNameOrEmail,
                request.Password
            );

            return isValid;
        }
    }
}