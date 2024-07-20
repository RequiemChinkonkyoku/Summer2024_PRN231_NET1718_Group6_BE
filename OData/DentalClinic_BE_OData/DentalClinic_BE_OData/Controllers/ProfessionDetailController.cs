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
    public class ProfessionDetailsController : ODataController
    {
        private readonly IDentistService _dentistService;

        public ProfessionDetailsController(IDentistService dentistService)
        {
            _dentistService = dentistService;
        }

        [EnableQuery]
        public async Task<ActionResult<ProfessionDetail>> Get(int key)
        {
            var professionDetails = await _dentistService.ViewProfession(key);
            if (professionDetails != null)
            {
                return Ok(professionDetails);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
