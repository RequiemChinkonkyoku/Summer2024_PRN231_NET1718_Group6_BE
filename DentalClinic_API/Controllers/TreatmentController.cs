using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Services.Implement;
using Services.Interface;

namespace DentalClinic_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TreatmentController : Controller
    {
        private ITreatmentService _treatmentService;
        public TreatmentController(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        [HttpGet("get-all-treatment")]
        public async Task<ActionResult<List<Treatment>>> GetAllTreatment()
        {
            var patients = await _treatmentService.GetAllTreatment();
            return Ok(patients);
        }

        [HttpGet("get-treatment-by-id/{id}")]
        public async Task<ActionResult<Dentist>> GetTreatmentById(int id)
        {
            var treatment = await _treatmentService.GetTreatmentByID(id);

            if (treatment != null)
            {
                return Ok(treatment);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("add-treatment")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Treatment>> AddTreatment([FromBody] AddTreatmentRequest addTreatmentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var treatment = await _treatmentService.AddTreatmentAsync(addTreatmentRequest);
            if (treatment != null)
            {
                return Ok(treatment);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("update-treatment/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateTreatment(int id, [FromBody] UpdateTreatmentRequest updateTreatmentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updateTreatment = await _treatmentService.UpdateTreatmentAsync(id, updateTreatmentRequest);
            if (updateTreatment == null)
            {
                return NotFound();
            }
            return Ok(updateTreatment);
        }

        [HttpDelete("delete-treatment/{id}")]
        public async Task<IActionResult> DeleteTreatment(int id)
        {
            var deletetreatment = await _treatmentService.DeleteTreatmentAsync(id);
            if (deletetreatment == null)
            {
                return NotFound();
            }

            return Ok(deletetreatment);
        }
    }
}
