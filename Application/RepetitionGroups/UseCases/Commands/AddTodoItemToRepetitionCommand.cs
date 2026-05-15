using Application.RepetitionGroups.UseCases.Dtos;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepetitionGroups.UseCases.Commands
{
    public class AddTodoItemToRepetitionCommand : IRequest<bool>
    {
        public int GroupId { get; set; }
        public TodoItemDto TodoItem { get; set; }
    }
}
