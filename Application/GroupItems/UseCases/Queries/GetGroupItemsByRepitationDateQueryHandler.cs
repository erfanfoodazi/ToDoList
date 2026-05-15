using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.GroupItems.UseCases.Queries
{
    public class GetGroupItemsByRepitationDateQueryHandler : IRequestHandler<GetGroupItemsByRepitationDateQuery, List<GroupItem>>
    {
        private readonly IGroupItemRepository _groupItemRepository;

        public GetGroupItemsByRepitationDateQueryHandler(IGroupItemRepository groupItemRepository)
        {
            _groupItemRepository = groupItemRepository;
        }

        public async Task<List<GroupItem>> Handle(GetGroupItemsByRepitationDateQuery request, CancellationToken cancellationToken)
        {
            var allGroupItems = await _groupItemRepository.GetAllGroupItemsAsync();
            var targetDate = request.TargetDate ?? DateTime.Today;

            var todayGroupItems = new List<GroupItem>();

            foreach (var group in allGroupItems)
            {
                if (ShouldRepetitionToday(group, targetDate))
                {
                    todayGroupItems.Add(group);
                }
            }

            return todayGroupItems;
        }

        private bool ShouldRepetitionToday(GroupItem group, DateTime targetDate)
        {
            if (group.EndRepit.HasValue && group.EndRepit.Value.Date < targetDate)
                return false;

            switch (group.RepetitionType)
            {
                case RepetitionType.Daily:
                    return true;  

                case RepetitionType.Weekly:
                    if (!group.CreatedAt.HasValue)
                        return false;
                    return targetDate.DayOfWeek == group.CreatedAt.Value.DayOfWeek;

                case RepetitionType.Monthly:
                    if (!group.CreatedAt.HasValue)
                        return false;
                    return targetDate.Day == group.CreatedAt.Value.Day;

                   
                case RepetitionType.None:
                default:
                    return false;
            }
        }
    }
}