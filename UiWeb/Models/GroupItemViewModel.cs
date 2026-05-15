// UiWeb/Models/GroupItemViewModel.cs
namespace UiWeb.Models
{
    public class GroupItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string RepetitionType { get; set; } = string.Empty;
        public int RepetitionTypeValue { get; set; }
        public string? CreatedAt { get; set; }
        public string? EndDate { get; set; }
        public int TodoItemsCount { get; set; }
        public List<TodoItemViewModel>? TodoItems { get; set; }
    }
}