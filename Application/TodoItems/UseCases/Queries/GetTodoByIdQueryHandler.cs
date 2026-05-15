using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TodoItems.UseCases.Queries
{
    public class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, TodoItem?>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        public GetTodoByIdQueryHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }
        public async Task<TodoItem?> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
        {
            return await _todoItemRepository.GetTodoItemByIdAsync(request.Id);
        }
    }
}
