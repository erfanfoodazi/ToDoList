using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TodoItems.UseCases.Queries
{
    public class GetTodosByPriorityQuery : IRequest<List<TodoItem>>
    {
        public Priority Priority { get; set; }
    }
}