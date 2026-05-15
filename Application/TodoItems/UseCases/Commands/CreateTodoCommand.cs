using Application.RepetitionGroups.UseCases.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TodoItems.UseCases.Commands;

public class CreateTodoCommand : IRequest<int>
{
    public int GroupId { get; set; }
    public TodoItemDto TodoItem { get; set; }
}
