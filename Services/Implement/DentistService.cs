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

        public DentistService(IRepositoryBase<Dentist> dentistRepository)
        {
            _dentistRepo = dentistRepository;
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

        //public async Task<Dentist> UpdateDentist(UpdateDentistRequest updateDentistRequest)
        //{
        //    var dentist = await _dentistRepo.FindByIdAsync(updateDentistRequest.DentistId);
        //    if (dentist == null)
        //    {
        //        return null; // Dentist not found
        //    }

        //    dentist.Name = updateDentistRequest.Name;
        //    dentist.Email = updateDentistRequest.Email;
        //    dentist.Password = updateDentistRequest.Password;
        //    dentist.Type = updateDentistRequest.Type;
        //    dentist.ContractType = updateDentistRequest.ContractType;
        //    dentist.Status = updateDentistRequest.Status;

        //    await _dentistRepo.UpdateAsync(dentist);
        //    return dentist;
        //}
    }
}
