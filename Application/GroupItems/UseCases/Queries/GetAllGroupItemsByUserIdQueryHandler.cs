using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.GroupItems.UseCases.Queries
{
    public class GetAllGroupItemsByUserIdQueryHandler : IRequestHandler<GetAllGroupItemsByUserIdQuery, List<GroupItem>>
    {
        private readonly IGroupItemRepository _groupItemRepository;
        public GetAllGroupItemsByUserIdQueryHandler(IGroupItemRepository groupItemRepository)
        {
            _groupItemRepository = groupItemRepository;
        }

        public async Task<List<GroupItem>> Handle(GetAllGroupItemsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _groupItemRepository.GetAllGroupItemsByUserId(request.UserId);
        }
    }
}
