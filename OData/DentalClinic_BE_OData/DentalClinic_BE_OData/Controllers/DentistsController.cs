using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.UriParser;
using Models;
using Models.DTOs;
using Services.Interface;

namespace DentalClinic_BE_OData.Controllers
{
    
    public class DentistsController : ODataController
    {
        private readonly IDentistService _dentistService;

        public DentistsController(IDentistService dentistService)
        {
            _dentistService = dentistService;
        }

        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var dentists = await _dentistService.GetAllDentistAsync();
            return Ok(dentists);
        }

        [EnableQuery] 
        public async Task<ActionResult<Dentist>> Get(int key)
        {
            
            var dentist = await _dentistService.GetDentistByID(key);

            if (dentist != null)
            {
                return Ok(dentist);
            }
            else
            {
                return NotFound();
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] AddDentistRequest addDentistRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dentist = await _dentistService.DentistAdd(addDentistRequest);

            if (dentist != null)
            {
                return Created(dentist); 
            }
            else
            {
                return BadRequest();
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Patch( int key, [FromBody] UpdateDentistRequest updateDentistRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedDentist = await _dentistService.UpdateDentist(key, updateDentistRequest);

            if (updatedDentist == null)
            {
                return NotFound();
            }

            
            return Ok(updatedDentist);
        }

        

        //[EnableQuery]
        //[HttpPatch("{key}")]
        //public async Task<IActionResult> Patch(int key, [FromBody] UpdateDentistAccountRequest updateDentistAccountRequest) 
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var updateDentistAccount = await _dentistService.UpdateDentistAccount(key, updateDentistAccountRequest);
        //    if (updateDentistAccount == null) 
        //    {
        //        return NotFound();
        //    }
        //    return Ok(updateDentistAccount);
        //}

        [EnableQuery]
        [HttpPatch("delete-dentist-account")]
        public async Task<IActionResult> DeleteDentistAccount([FromODataUri] int key)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deleteDentistAccount = await _dentistService.DeleteDentistAccount(key);
            if (deleteDentistAccount == null) 
            {
                return NotFound();
            }
            return Ok(deleteDentistAccount);
        }

    }
}

