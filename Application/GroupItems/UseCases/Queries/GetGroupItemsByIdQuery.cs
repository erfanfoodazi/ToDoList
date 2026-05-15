using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GroupItems.UseCases.Queries
{
    public class GetGroupItemsByIdQuery : IRequest<GroupItem?>
    {
        public int Id { get; set; }
    }
}
