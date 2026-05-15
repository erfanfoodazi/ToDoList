using Application.Interfaces;
using MediatR;

namespace Application.Users.UseCases.Commands
{
    public class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        public AddUserToRoleCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
        {
            var existUser = await _userRepository.GetUserByIdAsync(request.Id);

            if (existUser == null) 
                return false;

            if (string.IsNullOrEmpty(request.Role))
                throw new Exception("Role is required");

           
            return await _userRepository.AddUserToRoleAsync(existUser, request.Role);
        }
    }
}
