using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Services.Interface;

namespace DentalClinic_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : Controller
    {
        private IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet(Name = "GetPatient")]
        public async Task<ActionResult<List<Patient>>> GetPatients()
        {
            var patients = await _patientService.GetAllPatient();
            return Ok(patients);
        }

        [HttpGet("{id}", Name = "GetPatientWithID")]
        public async Task<ActionResult<Patient>> GetPatientById(int id)
        {
            var patient = await _patientService.GetPatientByID(id);

            if (patient != null)
            {
                return Ok(patient);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("login", Name = "PatientLogin")]
        public async Task<ActionResult<Patient>> PatientLogin([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid request");
            }

            var patient = await _patientService.PatientLogin(loginRequest.Email, loginRequest.Password);

            if (patient != null)
            {
                return Ok(new { patient, redirectUrl = "https://www.youtube.com/watch?v=v56H49q_elw&list=RDMM1iDk_rHA7FM&index=6" });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
