using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TodoItems.UseCases.Queries
{
    public class GetTodosByPriorityQueryHandler : IRequestHandler<GetTodosByPriorityQuery, List<TodoItem>>
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public GetTodosByPriorityQueryHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public async Task<List<TodoItem>> Handle(GetTodosByPriorityQuery request, CancellationToken cancellationToken)
        {
            var allTodos = await _todoItemRepository.GetAllTodoItemsAsync();
            return allTodos.Where(t => t.Priority == request.Priority).ToList();
        }
    }
}