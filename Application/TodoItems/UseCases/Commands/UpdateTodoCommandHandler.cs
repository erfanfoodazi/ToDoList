using Application.Interfaces;
using Domain.Enums;
using MediatR;

namespace Application.TodoItems.UseCases.Commands
{
    public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, bool>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        public UpdateTodoCommandHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }
        public async Task<bool> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            var existingTodo = await _todoItemRepository.GetTodoItemByIdAsync(request.Id);

            if (existingTodo == null)
                return false;

            var title = existingTodo.Title;

            var allTodoInThisGroup = await _todoItemRepository.GetTodoItemByGroupIdAsync(request.GroupItemId);
            
            bool allSuccess = true;

            foreach (var item in allTodoInThisGroup)
            {
                if(item.Title == title)
                {
                    item.Title = request.Title;
                    item.Description = request.Description;
                    item.Priority = (Priority)request.Priority;
                    item.GroupItemId = request.GroupItemId;
                    var result = await _todoItemRepository.UpdateTodoItemAsync(item);
                    if (!result)
                    {
                        allSuccess = false;
                    }
                }

            }
            

            
            return allSuccess;
        }
    }
}
