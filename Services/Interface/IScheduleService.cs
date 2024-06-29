using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IScheduleService
    {
        Task<List<Schedule>> GetAllSchedules();
        Task<List<Schedule>> ViewClinicScheduleAsync();
        Task<CreateScheduleResponse> CreateSchedule(CreateScheduleRequest request);
        Task<List<Schedule>> GetScheduleById(int id);
        Task<List<Schedule>> GetSchedulesForApp(int treatmentId);
    }
}
