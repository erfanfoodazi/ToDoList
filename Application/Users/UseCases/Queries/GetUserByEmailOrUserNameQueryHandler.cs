using Application.Interfaces;
using Application.Users.UseCases.Dtos;
using MediatR;

namespace Application.Users.UseCases.Queries
{
    public class GetUserByEmailOrUserNameQueryHandler : IRequestHandler<GetUserByEmailOrUserNameQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        public GetUserByEmailOrUserNameQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetUserByEmailOrUserNameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.EmailOrUserName))
                throw new ArgumentException("Email or Username is required", nameof(request.EmailOrUserName));

            var user = await _userRepository.GetUserByEmailAsync(request.EmailOrUserName)
                               ?? await _userRepository.GetUserByUserNameAsync(request.EmailOrUserName);

            if (user == null)
                throw new ArgumentNullException(nameof(user), $"User with '{request.EmailOrUserName}' was not found");

            var userDto = new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt,
            };
            return userDto;
        }
    }
}
