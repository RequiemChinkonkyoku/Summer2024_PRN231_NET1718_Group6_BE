using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Newtonsoft.Json;
using Services.Implement;
using Services.Interface;

namespace DentalClinic_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TreatmentController : Controller
    {
        private ITreatmentService _treatmentService;
        private HttpClient _httpClient;
        public TreatmentController(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
            _httpClient = new HttpClient();
        }

        [HttpGet("get-all-treatment")]
        public async Task<ActionResult<List<Treatment>>> GetAllTreatment()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5298/odata/Treatments");
            string json = await response.Content.ReadAsStringAsync();
            dynamic allTreatments = JsonConvert.DeserializeObject<OdataResponse<IEnumerable<Treatment>>>(json);
            return Ok(allTreatments);
        }

        //[HttpGet("get-all-treatment")]
        //public async Task<ActionResult<List<Treatment>>> GetAllTreatment()
        //{
        //    var patients = await _treatmentService.GetAllTreatment();
        //    return Ok(patients);
        //}

        [HttpGet("get-treatment-by-id-odata/{id}")]
        public async Task<ActionResult<Treatment>> GetTreatmentByIdOdata(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5298/odata/Treatments({id})?$expand=Professions");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var treatment = JsonConvert.DeserializeObject<Treatment>(json);
                return Ok(treatment);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("get-treatment-by-id/{id}")]
        public async Task<ActionResult<Dentist>> GetTreatmentById(int id)
        {
            var treatment = await _treatmentService.GetTreatmentByID(id);

            if (treatment != null)
            {
                return Ok(treatment);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("add-treatment")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Treatment>> AddTreatment([FromBody] AddTreatmentRequest addTreatmentRequest)
        {
            try
            {
                var json = JsonConvert.SerializeObject(addTreatmentRequest);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:5298/odata/Treatments", content);

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

        //[HttpPost("add-treatment")]
        //[Authorize(Roles = "Manager")]
        //public async Task<ActionResult<Treatment>> AddTreatment([FromBody] AddTreatmentRequest addTreatmentRequest)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var treatment = await _treatmentService.AddTreatmentAsync(addTreatmentRequest);
        //    if (treatment != null)
        //    {
        //        return Ok(treatment);
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}

        [HttpPut("update-treatment/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateTreatment(int id, [FromBody] UpdateTreatmentRequest updateTreatmentRequest)
        {
            try
            {
                var json = JsonConvert.SerializeObject(updateTreatmentRequest);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PatchAsync($"http://localhost:5298/odata/Treatments/{id}", content);

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

        //[HttpPut("update-treatment/{id}")]
        //[Authorize(Roles = "Manager")]
        //public async Task<IActionResult> UpdateTreatment(int id, [FromBody] UpdateTreatmentRequest updateTreatmentRequest)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var updateTreatment = await _treatmentService.UpdateTreatmentAsync(id, updateTreatmentRequest);
        //    if (updateTreatment == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(updateTreatment);
        //}

        [HttpDelete("delete-treatment/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteTreatment(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:5298/odata/Treatments/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Ok(result);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"Failed to delete dentist: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpDelete("delete-treatment/{id}")]
        //public async Task<IActionResult> DeleteTreatment(int id)
        //{
        //    var deletetreatment = await _treatmentService.DeleteTreatmentAsync(id);
        //    if (deletetreatment == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(deletetreatment);
        //}
    }
}
