namespace UiWeb.Models
{
    public class ProfileUserViewModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int SuccessRate { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
