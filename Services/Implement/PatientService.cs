using Models;
using Repositories.Implement;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class PatientService : Interface.IPatientService
    {
        private PatientRepository _patientRepo;

        public PatientService(PatientRepository patientRepository)
        {
            _patientRepo = patientRepository;
        }

        public async Task<List<Patient>> GetAllPatient()
        {
            return await _patientRepo.GetAllAsync();
        }
    }
}
