using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using Repositories.Implement;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class DentistService : IDentistService
    {
        private readonly IRepositoryBase<Dentist> _dentistRepo;
        private readonly IRepositoryBase<Schedule> _scheduleRepo;

        public DentistService(IRepositoryBase<Dentist> dentistRepository, IRepositoryBase<Schedule> scheduleRepository)
        {
            _dentistRepo = dentistRepository;
            _scheduleRepo = scheduleRepository;
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
        public async Task<Dentist> DentistLogin(string email, string password)
        {
            var dentists = await _dentistRepo.GetAllAsync();

            if (!dentists.IsNullOrEmpty())
            {
                var dentist = dentists.FirstOrDefault(p => p.Email.Equals(email) &&
                                                           p.Password.Equals(password));

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

    }
}
