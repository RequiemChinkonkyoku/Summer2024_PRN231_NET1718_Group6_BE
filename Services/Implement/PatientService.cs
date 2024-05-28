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
    public class PatientService : IPatientService
    {
        private IRepositoryBase<Patient> _patientRepo;

        public PatientService(IRepositoryBase<Patient> patientRepository)
        {
            _patientRepo = patientRepository;
        }

        public async Task<List<Patient>> GetAllPatient()
        {
            return await _patientRepo.GetAllAsync();
        }

        public async Task<Patient> GetPatientByID(int id)
        {
            var patients = await _patientRepo.GetAllAsync();

            if (!patients.IsNullOrEmpty())
            {
                var patient = patients.FirstOrDefault(p => p.PatientId == id);

                if (patient != null)
                {
                    return patient;
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
