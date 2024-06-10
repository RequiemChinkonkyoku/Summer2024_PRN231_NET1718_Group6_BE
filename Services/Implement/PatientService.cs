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
        private readonly IRepositoryBase<Schedule> _scheduleRepo;

        public PatientService(IRepositoryBase<Patient> patientRepository, IRepositoryBase<MedicalRecord> medicalRecordRepo, IRepositoryBase<Schedule> scheduleRepo)
        {
            _patientRepo = patientRepository;
            _medicalRecordRepo = medicalRecordRepo;
            _scheduleRepo = scheduleRepo;
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

        public async Task<Patient> AddPatientAsync(AddPatientRequest addPatientRequest, int customerId)
        {
            var patient = new Patient()
            {
                Name = addPatientRequest.Name,
                Age = addPatientRequest.Age,
                Address = addPatientRequest.Address,
                Gender = addPatientRequest.Gender,
                CustomerId = customerId
            };

            await _patientRepo.AddAsync(patient);
            return patient;
        }

        public async Task<Patient> UpdatePatientAsync(int patientId, UpdatePatientRequest updatePatientRequest, int customerId)
        {
            var patient = await _patientRepo.GetAllAsync();
            var updatepatient = patient.FirstOrDefault(d => d.PatientId == patientId);

            if (updatepatient != null)
            {
                if (updatepatient.CustomerId != customerId)
                {
                    throw new Exception("You do not have permission to update this patient");
                }
                updatepatient.Name = updatePatientRequest.Name;
                updatepatient.Age = updatePatientRequest.Age;
                updatepatient.Address = updatePatientRequest.Address;
                updatepatient.Gender = updatePatientRequest.Gender;
                updatepatient.CustomerId = customerId;
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

        public async Task<List<GetPatientListResponse>> GetPatientListByCustomer(int customerId)
        {
            var list = await _patientRepo.GetAllAsync();
            var patientList = list.Where(p => p.CustomerId == customerId);
            List<GetPatientListResponse> response = new List<GetPatientListResponse>();

            foreach (var patient in patientList)
            {
                response.Add(new GetPatientListResponse
                {
                    PatientId = patient.PatientId,
                    Name = patient.Name,
                });
            }

            if (response.Any())
            {
                return response;
            }

            return null;
        }
    }
}
