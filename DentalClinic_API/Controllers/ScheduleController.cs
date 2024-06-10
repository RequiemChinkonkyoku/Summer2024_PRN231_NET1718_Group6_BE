using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Implement;
using Services.Interface;

namespace DentalClinic_API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("get-all-schedule")]
        public async Task<ActionResult<List<Schedule>>> GetAllSchedules()
        {
            return await _scheduleService.GetAllSchedules();
        }

        [HttpGet("get-clinic-schedule")]
        public async Task<ActionResult<List<Schedule>>> ViewClinicSchedule()
        {
            var schedules = await _scheduleService.ViewClinicScheduleAsync();
            return Ok(schedules);
        }
    }
}
