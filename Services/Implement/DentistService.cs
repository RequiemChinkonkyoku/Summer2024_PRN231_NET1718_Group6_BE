using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using Repositories.Implement;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class DentistService : IDentistService
    {
        private readonly IRepositoryBase<Dentist> _dentistRepo;
        private readonly IRepositoryBase<Schedule> _scheduleRepo;
        private readonly IRepositoryBase<Profession> _professionRepo;
        private readonly IRepositoryBase<Treatment> _treatmentRepo;
        private readonly IRepositoryBase<Appointment> _appointmentRepo;
        private readonly IRepositoryBase<AppointmentDetail> _appointmentDetailRepo;
        private readonly IRepositoryBase<Profession> _profRepo;

        public DentistService(IRepositoryBase<Dentist> dentistRepository, IRepositoryBase<Schedule> scheduleRepository, IRepositoryBase<Profession> professionRepo, IRepositoryBase<Treatment> treatmentRepo, IRepositoryBase<Appointment> appointmentRepo, IRepositoryBase<AppointmentDetail> appointmentDetailRepo, IRepositoryBase<Profession> profRepo)
        {
            _dentistRepo = dentistRepository;
            _scheduleRepo = scheduleRepository;
            _professionRepo = professionRepo;
            _treatmentRepo = treatmentRepo;
            _appointmentRepo = appointmentRepo;
            _appointmentDetailRepo = appointmentDetailRepo;
            _profRepo = profRepo;
        }

        public async Task<List<Dentist>> GetAllDentistAsync()
        {
            return await _dentistRepo.GetAllAsync();
        }

        public async Task<Dentist> GetDentistByID(int id)
        {
            var dentists = await _dentistRepo.GetAllAsync();

            if (!dentists.IsNullOrEmpty())
            {
                var dentist = dentists.FirstOrDefault(d => d.DentistId == id);

                if (dentist != null)
                {
                    return dentist;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<Dentist> DentistAdd(AddDentistRequest addDentistRequest)
        {
            var dentist = new Dentist();

            dentist.Name = addDentistRequest.Name;
            dentist.Email = addDentistRequest.Email;
            dentist.Password = addDentistRequest.Password;
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(addDentistRequest.Password);
            dentist.PasswordHash = passwordHash;
            dentist.Type = addDentistRequest.Type;
            dentist.ContractType = addDentistRequest.ContractType;
            dentist.Status = 1;

            await _dentistRepo.AddAsync(dentist);
            return dentist;
        }

        public async Task<Dentist> UpdateDentist(int id, UpdateDentistRequest updateDentistRequest)
        {
            var dentist = await _dentistRepo.GetAllAsync();
            var updatedentist = dentist.FirstOrDefault(d => d.DentistId == id);

            if (dentist == null)
            {
                return null;
            }

            updatedentist.Name = updateDentistRequest.Name;
            updatedentist.Email = updateDentistRequest.Email;
            updatedentist.Password = updateDentistRequest.Password;
            updatedentist.Type = updateDentistRequest.Type;
            updatedentist.ContractType = updateDentistRequest.ContractType;
            updatedentist.Status = updateDentistRequest.Status;

            await _dentistRepo.UpdateAsync(updatedentist);
            return updatedentist;
        }

        public async Task<Dentist> DeleteDentist(int id)
        {
            var dentist = await _dentistRepo.GetAllAsync();
            var deletedentist = dentist.FirstOrDefault(d => d.DentistId == id);
            if (dentist == null)
            {
                return null;
            }

            deletedentist.Status = 0;
            await _dentistRepo.UpdateAsync(deletedentist);
            return deletedentist;
        }

        public async Task<List<Schedule>> ViewSchedule(int id)
        {
            var schedules = await _scheduleRepo.GetAllAsync();

            if (!schedules.IsNullOrEmpty())
            {
                var dentistSchedule = schedules.Where(sch => sch.DentistId == id).ToList();

                if (dentistSchedule != null)
                {
                    return dentistSchedule;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ProfessionDetail>> ViewProfession(int id)
        {
            var professions = await _professionRepo.GetAllAsync();
            if (!professions.IsNullOrEmpty())
            {
                var result = from p in professions
                             join d in await _dentistRepo.GetAllAsync() on p.DentistId equals d.DentistId
                             join t in await _treatmentRepo.GetAllAsync() on p.TreatmentId equals t.TreatmentId
                             where d.DentistId == id
                             select new ProfessionDetail
                             {
                                 ProfessionId = p.ProfessionId,
                                 DentistId = d.DentistId,
                                 DentistName = d.Name,
                                 DentistEmail = d.Email,
                                 TreatmentName = t.Name,
                                 TreatmentDescription = t.Description,
                                 TreatmentPrice = t.Price
                             };
                return result.ToList();
            }
            else
            {
                return null;
            }
        }
        public async Task<Dentist> UpdateDentistAccount(int id, UpdateDentistAccountRequest updateDentistAccountRequest)
        {
            var dentist = await _dentistRepo.GetAllAsync();
            var updateDentistAccount = dentist.FirstOrDefault(d => d.DentistId == id);

            if (dentist == null)
            {
                return null;
            }

            updateDentistAccount.Email = updateDentistAccountRequest.Email;
            updateDentistAccount.Password = updateDentistAccountRequest.Password;

            await _dentistRepo.UpdateAsync(updateDentistAccount);
            return updateDentistAccount;
        }
        public async Task<Dentist> DeleteDentistAccount(int id)
        {
            var dentist = await _dentistRepo.GetAllAsync();
            var deleteDentistAccount = dentist.FirstOrDefault(d => d.DentistId == id);
            if (dentist == null)
            {
                return null;
            }

            deleteDentistAccount.Status = 0;
            await _dentistRepo.UpdateAsync(deleteDentistAccount);
            return deleteDentistAccount;
        }

        public async Task<List<GetDentistsForAppResponse>> GetDentistsForApp(GetDentistsForAppRequest request)
        {
            int treatmentId = request.TreatmentId;
            DateTime date = request.Date;
            int timeSlot = request.TimeSlot;

            // Fetch all professionals asynchronously
            var allProfs = await _profRepo.GetAllAsync();

            var allDentists = await _dentistRepo.GetAllAsync();

            // Filter professionals by treatmentId
            var profList = allProfs.Where(p => p.TreatmentId == treatmentId).ToList();

            if (profList.Any())
            {
                var dentList = new List<Dentist>();
                foreach (var prof in profList)
                {
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
                    if (scheduleList.Any())
                    {
                        var filteredScheduleList = new List<Schedule>();
                        foreach (var schedule in scheduleList)
                        {
                            if (DateTime.Compare(schedule.WorkDate, date) == 0 && schedule.TimeSlot == timeSlot)
                            {
                                filteredScheduleList.Add(schedule);
                            }
                        }
                        if (filteredScheduleList.Any())
                        {
                            var filteredDentList = new List<Dentist>();
                            var response = new List<GetDentistsForAppResponse>();
                            foreach (Schedule schedule in filteredScheduleList)
                            {
                                var dentist = allDentists.FirstOrDefault(d => d.DentistId == schedule.DentistId);
                                filteredDentList.Add(dentist);
                                var result = new GetDentistsForAppResponse();
                                result.DentistId = dentist.DentistId;
                                result.DentistName = dentist.Name;
                                result.ScheduleId = schedule.ScheduleId;
                                response.Add(result);
                            }
                            return response;
                        }
                    }
                }
            }

            return null;
        }
    }
}
