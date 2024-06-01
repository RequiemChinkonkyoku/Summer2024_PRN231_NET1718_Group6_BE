using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Services.Implement;
using Services.Interface;

namespace DentalClinic_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DentistController : Controller
    {
        private IDentistService _dentistService;

        public DentistController(IDentistService dentistService)
        {
            _dentistService = dentistService;
        }

        [HttpGet("get-all-dentists")]
        public async Task<ActionResult<List<Dentist>>> GetAllDentist()
        {
            var dentists = await _dentistService.GetAllDentistAsync();
            return Ok(dentists);
        }

        [HttpGet("get-dentist-by-id/{id}")]
        public async Task<ActionResult<Dentist>> GetDentistById(int id)
        {
            var dentist = await _dentistService.GetDentistByID(id);

            if (dentist != null)
            {
                return Ok(dentist);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("dentist-login")]
        public async Task<ActionResult<Dentist>> PatientLogin([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid request");
            }

            var dentist = await _dentistService.DentistLogin(loginRequest.Email, loginRequest.Password);

            if (dentist != null)
            {
                HttpContext.Session.SetInt32("DentistID", dentist.DentistId);
                HttpContext.Session.SetString("AccountType", "Dentist");
                return Ok(dentist);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            Response.Cookies.Delete(".CCP.Session");

            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("get-session")]
        public IActionResult GetSession()
        {
            var _session = HttpContext.Session;
            var dentistID = _session.GetInt32("DentistID");

            if (dentistID != null)
            {
                return Ok(new { DentistID = dentistID });
            }
            else
            {
                return NotFound("Session data not found.");
            }
        }

        [HttpPost("add-dentist")]
        public async Task<ActionResult<Dentist>> AddDentist([FromBody] AddDentistRequest addDentistRequest)
        {
            if (addDentistRequest == null)
            {
                return BadRequest();
            }

            var dentist = await _dentistService.DentistAdd(addDentistRequest);
            if (dentist != null)
            {
                return Ok(dentist);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("update-dentist/{id}")]
        public async Task<IActionResult> UpdateDentist(int id, [FromBody] UpdateDentistRequest updateDentistRequest)
        {
            if (updateDentistRequest == null)
            {
                return BadRequest();
            }

            var updatedDentist = await _dentistService.UpdateDentist(id, updateDentistRequest);

            if (updatedDentist == null)
            {
                return NotFound();
            }

            return Ok(updatedDentist);
        }

        [HttpDelete("delete-dentist/{id}")]
        public async Task<IActionResult> DeleteDentist(int id)
        {
            var deletedentist = await _dentistService.DeleteDentist(id);
            if (deletedentist == null)
            {
                return NotFound();
            }

            return Ok(deletedentist);
        }
    }
}
