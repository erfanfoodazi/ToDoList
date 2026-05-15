using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RepetedGroups.UseCases.Commands
{
    public class GenerateRepetitionsByDateRepetitionCommand : IRequest<int>
    {
        public int GroupId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
    }
}