using System.Collections.Generic;

namespace CVBackend.Dtos
{
    public class ExperienceDto
    {
        public string Id { get; set; }
        public string Company { get; set; }
        public string Role { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }

    public class ProjectDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }

    public class CvDto
    {
        public string Name { get; set; }
        public string LinkedIn { get; set; }
        public string GitHub { get; set; }
        public string ProfessionalSummary { get; set; }
        public List<ExperienceDto> Experience { get; set; }
        public List<ProjectDto> Projects { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
