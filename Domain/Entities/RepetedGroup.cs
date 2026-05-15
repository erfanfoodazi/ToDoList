using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RepetedGroup
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public GroupItem? GroupItem { get; set; }
        public DateTime RepetitionDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }
        public List<TodoItem>? TodoItems { get; set; }

        public RepetedGroup() { }

        public RepetedGroup(int groupId, List<TodoItem> todoItems, DateTime repetitionDate)
        {
            GroupId = groupId;
            RepetitionDate = repetitionDate;
            TodoItems = todoItems;
        }
    }
}
