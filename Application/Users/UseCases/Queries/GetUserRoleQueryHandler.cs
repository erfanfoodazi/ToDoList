using Application.Interfaces;
using MediatR;

namespace Application.Users.UseCases.Queries
{
    public class GetUserRoleQueryHandler : IRequestHandler<GetUserRoleQuery, IList<string>>
    {
        private readonly IUserRepository _userRepository;
        public GetUserRoleQueryHandler(IUserRepository userRepository)

        {
            _userRepository = userRepository;
        }

        public async Task<IList<string>> Handle(GetUserRoleQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id);
            
            if(user == null) 
                throw new ArgumentNullException(nameof(user));

            var roles = await _userRepository.GetUserRolesAsync(user);
            if (roles == null || !roles.Any())
                return new List<string>();

            return roles;
        }
    }
}
