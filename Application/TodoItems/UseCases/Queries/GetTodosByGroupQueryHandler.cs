using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TodoItems.UseCases.Queries
{
    public class GetTodosByGroupQueryHandler : IRequestHandler<GetTodosByGroupQuery, List<TodoItem>>
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public GetTodosByGroupQueryHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public async Task<List<TodoItem>> Handle(GetTodosByGroupQuery request, CancellationToken cancellationToken)
        {
            return await _todoItemRepository.GetTodoItemsByGroupIdAsync(request.GroupId);
        }
    }
}