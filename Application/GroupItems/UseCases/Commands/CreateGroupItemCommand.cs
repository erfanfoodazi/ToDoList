using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GroupItems.UseCases.Commands
{
    public class CreateGroupItemCommand : IRequest<GroupItem>
    {
        public string Title { get; set; } = string.Empty;
        public int RepetitionType { get; set; }
        public DateTime? EndDate { get; set; }
        public int UserId { get; set; }
    }
}
