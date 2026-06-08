
using Application.GroupItems.UseCases.Queries;
using Application.Interfaces;
using Application.RepetitionGroups.UseCases.Queries;
using Application.TodoItems.UseCases.Commands;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace UiWeb.Services
{
    public class RepetitionCheckerService
    {
        private readonly IMediator _mediator;
        private readonly IGroupItemRepository _groupRepository;
        private readonly IRepetitionGroupRepository _repetitionRepository;
        private readonly ILogger<RepetitionCheckerService> _logger;

        public RepetitionCheckerService(
            IMediator mediator,
            IGroupItemRepository groupRepository,
            IRepetitionGroupRepository repetitionRepository,
            ILogger<RepetitionCheckerService> logger)
        {
            _mediator = mediator;
            _groupRepository = groupRepository;
            _repetitionRepository = repetitionRepository;
            _logger = logger;
        }

        public async Task CheckAndGenerateRepetitions(int userId)
        {
            try
            {
                _logger.LogInformation("Checking repetitions for user {UserId}", userId);

                var groups = await _mediator.Send(new GetAllGroupItemsByUserIdQuery { UserId = userId});

                foreach (var group in groups)
                {
                    await CheckAndGenerateForGroup(group);
                    await CheckAndRemoveAdditionalTodos(group);
                }

                _logger.LogInformation("Repetition check completed for user {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking repetitions for user {UserId}", userId);
            }
        }

        private async Task CheckAndGenerateForGroup(GroupItem group)
        {
            if (group.RepetitionType == RepetitionType.None)
                return;

            var existingRepetitions = await _mediator.Send(new GetAllRepetitionByGroupQuery { GroupId = group.Id});
            var lastRepetition = existingRepetitions.OrderByDescending(r => r.RepetitionDate).FirstOrDefault();

            DateTime startDate;
            if (lastRepetition == null)
            {
                startDate = group.CreatedAt ?? DateTime.Now;
            }
            else
            {
                startDate = lastRepetition.RepetitionDate;
            }

            var nextDate = GetNextRepetitionDate(group, startDate);
            var today = DateTime.Today;
            var generatedCount = 0;

            while (nextDate <= today)
            {
                var alreadyExists = existingRepetitions.Any(r => r.RepetitionDate.Date == nextDate.Date);

                if (!alreadyExists)
                {
                    await CreateRepetition(group, nextDate);
                    generatedCount++;
                }

                nextDate = GetNextRepetitionDate(group, nextDate);
            }

            if (generatedCount > 0)
            {
                _logger.LogInformation("Generated {Count} repetitions for group {GroupId} ({GroupTitle})",
                    generatedCount, group.Id, group.Title);
            }
        }

        private DateTime GetNextRepetitionDate(GroupItem group, DateTime fromDate)
        {
            return group.RepetitionType switch
            {
                RepetitionType.Daily => fromDate.AddDays(1),
                RepetitionType.Weekly => fromDate.AddDays(7),
                RepetitionType.Monthly => fromDate.AddMonths(1),
                _ => fromDate.AddDays(1)
            };
        }

        private async Task CreateRepetition(GroupItem group, DateTime repetitionDate)
        {
            var sourceTodoItems = group.TodoItems ?? new List<TodoItem>();

            var todoItems = sourceTodoItems.Select(t => new TodoItem
            {
                Title = t.Title,
                Description = t.Description,
                Priority = t.Priority,
                IsComplete = false,
                Created = DateTime.Now,
                UserId = group.UserId,
                GroupItemId = null
            }).ToList();

            var repetition = new RepetedGroup
            {
                GroupId = group.Id,
                RepetitionDate = repetitionDate,
                IsCompleted = false,
                TodoItems = todoItems
            };

            await _repetitionRepository.CreateRepetationGroupAsync(repetition);
            
        }

        private async Task CheckAndRemoveAdditionalTodos(GroupItem group)
        {
            var repetitions = await _mediator.Send(new GetAllRepetitionByGroupQuery() { GroupId = group.Id });
            
            foreach (var repetition in repetitions)
            {
                var tasks = repetition.TodoItems;
                if (tasks == null || !tasks.Any())
                    continue;

                var duplicateGroups = tasks
                    .GroupBy(t => new { t.Title, t.GroupItemId, t.RepitedGroupId })
                    .Where(g => g.Count() > 1);

                foreach (var duplicateGroup in duplicateGroups)
                {
                    var tasksToDelete = duplicateGroup.Skip(1).ToList();
                    foreach (var task in tasksToDelete)
                    {
                        await _mediator.Send(new DeleteTodoByIdCommand { TodoId = task.Id });
                    }
                }
            }
        }
    }
}