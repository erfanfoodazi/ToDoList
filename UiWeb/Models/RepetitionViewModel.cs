namespace UiWeb.Models
{
    public class RepetitionViewModel
    {
        public int Id { get; set; }
        public string RepetitionDate { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<TodoItemViewModel> TodoItems { get; set; } = new();
    }
}