using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepetitionGroups.UseCases.Dtos
{
    public class TodoItemDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Created { get; set; }
        public bool IsComplete { get; set; } = false;
        public int Priority { get; set; }
        public int? GroupItemId { get; set; }
        public int? RepitedGroupId { get; set; }
        public int? UserId { get; set; }
    }
}
