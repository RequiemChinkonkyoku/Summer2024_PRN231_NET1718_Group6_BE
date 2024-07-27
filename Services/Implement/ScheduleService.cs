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
        private readonly IRepositoryBase<Profession> _profRepo;

        public ScheduleService(IRepositoryBase<Schedule> scheduleRepo,
                               IRepositoryBase<Dentist> dentistRepo,
                               IRepositoryBase<Profession> profRepo)
        {
            _scheduleRepo = scheduleRepo;
            _dentistRepo = dentistRepo;
            _profRepo = profRepo;
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

            var scheduleList = await _scheduleRepo.GetAllAsync();
            var dentistSchedule = scheduleList.Where(s => s.DentistId == request.DentistId);
            var schedules = new List<Schedule>();
            var endDate = request.StartDate.AddDays(request.RepeatForWeeks * 7);

            for (var date = request.StartDate; date <= endDate; date = date.AddDays(1))
            {
                var dayOfWeekTimeSlot = request.DayOfWeekTimeSlots.Where(d => d.DayOfWeek == date.DayOfWeek).ToList();

                foreach (var timeSlot in dayOfWeekTimeSlot)
                {
                    var existingSchedule = dentistSchedule.FirstOrDefault(s => s.WorkDate == date.Date && s.TimeSlot == timeSlot.TimeSlot);

                    if (existingSchedule == null)
                    {
                        var schedule = new Schedule
                        {
                            WorkDate = date,
                            TimeSlot = timeSlot.TimeSlot,
                            Status = 1,
                            DentistId = request.DentistId
                        };

                        schedules.Add(schedule);
                    }
                }
            }

            try
            {
                if (schedules.Count > 0)
                {
                    foreach (var schedule in schedules)
                    {
                        await _scheduleRepo.AddAsync(schedule);
                    }
                }

                return new CreateScheduleResponse { Success = true, Schedules = schedules };
            }
            catch (Exception ex)
            {
                return new CreateScheduleResponse { Success = false, ErrorMessage = $"Error creating schedules: {ex.Message}" };
            }
        }

        public async Task<List<Schedule>> GetScheduleById(int id)
        {
            var scheduleList = _scheduleRepo.GetAllAsync().Result.Where(s => s.DentistId == id);

            if (scheduleList.Any())
            {
                return scheduleList.ToList();
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Schedule>> GetSchedulesForApp(int treatmentId)
        {
            // Fetch all professionals asynchronously
            var allProfs = await _profRepo.GetAllAsync();

            // Filter professionals by treatmentId
            var profList = allProfs.Where(p => p.TreatmentId == treatmentId).ToList();

            if (profList.Any())
            {
                var dentList = new List<Dentist>();
                foreach (var prof in profList)
                {
                    // Fetch all dentists asynchronously
                    var allDentists = await _dentistRepo.GetAllAsync();

                    // Filter dentists by DentistId
                    var dentist = allDentists.FirstOrDefault(d => d.DentistId == prof.DentistId);
                    if (dentist != null)
                    {
                        dentList.Add(dentist);
                    }
                }
                if (dentList.Any())
                {
                    var allSchedules = await _scheduleRepo.GetAllAsync();
                    var filteredSchedules = allSchedules.Where(s => s.WorkDate > DateTime.Now
                                                               && s.Status == 1);
                    var scheduleList = new List<Schedule>();
                    foreach (var schedule in filteredSchedules)
                    {
                        foreach (var dentist in dentList)
                        {
                            if (schedule.DentistId.Equals(dentist.DentistId))
                            {
                                scheduleList.Add(schedule);
                            }
                        }
                    }
                    return scheduleList;
                }
            }

            return null;
        }

    }
}
