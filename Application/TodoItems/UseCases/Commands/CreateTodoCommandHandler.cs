using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TodoItems.UseCases.Commands;

public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, int>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IGroupItemRepository _groupItemRepository;
    private readonly IRepetitionGroupRepository _repetitionGroupRepository;
    public CreateTodoCommandHandler(ITodoItemRepository todoItemRepository,
            IGroupItemRepository groupItemRepository,
            IRepetitionGroupRepository repetitionGroupRepository)
    {
        _todoItemRepository = todoItemRepository;
        _groupItemRepository = groupItemRepository;
        _repetitionGroupRepository = repetitionGroupRepository;
    }
    
    public async Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupItemRepository.GetGroupItemByIdAsync(request.GroupId);
        if (group == null)
            throw new ArgumentException(nameof(group));
        var todo = new TodoItem(
            request.TodoItem.Title,
            request.TodoItem.Description ?? "",
            request.TodoItem.Priority,
            group.UserId,
            request.GroupId
        )
        {
            Created = group.CreatedAt,

        };

        
        var item = await _todoItemRepository.AddTodoItemAsync(todo);
        return item.Id;
    }
}