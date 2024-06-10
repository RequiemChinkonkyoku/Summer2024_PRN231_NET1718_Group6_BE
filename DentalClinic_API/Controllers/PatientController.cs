using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using Services.Implement;
using Services.Interface;
using System.Security.Claims;

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

        [HttpGet("get-all-patients")]
        public async Task<ActionResult<List<Patient>>> GetAllPatients()
        {
            var patients = await _patientService.GetAllPatient();
            return Ok(patients);
        }

        [HttpGet("get-patient-by-id/{id}")]
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

        [HttpDelete("delete-patient/{id}")]
        public async Task<IActionResult> DeleteDentist(int id)
        {
            var deletepatient = await _patientService.DeletePatient(id);
            if (deletepatient == null)
            {
                return NotFound();
            }

            return Ok(deletepatient);
        }

        [HttpGet("get-medical-record/{id}")]
        public async Task<ActionResult<MedicalRecord>> ViewMedicalRecord(int id)
        {
            var medicalrecord = await _patientService.ViewMedicalRecord(id);

            if (medicalrecord != null)
            {
                return Ok(medicalrecord);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("get-clinic-schedule")]
        public async Task<ActionResult<List<Schedule>>> ViewClinicSchedule()
        {
            var schedules = await _patientService.ViewClinicScheduleAsync();
            return Ok(schedules);
        }

        [HttpPost("add-patient")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddPatient([FromBody] AddPatientRequest addPatientRequest)
        {
            int userId = 0;

            try
            {
                userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            var _newPatient = await _patientService.AddPatientAsync(addPatientRequest, userId);
            return Ok(_newPatient);
        }

        [HttpPut("update-patient/{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientRequest updatePatientRequest)
        {
            int userId = 0;

            try
            {
                userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            var _updatedPatient = await _patientService.UpdatePatientAsync(id, updatePatientRequest, userId);
            return Ok(_updatedPatient);
        }

        [HttpGet("get-patient-list-by-customer")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetPatientListByCustomer()
        {
            int userId = 0;

            try
            {
                userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            var response = await _patientService.GetPatientListByCustomer(userId);

            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
