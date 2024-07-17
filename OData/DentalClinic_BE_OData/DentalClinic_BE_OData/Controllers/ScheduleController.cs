using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Models;
using Models.DTOs;
using Services.Interface;

namespace DentalClinic_BE_OData.Controllers
{
    public class SchedulesController : ODataController
    {
        private readonly IScheduleService _scheduleService;
        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var schedules = await _scheduleService.GetAllSchedules();
            return Ok(schedules);
        }

        [EnableQuery]
        public async Task<ActionResult<Schedule>> Get(int key)
        { 
            var schedule = await _scheduleService.GetScheduleById(key);
            if (schedule != null) 
            {
                return Ok(schedule);
            }
            else
            {
                return NotFound();
            }
        }
    }
}