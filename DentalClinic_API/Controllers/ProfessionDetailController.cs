using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Newtonsoft.Json;
using Services.Implement;
using Services.Interface;
using System.Net.Http;
using System.Security.Claims;

namespace DentalClinic_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfessionDetailController : Controller
    {
        private HttpClient _httpClient;
        private IDentistService _dentistService;
        public ProfessionDetailController(IDentistService dentistService)
        {
            _dentistService = dentistService;
            _httpClient = new HttpClient();
        }

        [HttpGet("view-dentist-profession/{id}")]
        public async Task<ActionResult<ProfessionDetail>> ViewProfession(int id)
        {
            var profession = await _dentistService.ViewProfession(id);

            if (profession != null)
            {
                return Ok(profession);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("view-dentist-profession-odata/{id}")]
        public async Task<ActionResult<ProfessionDetail>> ViewProfessionOdata(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5298/odata/ProfessionDetails({id})");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var professionDetail = JsonConvert.DeserializeObject<ProfessionDetail>(json);
                return Ok(professionDetail);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
