using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Application.TodoItems.UseCases.Commands
{
    public class DeleteTodoByIdCommand : IRequest<bool>
    {
        public int TodoId { get; set; }
    }

    public class DeleteTodoByIdCommandHandler : IRequestHandler<DeleteTodoByIdCommand, bool>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        public DeleteTodoByIdCommandHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }
        public async Task<bool> Handle(DeleteTodoByIdCommand request, CancellationToken cancellationToken)
        {
            return await _todoItemRepository.DeleteTodoItemByTodoIdAsync(request.TodoId);
        }
    }
}
