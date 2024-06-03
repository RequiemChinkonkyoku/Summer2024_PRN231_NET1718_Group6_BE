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
    public class PatientService : IPatientService
    {
        private IRepositoryBase<Patient> _patientRepo;
        private IRepositoryBase<MedicalRecord> _medicalRecordRepo;

        public PatientService(IRepositoryBase<Patient> patientRepository, IRepositoryBase<MedicalRecord> medicalRecordRepo)
        {
            _patientRepo = patientRepository;
            _medicalRecordRepo = medicalRecordRepo;
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

        public async Task<Patient> AddPatientAsync(AddPatientRequest addPatientRequest, int accountId)
        {
            var patient = new Patient()
            {
                Name = addPatientRequest.Name,
                Age = addPatientRequest.Age,
                Address = addPatientRequest.Address,
                Gender = addPatientRequest.Gender,
                CustomerId = accountId
            };

            await _patientRepo.AddAsync(patient);
            return patient;
        }

        public async Task<Patient> UpdatePatientAsync(int patientId, UpdatePatientRequest updatePatientRequest, int accountId)
        {
            var patient = await _patientRepo.GetAllAsync();
            var updatepatient = patient.FirstOrDefault(d => d.PatientId == patientId);

            if (updatepatient != null)
            {
                if (updatepatient.CustomerId != accountId)
                {
                    throw new Exception("You do not have permission to update this patient");
                }
                updatepatient.Name = updatePatientRequest.Name;
                updatepatient.Age = updatePatientRequest.Age;
                updatepatient.Address = updatePatientRequest.Address;
                updatepatient.Gender = updatePatientRequest.Gender;
                updatepatient.CustomerId = accountId;
            }
            else return null;

            await _patientRepo.UpdateAsync(updatepatient);
            return updatepatient;
        }

        public async Task<MedicalRecord> ViewMedicalRecord(int id)
        {
            var medicalrecords = await _medicalRecordRepo.GetAllAsync();
            if (!medicalrecords.IsNullOrEmpty())
            {
                var medicalrecord = medicalrecords.FirstOrDefault(m => m.PatientId == id);

                if (medicalrecord != null)
                {
                    return medicalrecord;
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
