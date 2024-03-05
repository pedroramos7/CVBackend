using Microsoft.AspNetCore.Mvc;
using CVBackend.Models; // Adjust this to point to your Models namespace
using CVBackend.Repositories; // Adjust this for the repository namespace
using System.Threading.Tasks;
using CVBackend.Dtos;

namespace CVBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalInfoController : ControllerBase
    {
        private readonly PersonalInfoRepository _personalInfoRepository;

        public PersonalInfoController(PersonalInfoRepository personalInfoRepository)
        {
            _personalInfoRepository = personalInfoRepository;
        }

        // GET: api/PersonalInfo
        [HttpGet]
        public async Task<ActionResult> GetAllPersonalInfo()
        {
            var personalInfos = await _personalInfoRepository.GetAllAsync();
            return Ok(personalInfos);
        }

        [HttpPost]
        public async Task<IActionResult> AddPersonalInfo(PersonalInfoDto personalInfoDto)
        {
            var personalInfo = new PersonalInfo
            {
                Name = personalInfoDto.Name,
                JobTitle = personalInfoDto.JobTitle,
                Email = personalInfoDto.Email,
                ContactNumber = personalInfoDto.ContactNumber,
                LinkedInLink = personalInfoDto.LinkedInLink,
                Address = personalInfoDto.Address
            };

            bool result = await _personalInfoRepository.AddPersonalInfoAsync(personalInfo);
            if(result)
            {
                return Ok(personalInfo); // or return CreatedAtAction if you prefer
            }
            else
            {
                return BadRequest("Data could not be added.");
            }
        }

        // Additional methods (POST, PUT, DELETE) to manipulate the personal info can be added here
    }
}
