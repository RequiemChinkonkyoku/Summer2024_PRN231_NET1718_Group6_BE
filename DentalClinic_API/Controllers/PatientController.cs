using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
    }
}
