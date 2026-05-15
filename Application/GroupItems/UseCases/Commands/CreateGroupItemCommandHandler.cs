using Application.Interfaces;
using Application.Users.UseCases.Queries;
using Domain.Entities;
using MediatR;

namespace Application.GroupItems.UseCases.Commands
{
    public class CreateGroupItemCommandHandler : IRequestHandler<CreateGroupItemCommand, GroupItem>
    {
        private readonly IGroupItemRepository _groupItemRepository;
        private readonly IUserRepository _userRepository;
        public CreateGroupItemCommandHandler(IGroupItemRepository groupItemRepository, IUserRepository userRepository)
        {
            _groupItemRepository = groupItemRepository;
            _userRepository = userRepository;
        }
        public async Task<GroupItem> Handle(CreateGroupItemCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null) 
                throw new ArgumentNullException(nameof(user), $"User with Id {request.UserId} not found");
            var group = new GroupItem(
                request.Title,
                request.RepetitionType,
                request.EndDate,
                request.UserId
                );
            return await _groupItemRepository.AddGroupItemAsync(group);
        }
    }
}
