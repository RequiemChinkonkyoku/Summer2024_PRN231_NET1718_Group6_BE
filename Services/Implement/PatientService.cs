﻿using Microsoft.IdentityModel.Tokens;
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
                YearOfBirth = addPatientRequest.YearOfBirth,
                Address = addPatientRequest.Address,
                Gender = addPatientRequest.Gender,
                CustomerId = customerId
            };

            patient.Status = 1;
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
                updatepatient.YearOfBirth = updatePatientRequest.YearOfBirth;
                updatepatient.Address = updatePatientRequest.Address;
                updatepatient.Gender = updatePatientRequest.Gender;
                updatepatient.CustomerId = customerId;
            }
            else return null;

            await _patientRepo.UpdateAsync(updatepatient);
            return updatepatient;
        }

        public async Task<Patient> DeletePatient(int id)
        {
            var dentist = await _patientRepo.GetAllAsync();
            var deletepatient = dentist.FirstOrDefault(d => d.PatientId == id);
            if (dentist == null)
            {
                return null;
            }

            deletepatient.Status = 0;
            await _patientRepo.UpdateAsync(deletepatient);
            return deletepatient;
        }

        

        public async Task<List<Schedule>> ViewClinicScheduleAsync()
        {
            return await _scheduleRepo.GetAllAsync();
        }

        public async Task<List<Patient>> GetPatientListByCustomer(int customerId)
        {
            var list = await _patientRepo.GetAllAsync();
            var patientList = list.Where(p => p.CustomerId == customerId);
            List<Patient> response = new List<Patient>();

            foreach (var patient in patientList)
            {
                response.Add(new Patient
                {
                    PatientId = patient.PatientId,
                    Name = patient.Name,
                    YearOfBirth = patient.YearOfBirth,
                    Gender = patient.Gender,
                    Status = patient.Status,
                    Address = patient.Address,
                    CustomerId = patient.CustomerId,
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
