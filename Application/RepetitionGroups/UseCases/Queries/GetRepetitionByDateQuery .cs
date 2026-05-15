using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepetitionGroups.UseCases.Queries
{
    public class GetRepetitionByDateQuery : IRequest<List<RepetedGroup>>
    {
        public DateTime Date { get; set; }
    }
}
