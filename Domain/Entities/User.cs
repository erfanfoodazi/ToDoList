using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public virtual ICollection<GroupItem>? GroupItems { get; set; }
        public virtual ICollection<TodoItem>? TodoItems { get; set; }

        public User()
        {
            CreatedAt = DateTime.Now;
            GroupItems = new List<GroupItem>();
            TodoItems = new List<TodoItem>();
        }
    }
}