using Models;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class ScheduleService : IScheduleService
    {
        private readonly IRepositoryBase<Schedule> _scheduleRepo;

        public ScheduleService(IRepositoryBase<Schedule> scheduleRepo)
        {
            _scheduleRepo = scheduleRepo;
        }

        public async Task<List<Schedule>> GetAllSchedules()
        {
            return await _scheduleRepo.GetAllAsync();
        }
    }
}
