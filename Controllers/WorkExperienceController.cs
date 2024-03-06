using CVBackend.Dtos;
using CVBackend.Models;
using CVBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CVBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WorkExperienceController : ControllerBase
    {
        private readonly WorkExperienceRepository _workExperienceRepository;

        public WorkExperienceController(WorkExperienceRepository workExperienceRepository)
        {
            _workExperienceRepository = workExperienceRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkExperience(WorkExperienceDto workExperience)
        {
            bool result = await _workExperienceRepository.AddWorkExperienceAsync(workExperience);
            if (result)
            {
                return Ok(workExperience); // Consider using CreatedAtAction for a more RESTful response
            }
            else
            {
                return BadRequest("Failed to add work experience.");
            }
        }

        // GET: api/WorkExperience
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkExperience>>> GetAllWorkExperiences()
        {
            var experiences = await _workExperienceRepository.GetAllWorkExperiencesAsync();
            return Ok(experiences);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkExperienceById(int id)
        {
            var workExperience = await _workExperienceRepository.GetWorkExperienceByIdAsync(id);
            if (workExperience != null)
            {
                return Ok(workExperience);
            }
            else
            {
                return NotFound($"Work Experience with ID {id} not found.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkExperience(int id, WorkExperience workExperience)
        {
            // Check if the work experience entry exists
            var existingWorkExperience = await _workExperienceRepository.GetWorkExperienceByIdAsync(id);
            if (existingWorkExperience == null)
            {
                return NotFound($"Work Experience with ID {id} not found.");
            }

            // Update the entry with new values
            bool updateResult = await _workExperienceRepository.UpdateWorkExperienceAsync(id, workExperience);
            if (updateResult)
            {
                return NoContent(); // Indicating the update was successful
            }
            else
            {
                return BadRequest("Failed to update work experience.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkExperience(int id)
        {
            // Attempt to delete the work experience entry
            bool deleteResult = await _workExperienceRepository.DeleteWorkExperienceAsync(id);
            if (deleteResult)
            {
                return Ok(); // Indicating the delete was successful
            }
            else
            {
                return NotFound($"Work Experience with ID {id} not found.");
            }
        }


    }
}