using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GroupItems.UseCases.Commands
{
    public class UpdateGroupItemCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int RepetitionType { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
