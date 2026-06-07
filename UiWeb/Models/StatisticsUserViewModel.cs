namespace UiWeb.Models
{
    public class StatisticsUserViewModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<RateOfSuccess>? LowPriority { get; set; }
        public List<RateOfSuccess>? MediumPriority { get; set; }
        public List<RateOfSuccess>? HighPriority { get; set; }
        public List<RateOfSuccess>? CriticalPriority { get; set; }
    }

}