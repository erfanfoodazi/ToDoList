using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepetitionGroups.UseCases.Commands
{
    public class CompleteRepetitionCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
