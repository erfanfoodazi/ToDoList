using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class GroupItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public RepetitionType RepetitionType { get; set; } = RepetitionType.None;
        public DateTime? CreatedAt { get; set; }
        public DateTime? EndRepit { get; set; }

        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        public virtual ICollection<TodoItem>? TodoItems { get; set; }
        public virtual ICollection<RepetedGroup>? RepetedGroups { get; set; }

        private GroupItem()
        {
            TodoItems = new List<TodoItem>();
            RepetedGroups = new List<RepetedGroup>();
        }

        public GroupItem(string title, int repetitionType, DateTime? endRepit, int userId)
        {
            Title = title;
            RepetitionType = (RepetitionType)repetitionType;
            CreatedAt = DateTime.Now;
            EndRepit = endRepit;
            UserId = userId;
            TodoItems = new List<TodoItem>();
            RepetedGroups = new List<RepetedGroup>();
        }
    }
}