using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TodoItems.UseCases.Commands
{
    public class DeleteTodoCommand : IRequest<bool>
    {
        public int? GroupId {  get; set; }
        public string? Title { get; set; }
    }
}
