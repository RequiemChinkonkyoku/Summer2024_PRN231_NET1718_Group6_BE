using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Models;
using Models.DTOs;
using Services.Implement;
using Services.Interface;

namespace DentalClinic_BE_OData.Controllers
{
    public class TreatmentsController : ODataController
    {
        private readonly ITreatmentService _treatmentService;
        public TreatmentsController(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var treatments = await _treatmentService.GetAllTreatment();
            return Ok(treatments);
        }

        [EnableQuery]
        public async Task<ActionResult<Treatment>> Get(int key)
        {
            var treatment = await _treatmentService.GetTreatmentByID(key);

            if (treatment != null)
            {
                return Ok(treatment);
            }
            else
            {
                return NotFound();
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] AddTreatmentRequest addTreatmentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var treatment = await _treatmentService.AddTreatmentAsync(addTreatmentRequest);

            if (treatment != null)
            {
                return Created(treatment);
            }
            else
            {
                return BadRequest();
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Patch(int key, [FromBody] UpdateTreatmentRequest updateTreatmentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedTreament = await _treatmentService.UpdateTreatmentAsync(key, updateTreatmentRequest);

            if (updatedTreament == null)
            {
                return NotFound();
            }


            return Ok(updatedTreament);
        }

        [EnableQuery]
        public async Task<IActionResult> Delete(int key)
        {
            var deletedtreatment = await _treatmentService.DeleteTreatmentAsync(key);
            if (deletedtreatment == null)
            {
                return NotFound();
            }

            return Ok(deletedtreatment);
        }
    }
}
