using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepetitionGroups.UseCases.Commands
{
    public class CompleteTodoItemRepetitionCommand : IRequest<bool>
    {
        public int TodoId { get; set; }
    }
}
