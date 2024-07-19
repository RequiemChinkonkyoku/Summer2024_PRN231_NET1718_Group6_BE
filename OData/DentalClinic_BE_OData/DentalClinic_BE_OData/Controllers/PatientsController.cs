using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Models;
using Models.DTOs;
using Services.Implement;
using Services.Interface;

namespace DentalClinic_BE_OData.Controllers
{
    public class PatientsController : ODataController
    {
       private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService=patientService;
        }

        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var patients = await _patientService.GetAllPatient();
            return Ok(patients);
        }

        [EnableQuery]
        public async Task<ActionResult<Patient>> Get(int key)
        {

            var patient = await _patientService.GetPatientByID(key);

            if (patient != null)
            {
                return Ok(patient);
            }
            else
            {
                return NotFound();
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] AddPatientRequest addPatientRequest , int customerID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = await _patientService.AddPatientAsync(addPatientRequest, customerID);

            if (patient != null)
            {
                return Created(patient);
            }
            else
            {
                return BadRequest();
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Patch(int key, [FromBody] UpdatePatientRequest updatePatientRequest, int customerID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPatient = await _patientService.UpdatePatientAsync(key, updatePatientRequest, customerID);

            if (updatedPatient == null)
            {
                return NotFound();
            }


            return Ok(updatedPatient);
        }

        [EnableQuery]
        public async Task<IActionResult> Delete(int key)
        {
            var deletedPatient = await _patientService.DeletePatient(key);
            if (deletedPatient == null)
            {
                return NotFound();
            }

            return Ok(deletedPatient);
        }
    }
}
