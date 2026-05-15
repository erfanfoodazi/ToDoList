// UiWeb/Models/TodoItemViewModel.cs
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UiWeb.Models
{
    public class TodoItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Priority { get; set; } = "1";
        public List<SelectListItem>? PriorityOptions { get; set; }
        public bool IsComplete { get; set; }
        public string? CreatedAt { get; set; }
        public int GroupId { get; set; }
    }
}