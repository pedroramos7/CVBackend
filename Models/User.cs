using System.Collections.Generic;

namespace CVBackend.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string LinkedInLink { get; set; }
        public string GitHubLink { get; set; }
        public string Address { get; set; }
        public string Biography { get; set; }
        public string ProfessionalSummary { get; set; }

        public List<Experience> Experiences { get; set; }
        public List<Project> Projects { get; set; }
    }
}
