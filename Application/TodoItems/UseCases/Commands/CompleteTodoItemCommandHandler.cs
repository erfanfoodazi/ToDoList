using Application.Interfaces;
using MediatR;

namespace Application.TodoItems.UseCases.Commands
{
    public class CompleteTodoItemCommandHandler : IRequestHandler<CompleteTodoItemCommand, bool>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        public CompleteTodoItemCommandHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }
        public async Task<bool> Handle(CompleteTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todo =await _todoItemRepository.GetTodoItemByIdAsync(request.TodoId);

            if( todo == null )
                throw new ArgumentException("Can't find any task with this id");

            var result = await _todoItemRepository.IsCompleteTodoItemAsync(request.TodoId);

            if(!result)
                throw new ArgumentException("Can't complete this task");

            return result;

        }
    }
}
