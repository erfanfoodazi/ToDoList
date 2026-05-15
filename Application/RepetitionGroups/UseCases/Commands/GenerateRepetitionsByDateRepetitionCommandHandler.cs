using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.RepetedGroups.UseCases.Commands
{
    public class GenerateRepetitionsByDateRepetitionCommandHandler : IRequestHandler<GenerateRepetitionsByDateRepetitionCommand, int>
    {
        private readonly IRepetitionGroupRepository _repetitionRepository;
        private readonly IGroupItemRepository _groupRepository;
        
        public GenerateRepetitionsByDateRepetitionCommandHandler(
            IRepetitionGroupRepository repetitionRepository,
            IGroupItemRepository groupRepository)
        {
            _repetitionRepository = repetitionRepository;
            _groupRepository = groupRepository;
        }
        
        public async Task<int> Handle(GenerateRepetitionsByDateRepetitionCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetGroupItemByIdAsync(request.GroupId);
            if (group?.TodoItems == null) return 0;
            
            if (group.RepetitionType == RepetitionType.None)
                return 0;  
            
            var startDate = request.StartDate;
            var endDate = request.EndDate ?? group.EndRepit ?? startDate.AddMonths(3);
            
            var existingRepetitions = await _repetitionRepository
                .GetAllRepetationsByGroupIdAsync(request.GroupId);
            
            var existingDates = existingRepetitions.Select(r => r.RepetitionDate.Date).ToHashSet();
            
            var newRepetitions = new List<RepetedGroup>();
            var currentDate = startDate;
            
            while (currentDate <= endDate)
            {
                if (ShouldRepetitionOccur(group, currentDate) && !existingDates.Contains(currentDate.Date))
                {
                    var repetition = new RepetedGroup
                    {
                        GroupId = request.GroupId,
                        RepetitionDate = currentDate,
                        IsCompleted = false,
                        TodoItems = group.TodoItems.Select(t => new TodoItem
                        {
                            Title = t.Title,
                            Description = t.Description,
                            Priority = t.Priority,
                            IsComplete = false,
                            Created = DateTime.Now,
                            GroupItemId = null 
                        }).ToList()
                    };
                    
                    newRepetitions.Add(repetition);
                }
                
                currentDate = GetNextRepetitionDate(group, currentDate);
            }
            
            int createdCount = 0;
            foreach (var repetition in newRepetitions)
            {
                var created = await _repetitionRepository.CreateRepetationGroupAsync(repetition);
                if (created != null)
                    createdCount++;
            }
            
            return createdCount;
        }
        
        private bool ShouldRepetitionOccur(GroupItem group, DateTime date)
        {
            switch (group.RepetitionType)
            {
                case RepetitionType.Daily:
                    return true; 
                    
                case RepetitionType.Weekly:
                    if (!group.CreatedAt.HasValue) return false;
                    return date.DayOfWeek == group.CreatedAt.Value.DayOfWeek;
                    
                case RepetitionType.Monthly:
                    if (!group.CreatedAt.HasValue) return false;
                    return date.Day == group.CreatedAt.Value.Day;
                    
               
                    
                default:
                    return false;
            }
        }
        
        private DateTime GetNextRepetitionDate(GroupItem group, DateTime currentDate)
        {
            return group.RepetitionType switch
            {
                RepetitionType.Daily => currentDate.AddDays(1),
                RepetitionType.Weekly => currentDate.AddDays(7),
                RepetitionType.Monthly => currentDate.AddMonths(1),
                _ => currentDate.AddDays(1)
            };
        }
    }
}