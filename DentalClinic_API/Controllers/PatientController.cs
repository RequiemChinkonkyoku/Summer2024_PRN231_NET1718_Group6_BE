using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
    public class PatientController : Controller
    {
        private IPatientService _patientService;
        private HttpClient _httpClient;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
            _httpClient = new HttpClient();
        }

        [HttpGet("get-all-patients")]
        public async Task<ActionResult<List<Patient>>> GetAllPatients()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5298/odata/Patients");
            string json = await response.Content.ReadAsStringAsync();
            dynamic allPatients = JsonConvert.DeserializeObject<OdataResponse<IEnumerable<Patient>>>(json);
            return Ok(allPatients);
        }

        //[HttpGet("get-all-patients")]
        //public async Task<ActionResult<List<Patient>>> GetAllPatients()
        //{
        //    var patients = await _patientService.GetAllPatient();
        //    return Ok(patients);
        //}



        [HttpGet("get-patient-by-id/{id}")]
        public async Task<ActionResult<Patient>> GetPatientById(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5298/odata/Patients({id})");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var patient = JsonConvert.DeserializeObject<Patient>(json);
                return Ok(patient);
            }
            else
            {
                return NotFound();
            }
        }

        //[HttpGet("get-patient-by-id/{id}")]
        //public async Task<ActionResult<Patient>> GetPatientById(int id)
        //{
        //    var patient = await _patientService.GetPatientByID(id);

        //    if (patient != null)
        //    {
        //        return Ok(patient);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        [HttpDelete("delete-patient/{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:5298/odata/Patients/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Ok(result);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"Failed to delete Patients: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpDelete("delete-patient/{id}")]
        //public async Task<IActionResult> DeletePatient(int id)
        //{
        //    var deletepatient = await _patientService.DeletePatient(id);
        //    if (deletepatient == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(deletepatient);
        //}


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

        [HttpPost("add-patient")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddPatient([FromBody] AddPatientRequest addPatientRequest)
        {
            int userId = 0;

            try
            {
                userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
                
                    var json = JsonConvert.SerializeObject(addPatientRequest);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _httpClient.PostAsync($"http://localhost:5298/odata/Patients/?customerID={userId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        return Ok(result);
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, $"Failed to add Patients: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

        //[HttpPost("add-patient")]
        //[Authorize(Roles = "Customer")]
        //public async Task<IActionResult> AddPatient([FromBody] AddPatientRequest addPatientRequest)
        //{
        //    int userId = 0;

        //    try
        //    {
        //        userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //    var _newPatient = await _patientService.AddPatientAsync(addPatientRequest, userId);
        //    return Ok(_newPatient);
        //}

        [HttpPut("update-patient/{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientRequest updatePatientRequest)
        {
            int userId = 0;

            try
            {
                userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
            
          
                var json = JsonConvert.SerializeObject(updatePatientRequest);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PatchAsync($"http://localhost:5298/odata/Patients/{id}?customerID={userId}", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Ok(result); // Return the updated Dentist details or success message
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"Failed to update Patients: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpPut("update-patient/{id}")]
        //public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientRequest updatePatientRequest)
        //{
        //    int userId = 0;

        //    try
        //    {
        //        userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //    var _updatedPatient = await _patientService.UpdatePatientAsync(id, updatePatientRequest, userId);
        //    return Ok(_updatedPatient);
        //}

        [HttpGet("get-patient-list-by-customer")]
        [Authorize(Roles = "Customer, Dentist")]
        public async Task<IActionResult> GetPatientListByCustomer()
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

            var response = await _patientService.GetPatientListByCustomer(userId);

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
