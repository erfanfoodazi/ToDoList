using Application.Interfaces;
using MediatR;

namespace Application.Users.UseCases.Commands
{
    public class EditUserPasswordCommandHandler : IRequestHandler<EditUserPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public EditUserPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(EditUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(request.Id);
            if (existingUser == null)
                return false;

            if (string.IsNullOrWhiteSpace(request.CurrentPassword))
                return false;

            if (string.IsNullOrWhiteSpace(request.NewPassword))
                return false;

            if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
                return false;

            if (request.NewPassword != request.ConfirmPassword)
                return false;

            if (request.CurrentPassword == request.NewPassword)
                return false;

            if (!IsPasswordStrong(request.NewPassword))
                return false;
            return await _userRepository.ChangePasswordAsync(existingUser, request.CurrentPassword, request.NewPassword);
        }

        private bool IsPasswordStrong(string password)
        {
            if (password.Length < 6)
                return false;

            bool hasUpper = false, hasLower = false, hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
            }

            return hasUpper && hasLower && hasDigit;
        }
    }
}