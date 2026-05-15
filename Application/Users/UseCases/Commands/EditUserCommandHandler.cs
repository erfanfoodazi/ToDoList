using Application.Interfaces;
using MediatR;

namespace Application.Users.UseCases.Commands
{
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, bool> 
    {
        private readonly IUserRepository _userRepository;

        public EditUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(request.Id);

            if (existingUser == null)
                return false;

            if (!string.IsNullOrWhiteSpace(request.FirstName))
                existingUser.FirstName = request.FirstName;

            if (!string.IsNullOrWhiteSpace(request.LastName))
                existingUser.LastName = request.LastName;

            if (!string.IsNullOrWhiteSpace(request.UserName))
                existingUser.UserName = request.UserName;

            if (!string.IsNullOrWhiteSpace(request.Email))
                existingUser.Email = request.Email;

            return await _userRepository.UpdateUserAsync(existingUser);
        }
    }
}