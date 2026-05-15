using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TodoItems.UseCases.Queries
{
    public class GetTodosByGroupQuery : IRequest<List<TodoItem>>
    {
        public int GroupId { get; set; }
    }
}