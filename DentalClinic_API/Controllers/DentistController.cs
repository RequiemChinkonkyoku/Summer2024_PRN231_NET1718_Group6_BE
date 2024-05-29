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

        [HttpGet(Name = "GetDentists")]
        public async Task<ActionResult<List<Dentist>>> GetAllDentist()
        {
            var patients = await _dentistService.GetAllDentistAsync();
            return Ok(patients);
        }

        [HttpGet("{id}", Name = "GetDentistWithID")]
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

        [HttpPost("login", Name = "DentistLogin")]
        public async Task<ActionResult<Dentist>> PatientLogin([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid request");
            }

            var dentist = await _dentistService.DentistLogin(loginRequest.Email, loginRequest.Password);

            if (dentist != null)
            {
                return Ok(dentist);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("add", Name = "AddDentist")]
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

        [HttpPut("{id}", Name = "UpdateDentist")]
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

        [HttpDelete("{id}", Name = "DeleteDentist")]
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
