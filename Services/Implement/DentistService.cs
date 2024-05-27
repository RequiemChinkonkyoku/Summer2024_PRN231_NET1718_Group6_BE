using Microsoft.IdentityModel.Tokens;
using Models;
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
    }
}
