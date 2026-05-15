using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TodoItems.UseCases.Queries
{
    public class GetTodosByStatusQueryHandler : IRequestHandler<GetTodosByStatusQuery, List<TodoItem>>
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public GetTodosByStatusQueryHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public async Task<List<TodoItem>> Handle(GetTodosByStatusQuery request, CancellationToken cancellationToken)
        {
            var allTodos = await _todoItemRepository.GetAllTodoItemsAsync();
            return allTodos.Where(t => t.IsComplete == request.IsComplete).ToList();
        }
    }
}