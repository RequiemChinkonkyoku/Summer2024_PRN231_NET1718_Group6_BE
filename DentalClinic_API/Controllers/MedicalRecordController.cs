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
    public class MedicalRecordController : Controller
    {
        private readonly IMedicalRecordService _medicalRecordService;
        public MedicalRecordController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }


        [HttpGet("get-all-medical-records")]
        public async Task<ActionResult<List<MedicalRecord>>> GetAllMedicalRecord()
        {
            var medicalRecords = await _medicalRecordService.GetAllMedicalRecordAsync();
            return Ok(medicalRecords);
        }

        [HttpGet("get-medical-record/{id}")]
        public async Task<ActionResult<MedicalRecord>> ViewMedicalRecord(int id)
        {
            var medicalRecord = await _medicalRecordService.ViewMedicalRecord(id);

            if (medicalRecord != null)
            {
                return Ok(medicalRecord);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("get-medical-record-by-app-id/{id}")]
        public async Task<ActionResult<MedicalRecord>> GetMedicalRecordByAppId(int id)
        {
            var medicalRecord = await _medicalRecordService.GetMedicalRecordByAppId(id);

            if (medicalRecord != null)
            {
                return Ok(medicalRecord);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("create-medical-record")]
        public async Task<IActionResult> AddMedicalRecord([FromBody] AddMedicalRecordRequest addMedicalRecordRequest, int appointmentID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var medicalRecord = await _medicalRecordService.AddMedicalRecordAsync(addMedicalRecordRequest, appointmentID);
                return CreatedAtAction(nameof(ViewMedicalRecord), new { id = medicalRecord.RecordId }, medicalRecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update-medical-record/{id}")]
        public async Task<IActionResult> UpdateMedicalRecord(int id, [FromBody] UpdateMedicalRecordRequest updateMedicalRecordRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var medicalRecord = await _medicalRecordService.UpdateMedicalRecordAsync(id ,updateMedicalRecordRequest);
                return Ok(medicalRecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
