using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TodoItems.UseCases.Commands
{
    public class CompleteTodoItemCommand : IRequest<bool>
    {
        public int TodoId { get; set; } 
    }
}
