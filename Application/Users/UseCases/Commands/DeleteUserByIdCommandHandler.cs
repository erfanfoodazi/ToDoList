using Application.Interfaces;
using MediatR;

namespace Application.Users.UseCases.Commands
{
    public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        public DeleteUserByIdCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
        {
            var existUser = await _userRepository.GetUserByIdAsync(request.Id);

            if (existUser == null)
                return false;

            return await _userRepository.DeleteUserAsync(request.Id);
        }
    }
}
