using Microsoft.AspNetCore.Mvc;
using CVBackend.Dtos;
using CVBackend.Repositories;
using CVBackend.Models;
using System.Threading.Tasks;
using System.Linq;

namespace CVBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CvController : ControllerBase
    {
        private readonly CvRepository _cvRepository;

        public CvController(CvRepository cvRepository)
        {
            _cvRepository = cvRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCv(int userId)
        {
            var user = await _cvRepository.GetUserAsync(userId);
            if (user == null)
            {
                return NotFound("CV not found");
            }

            var cvDto = new CvDto
            {
                Name = user.Name,
                LinkedIn = user.LinkedInLink,
                GitHub = user.GitHubLink,
                ProfessionalSummary = user.ProfessionalSummary,
                Experience = user.Experiences.Select(e => new ExperienceDto
                {
                    Id = e.ExperienceId.ToString(),
                    Company = e.JobTitle,
                    Role = e.JobTitle,
                    StartDate = e.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = e.EndDate.ToString("yyyy-MM-dd"),
                    Description = e.Description,
                    Order = e.Order
                }).ToList(),
                Projects = user.Projects.Select(p => new ProjectDto
                {
                    Id = p.ProjectId.ToString(),
                    Title = p.Title,
                    Role = p.Role,
                    StartDate = p.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = p.EndDate.ToString("yyyy-MM-dd"),
                    Description = p.Description,
                    Order = p.Order
                }).ToList(),
                Address = user.Address,
                Email = user.Email
            };

            return Ok(cvDto);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCv([FromBody] CvDto cvDto)
        {
            var user = new User
            {
                UserId = 1, // Assuming a single user for simplicity
                Name = cvDto.Name,
                LinkedInLink = cvDto.LinkedIn,
                GitHubLink = cvDto.GitHub,
                ProfessionalSummary = cvDto.ProfessionalSummary,
                Experiences = cvDto.Experience.Select(e => new Experience
                {
                    ExperienceId = int.TryParse(e.Id, out var id) ? id : 0,
                    JobTitle = e.Company,
                    Description = e.Description,
                    StartDate = DateTime.Parse(e.StartDate),
                    EndDate = DateTime.Parse(e.EndDate),
                    Location = e.Role,
                    Order = e.Order
                }).ToList(),
                Projects = cvDto.Projects.Select(p => new Project
                {
                    ProjectId = int.TryParse(p.Id, out var id) ? id : 0,
                    Title = p.Title,
                    Role = p.Role,
                    Description = p.Description,
                    StartDate = DateTime.Parse(p.StartDate),
                    EndDate = DateTime.Parse(p.EndDate),
                    Order = p.Order
                }).ToList(),
                Address = cvDto.Address,
                Email = cvDto.Email
            };

            var success = await _cvRepository.SaveUserAsync(user);
            if (success)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500, "An error occurred while saving the CV.");
            }
        }
    }
}
