namespace UiWeb.Models
{
    public class GroupTodosViewModel
    {
        public int GroupId { get; set; }
        public string GroupTitle { get; set; } = string.Empty;
        public string RepetitionType { get; set; } = string.Empty;
        public string? CreatedAt { get; set; }
        public List<RepetitionViewModel> Repetitions { get; set; } = new();
        public List<RateOfSuccess> RateOfSuccesses { get; set; } = new();
        public List<string> DistinctTaskNames => Repetitions
            .SelectMany(r => r.TodoItems)
            .Select(t => t.Title)
            .Distinct()
            .ToList();

        public string GetTaskPriority(string taskName)
        {
            var todo = Repetitions
                .SelectMany(r => r.TodoItems)
                .FirstOrDefault(t => t.Title == taskName);
            return todo?.Priority ?? "None";
        }
        public List<TaskInfo> TasksWithPriority
        {
            get
            {
                if (!Repetitions.Any())
                    return new List<TaskInfo>();

                return Repetitions
                    .SelectMany(r => r.TodoItems)
                    .GroupBy(t => t.Title)
                    .Select(g => new TaskInfo
                    {
                        Id = g.First().Id,
                        Title = g.Key,
                        Priority = g.First().Priority,
                        Description = g.First().Description ?? "",
                        GroupId = g.First().GroupId 
                    })
                    .ToList();
            }
        }
        public class TaskInfo
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Priority { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public int GroupId { get; set; }
        }
    }
}