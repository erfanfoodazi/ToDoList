using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Created { get; set; } 
        public bool IsComplete { get; set; } = false;
        public Priority Priority { get; set; }
        public int? GroupItemId { get; set; }
        [ForeignKey("GroupItemId")]
        public GroupItem? GroupItem { get; set; }
        public int? RepitedGroupId{ get; set; }
        [ForeignKey("GroupRepetitionId")]
        public RepetedGroup? RepitedGroup { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public TodoItem() { }
        public TodoItem(string title, string description, int priority, int? userId = null, int? groupItemId = null)  // nullable و مقدار پیش‌فرض
        {
            Title = title;
            Description = description;
            Created = DateTime.Now;
            Priority = (Priority)priority;
            UserId = userId;
            GroupItemId = groupItemId;
        }
    }

}
