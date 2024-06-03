﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using Services.Implement;
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
        public async Task<IActionResult> AddPatient([FromBody] AddPatientRequest addPatientRequest)
        {
            
            var accountId = HttpContext.Session.GetInt32("PatientID");

            if (!accountId.HasValue)
            {
                return Unauthorized("User not logged in");
            }
            try
            {
                var _newpatient = await _patientService.AddPatientAsync(addPatientRequest, accountId.Value);
                return Ok(_newpatient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update-patient/{id}")]
        public async Task<IActionResult> UpdatePatient(int patientId, [FromBody] UpdatePatientRequest updatePatientRequest)
        {
            try
            {
                var accountId = HttpContext.Session.GetInt32("PatientID");

                if (!accountId.HasValue)
                {
                    return Unauthorized("User not logged in");
                }

                var _updatedPatient = await _patientService.UpdatePatientAsync(patientId, updatePatientRequest, accountId.Value);

                return Ok(_updatedPatient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
