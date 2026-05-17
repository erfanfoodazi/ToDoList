using Application.Interfaces;
using MediatR;

namespace Application.TodoItems.UseCases.Commands
{
    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, bool>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        public DeleteTodoCommandHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }
        public async Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            return await _todoItemRepository.DeleteTodoItemByTitleAsync(request.Title, (int)request.GroupId);
        }
    }
}
