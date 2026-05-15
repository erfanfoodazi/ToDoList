using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepetitionGroups.UseCases.Queries
{
    public class GetAllRepetitionByGroupQuery : IRequest<List<RepetedGroup>>
    {
        public int GroupId { get; set; }
    }
}
