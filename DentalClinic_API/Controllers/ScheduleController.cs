using Microsoft.AspNetCore.Mvc;
using Models;
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
    }
}
