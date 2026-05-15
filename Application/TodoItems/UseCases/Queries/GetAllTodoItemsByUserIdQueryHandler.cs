using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TodoItems.UseCases.Queries
{
    public class GetAllTodoItemsByUserIdQueryHandler : IRequestHandler<GetAllTodoItemsByUserIdQuery, List<TodoItem>>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        public GetAllTodoItemsByUserIdQueryHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }
        public async Task<List<TodoItem>> Handle(GetAllTodoItemsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _todoItemRepository.GetAllTodoItemByUserIdAsync(request.UserId);
        }
    }
}
