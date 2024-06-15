using Models;
using Models.DTOs;
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
        private readonly IRepositoryBase<Dentist> _dentistRepo;

        public ScheduleService(IRepositoryBase<Schedule> scheduleRepo,
                               IRepositoryBase<Dentist> dentistRepo)
        {
            _scheduleRepo = scheduleRepo;
            _dentistRepo = dentistRepo;
        }

        public async Task<List<Schedule>> GetAllSchedules()
        {
            return await _scheduleRepo.GetAllAsync();
        }

        public async Task<List<Schedule>> ViewClinicScheduleAsync()
        {
            return await _scheduleRepo.GetAllAsync();
        }

        public async Task<CreateScheduleResponse> CreateSchedule(CreateScheduleRequest request)
        {
            var dentist = await _dentistRepo.FindByIdAsync(request.DentistId);

            if (dentist == null)
            {
                return new CreateScheduleResponse { Success = false, ErrorMessage = "Unable to find dentist" };
            }

            if (request.DayOfWeekTimeSlots == null || !request.DayOfWeekTimeSlots.Any())
            {
                return new CreateScheduleResponse { Success = false, ErrorMessage = "Choose at least one day of the week" };
            }

            foreach (var slot in request.DayOfWeekTimeSlots)
            {
                if (!Enum.IsDefined(typeof(DayOfWeek), slot.DayOfWeek))
                {
                    return new CreateScheduleResponse { Success = false, ErrorMessage = $"Invalid DayOfWeek: {slot.DayOfWeek}" };
                }

                if (slot.TimeSlot <= 0)
                {
                    return new CreateScheduleResponse { Success = false, ErrorMessage = "TimeSlot must be a positive integer" };
                }
            }

            if (request.RepeatForWeeks == 0)
            {
                return new CreateScheduleResponse { Success = false, ErrorMessage = "Must be repeated for at least 1 week" };
            }

            var schedules = new List<Schedule>();
            var endDate = request.StartDate.AddDays(request.RepeatForWeeks * 7);

            for (var date = request.StartDate; date <= endDate; date = date.AddDays(1))
            {
                var dayOfWeekTimeSlot = request.DayOfWeekTimeSlots.FirstOrDefault(d => d.DayOfWeek == date.DayOfWeek);
                if (dayOfWeekTimeSlot != null)
                {
                    var schedule = new Schedule
                    {
                        WorkDate = date,
                        TimeSlot = dayOfWeekTimeSlot.TimeSlot,
                        Status = 1,
                        DentistId = request.DentistId
                    };

                    schedules.Add(schedule);
                }
            }

            try
            {
                foreach (var schedule in schedules)
                {
                    await _scheduleRepo.AddAsync(schedule);
                }

                return new CreateScheduleResponse { Success = true, Schedules = schedules };
            }
            catch (Exception ex)
            {
                return new CreateScheduleResponse { Success = false, ErrorMessage = $"Error creating schedules: {ex.Message}" };
            }
        }
    }
}
