using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TodoItems.UseCases.Queries
{
    public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, List<TodoItem>>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        public GetAllTodosQueryHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }
        public async Task<List<TodoItem>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
        {
            return await _todoItemRepository.GetAllTodoItemsAsync();
        }
    }
}
