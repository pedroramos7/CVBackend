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


        [HttpGet("{id}")]
        public async Task<ActionResult<PersonalInfo>> GetPersonalInfoById(int id)
        {
            var personalInfo = await _personalInfoRepository.GetByIdAsync(id);
            if (personalInfo == null)
            {
                return NotFound($"Personal info with ID {id} not found.");
            }
            return Ok(personalInfo);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonalInfo(int id, PersonalInfoDto personalInfoDto)
        {
            var personalInfoToUpdate = await _personalInfoRepository.GetByIdAsync(id);
            if (personalInfoToUpdate == null)
            {
                return NotFound($"Personal info with ID {id} not found.");
            }

            // Update the entity based on the DTO
            personalInfoToUpdate.Name = personalInfoDto.Name;
            personalInfoToUpdate.JobTitle = personalInfoDto.JobTitle;
            personalInfoToUpdate.Email = personalInfoDto.Email;
            personalInfoToUpdate.ContactNumber = personalInfoDto.ContactNumber;
            personalInfoToUpdate.LinkedInLink = personalInfoDto.LinkedInLink;
            personalInfoToUpdate.Address = personalInfoDto.Address;

            bool result = await _personalInfoRepository.UpdatePersonalInfoAsync(personalInfoToUpdate);
            if (result)
            {
                return Ok(new { Message = "Personal information updated successfully.", UpdatedPersonalInfo = personalInfoToUpdate });
 
            }
            else
            {
                return BadRequest("Update failed.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonalInfo(int id)
        {
            var personalInfoToDelete = await _personalInfoRepository.GetByIdAsync(id);
            if (personalInfoToDelete == null)
            {
                return NotFound($"Personal info with ID {id} not found.");
            }

            var result = await _personalInfoRepository.DeletePersonalInfoAsync(id);
            if(result)
            {
                return Ok(new {Message = $"ID {id} Personal information was deleted successfuly"});
            }
            else
            {
                return BadRequest("Delete operation failed");
            }


        }

    }
}
