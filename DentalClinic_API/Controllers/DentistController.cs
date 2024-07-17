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
    public class DentistController : Controller
    {
        private HttpClient _httpClient;
        private IDentistService _dentistService;

        public DentistController(IDentistService dentistService)
        {
            _dentistService = dentistService;
            _httpClient = new HttpClient();
        }

        [HttpGet("get-all-dentists")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<List<Dentist>>> GetAllDentist()
        {
            var dentists = await _dentistService.GetAllDentistAsync();
            return Ok(dentists);
        }

        [HttpGet("get-all-dentists-odata")]
        public async Task<IActionResult> GetFromOdata()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5298/odata/Dentists");
            string json = await response.Content.ReadAsStringAsync();
            dynamic allDentists = JsonConvert.DeserializeObject<OdataResponse<IEnumerable<Dentist>>>(json);
            return Ok(allDentists);
        }


        [HttpGet("get-dentist-by-id/{id}")]
        public async Task<ActionResult<Dentist>> GetDentistById(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5298/odata/Dentists({id})");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var dentist = JsonConvert.DeserializeObject<Dentist>(json);
                return Ok(dentist);
            }
            else
            {
                return NotFound();
            }
        }

        //[HttpGet("get-dentist-by-id/{id}")]
        //public async Task<ActionResult<Dentist>> GetDentistById(int id)
        //{
        //    var dentist = await _dentistService.GetDentistByID(id);

        //    if (dentist != null)
        //    {
        //        return Ok(dentist);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        [HttpGet("get-current-dentist")]
        [Authorize(Roles = "Dentist")]
        public async Task<IActionResult> GetCurrentDentist()
        {
            int userId = 0;

            try
            {
                userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            var dentist = await _dentistService.GetDentistByID(userId);

            if (dentist != null)
            {
                return Ok(dentist);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("view-dentist-schedule/{id}")]
        public async Task<ActionResult<Schedule>> ViewSchedule(int id)
        {
            var schedule = await _dentistService.ViewSchedule(id);

            if (schedule != null)
            {
                return Ok(schedule);
            }
            else
            {
                return NotFound();
            }
        }

        //[HttpGet("view-dentist-profession/{id}")]
        //public async Task<ActionResult<ProfessionDetail>> ViewProfession(int id)
        //{
        //    var profession = await _dentistService.ViewProfession(id);

        //    if (profession != null)
        //    {
        //        return Ok(profession);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        [HttpGet("view-dentist-profession-id/{id}")]
        public async Task<ActionResult<ProfessionDetail>> ViewProfession(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5298/odata/Schedules({id})");
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

        [HttpPost("add-dentist")]
        public async Task<ActionResult<Dentist>> AddDentist([FromBody] AddDentistRequest addDentistRequest)
        {
            try
            {
                var json = JsonConvert.SerializeObject(addDentistRequest);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:5298/odata/Dentists", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Ok(result); 
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"Failed to add dentist: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpPost("add-dentist")]
        //[Authorize(Roles = "Manager")]
        //public async Task<ActionResult<Dentist>> AddDentist([FromBody] AddDentistRequest addDentistRequest)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var dentist = await _dentistService.DentistAdd(addDentistRequest);
        //    if (dentist != null)
        //    {
        //        return Ok(dentist);
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}

        [HttpPut("update-dentist/{id}")]
        public async Task<IActionResult> UpdateDentist(int id, [FromBody] UpdateDentistRequest updateDentistRequest)
        {
            try
            {
                var json = JsonConvert.SerializeObject(updateDentistRequest);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PatchAsync($"http://localhost:5298/odata/Dentists/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Ok(result); // Return the updated Dentist details or success message
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"Failed to update dentist: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpPut("update-dentist/{id}")]
        //public async Task<IActionResult> UpdateDentist(int id, [FromBody] UpdateDentistRequest updateDentistRequest)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var updatedDentist = await _dentistService.UpdateDentist(id, updateDentistRequest);

        //    if (updatedDentist == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(updatedDentist);
        //}

        [HttpPut("update-dentist-account/{id}")]
        public async Task<IActionResult> UpdateDentistAccount(int id, [FromBody] UpdateDentistAccountRequest updateDentistAccountRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedDentistAccount = await _dentistService.UpdateDentistAccount(id, updateDentistAccountRequest);

            if (updatedDentistAccount == null)
            {
                return NotFound();
            }

            return Ok(updatedDentistAccount);
        }

        [HttpPut("update-current-dentist")]
        public async Task<IActionResult> UpdateCurrentDentist([FromBody] UpdateDentistRequest updateDentistRequest)
        {
            int userId = 0;

            try
            {
                userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (updateDentistRequest == null)
            {
                return BadRequest();
            }

            var updatedDentist = await _dentistService.UpdateDentist(userId, updateDentistRequest);

            if (updatedDentist == null)
            {
                return NotFound();
            }

            return Ok(updatedDentist);
        }

        [HttpDelete("delete-dentist/{id}")]
        public async Task<IActionResult> DeleteDentist(int id)
        {
            var deletedentist = await _dentistService.DeleteDentist(id);
            if (deletedentist == null)
            {
                return NotFound();
            }

            return Ok(deletedentist);
        }

        [HttpDelete("delete-dentist-account/{id}")]
        public async Task<IActionResult> DeleteDentistAccount(int id)
        {
            var deleteDentistAccount = await _dentistService.DeleteDentistAccount(id);
            if (deleteDentistAccount == null)
            {
                return NotFound();
            }

            return Ok(deleteDentistAccount);
        }

        [HttpPost("get-dentist-for-app")]
        public async Task<IActionResult> GetDentistsForApp([FromBody] GetDentistsForAppRequest request)
        {
            int userId = 0;

            try
            {
                userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            var response = await _dentistService.GetDentistsForApp(request);

            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
