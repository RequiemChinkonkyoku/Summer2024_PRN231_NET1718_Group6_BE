using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Services.Implement;
using Services.Interface;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Net.Http;


namespace DentalClinic_API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ScheduleController : Controller
    {
        private IScheduleService _scheduleService;
        private HttpClient _httpClient;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
            _httpClient = new HttpClient();
        }

        [HttpGet("get-all-schedule")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<List<Schedule>>> GetAllSchedules()
        {
            return await _scheduleService.GetAllSchedules();
        }

        [HttpGet("get-all-schedule-odata")]
        public async Task<IActionResult> GetAllSchedulesOData()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:44380/odata/Schedules");
            string json = await response.Content.ReadAsStringAsync();
            dynamic schedules = JsonConvert.DeserializeObject<OdataResponse<IEnumerable<Schedule>>>(json);
            return Ok(schedules);
        }

        [HttpGet("get-clinic-schedule")]
        public async Task<ActionResult<List<Schedule>>> ViewClinicSchedule()
        {
            var schedules = await _scheduleService.ViewClinicScheduleAsync();
            return Ok(schedules);
        }

        [HttpPost("create-schedule")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<CreateScheduleResponse>> CreateSchedule([FromBody] CreateScheduleRequest request)
        {
            var response = await _scheduleService.CreateSchedule(request);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.ErrorMessage);
            }
        }

        [HttpGet("get-schedule-by-id/{id}")]
        [Authorize(Roles = "Dentist")]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            var response = await _scheduleService.GetScheduleById(id);

            if (response.Any())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Hihi");
            }
        }

        [HttpGet("get-current-schedule")]
        [Authorize(Roles = "Dentist")]
        public async Task<IActionResult> GetCurrentSchedule()
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

            var response = await _scheduleService.GetScheduleById(userId);

            if (response.Any())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Hihi");
            }
        }

        [HttpGet("get-schedules-for-app")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetSchedulesForApp(int treatmentId)
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

            var response = await _scheduleService.GetSchedulesForApp(treatmentId);

            if (response.Any())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Failed");
            }
        }
    }
}
