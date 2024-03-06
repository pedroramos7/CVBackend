using CVBackend.Dtos;
using CVBackend.Models;
using CVBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CVBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutmeController : ControllerBase
    {   
        private readonly AboutmeRepository _aboutMeRepository;

        public AboutmeController(AboutmeRepository aboutMeRepository)
        {
            _aboutMeRepository = aboutMeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddAboutMe(AboutmeDto  aboutMeDto)
        {
             var aboutMe = new Aboutme
            {
                Biography = aboutMeDto.Biography,
            };
            bool result = await _aboutMeRepository.AddAboutMeAsync(aboutMe);
            if (result)
            {
                return Ok(aboutMeDto);
            }
            else
            {
                return BadRequest("Failed to add about me content.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAboutMe(int id)
        {
            var aboutMe = await _aboutMeRepository.GetAboutMeAsync(id);
            if (aboutMe != null)
            {
                return Ok(aboutMe);
            }
            else
            {
                return NotFound($"About Me content with ID {id} not found.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAboutMe(int id, AboutmeDto aboutMeDto)
        {
            bool result = await _aboutMeRepository.UpdateAboutMeAsync(id, aboutMeDto);
            if (result)
            {
                return Ok(new { Message = "About me information updated successfully.", aboutMe = aboutMeDto.Biography });
            }
            else
            {
                return NotFound($"About Me content with ID {id} not found.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAboutMe(int id)
        {
            bool result = await _aboutMeRepository.DeleteAboutMeAsync(id);
            if (result)
            {
                return Ok(new {Message = $"ID {id} About me information was deleted successfuly"});
            }
            else
            {
                return NotFound($"About Me content with ID {id} not found.");
            }
        }


    }
}