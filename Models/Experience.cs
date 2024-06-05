namespace CVBackend.Models
{
    public class Experience
    {
        public int ExperienceId { get; set; }
        public int UserId { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public int Order { get; set; }
    }
}
