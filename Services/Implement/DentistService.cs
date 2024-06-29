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
        public DentistService(IRepositoryBase<Dentist> dentistRepository, IRepositoryBase<Schedule> scheduleRepository, IRepositoryBase<Profession> professionRepo, IRepositoryBase<Treatment> treatmentRepo, IRepositoryBase<Appointment> appointmentRepo, IRepositoryBase<AppointmentDetail> appointmentDetailRepo)
        {
            _dentistRepo = dentistRepository;
            _scheduleRepo = scheduleRepository;
            _professionRepo = professionRepo;
            _treatmentRepo = treatmentRepo;
            _appointmentRepo = appointmentRepo;
            _appointmentDetailRepo = appointmentDetailRepo;
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
            var dentist = new Dentist()
            {
                Name = addDentistRequest.Name,
                Email = addDentistRequest.Email,
                Password = addDentistRequest.Password,
                Type = addDentistRequest.Type,
                ContractType = addDentistRequest.ContractType,
            };
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
        public async Task<Schedule> ViewSchedule(int id) 
        {
            var schedules = await _scheduleRepo.GetAllAsync();
            if (!schedules.IsNullOrEmpty())
            {
                var schedule = schedules.FirstOrDefault(s => s.DentistId == id);

                if (schedule != null)
                {
                    return schedule;
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

        public async Task<List<DentistAppointment>> ViewDentistAppointment(int id)
        {
            var appointments = await _appointmentRepo.GetAllAsync();
            var appointmentDetails = await _appointmentDetailRepo.GetAllAsync();
            if (appointments != null && appointments.Any() && appointmentDetails != null && appointmentDetails.Any())
            {
                var result = from a in appointments
                             join ad in appointmentDetails on a.AppointmentId equals ad.AppointmentId
                             where ad.DentistId == id
                             select new DentistAppointment
                             {
                                AppointmentId = a.AppointmentId,
                                 CreateDate = a.CreateDate,
                                 ArrivalDate = a.ArrivalDate,
                                 TimeSlot = a.TimeSlot,
                                 Status = a.Status,
                                 CustomerId = a.CustomerId,
                                 PatientId = a.PatientId,
                                 TreatmentId = ad.TreatmentId,
                                 DentistId = ad.DentistId,
                                 ScheduleId = ad.ScheduleId,
                             };
                return result.ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
