using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Models;
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

        public async Task<IActionResult> Get()
        {
            var dentists = await _dentistService.GetAllDentistAsync();
            return Ok(dentists);
        }
    }
}
