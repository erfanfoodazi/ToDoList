using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.GroupItems.UseCases.Queries
{
    public class GetAllGroupItemQueryHandler : IRequestHandler<GetAllGroupItemsQuery, List<GroupItem>>
    {
        private readonly IGroupItemRepository _groupItemRepository;
        public GetAllGroupItemQueryHandler(IGroupItemRepository groupItemRepository)
        {
            _groupItemRepository = groupItemRepository;
        }

        public async Task<List<GroupItem>> Handle(GetAllGroupItemsQuery request, CancellationToken cancellationToken)
        {
            return await _groupItemRepository.GetAllGroupItemsAsync();
        }
    }
}
