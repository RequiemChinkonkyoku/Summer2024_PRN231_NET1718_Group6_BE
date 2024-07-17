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
    public class MedicalRecordController : Controller
    {
        private IMedicalRecordService _medicalRecordService;
        public MedicalRecordController(IMedicalRecordService medicalRecordService) {
            _medicalRecordService = medicalRecordService;
        }

        [HttpGet("get-medical-record/{id}")]
        public async Task<ActionResult<MedicalRecord>> ViewMedicalRecord(int id)
        {
            var medicalrecord = await _medicalRecordService.ViewMedicalRecord(id);

            if (medicalrecord != null)
            {
                return Ok(medicalrecord);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
