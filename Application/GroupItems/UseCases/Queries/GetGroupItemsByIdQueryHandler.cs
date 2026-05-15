using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.GroupItems.UseCases.Queries
{
    public class GetGroupItemsByIdQueryHandler : IRequestHandler<GetGroupItemsByIdQuery, GroupItem?>
    {
        private readonly IGroupItemRepository _groupItemRepository;
        public GetGroupItemsByIdQueryHandler(IGroupItemRepository groupItemRepository)
        {
            _groupItemRepository = groupItemRepository;
        }
        public async Task<GroupItem?> Handle(GetGroupItemsByIdQuery request, CancellationToken cancellationToken)
        {
            return await _groupItemRepository.GetGroupItemByIdAsync(request.Id);
        }
    }
}
