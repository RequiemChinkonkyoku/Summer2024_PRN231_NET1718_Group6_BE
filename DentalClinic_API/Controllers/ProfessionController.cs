using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Repositories.Interface;
using Services.Interface;

namespace DentalClinic_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfessionController : Controller
    {
        private readonly IProfessionService _profService;

        public ProfessionController(IProfessionService professionService)
        {
            _profService = professionService;
        }

        [HttpGet("get-all-professions")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllProfessions()
        {
            var professions = await _profService.GetAllProfessions();

            if (professions != null)
            {
                return Ok(professions);
            }
            else
            {
                return BadRequest("No profession found");
            }
        }

        [HttpPost("add-new-prof")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddNewProfession([FromBody] AddProfessionRequest request)
        {
            var response = await _profService.AddNewProfession(request);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.ErrorMessage);
            }
        }
    }
}
