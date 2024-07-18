using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IRepositoryBase<MedicalRecord> _medicalRecordRepo;
        private readonly IRepositoryBase<Appointment> _appointmentRepo;

        public MedicalRecordService(IRepositoryBase<MedicalRecord> medicalRecordRepo, IRepositoryBase<Appointment> appointmentRepo)
        {
            _medicalRecordRepo = medicalRecordRepo;
            _appointmentRepo = appointmentRepo;

        }

        public async Task<List<MedicalRecord>> GetAllMedicalRecordAsync()
        {
            return await _medicalRecordRepo.GetAllAsync();
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

        public async Task<MedicalRecord> GetMedicalRecordByAppId(int id)
        {
            var medicalRecords = await _medicalRecordRepo.GetAllAsync();
            if (!medicalRecords.IsNullOrEmpty())
            {
                var medicalRecord = medicalRecords.FirstOrDefault(m => m.AppointmentId == id);

                if (medicalRecord != null)
                {
                    return medicalRecord;
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

        public async Task<MedicalRecord> AddMedicalRecordAsync(AddMedicalRecordRequest addMedicalRecordRequest, int appointmentID)
        {
            if (addMedicalRecordRequest == null)
            {
                throw new ArgumentNullException(nameof(addMedicalRecordRequest));
            }

            var appointment = await _appointmentRepo.FindByIdAsync(appointmentID);
            if (appointment == null)
            {
                throw new ArgumentException("Invalid appointment ID.");
            }

            var medicalRecord = new MedicalRecord
            {
                Diagnosis = addMedicalRecordRequest.Diagnosis,
                Note = addMedicalRecordRequest.Note,
                Status = 0,
                AppointmentId = appointmentID,
                PatientId = appointment.PatientId
            };

            await _medicalRecordRepo.AddAsync(medicalRecord);
            return medicalRecord;
        }

        public async Task<MedicalRecord> UpdateMedicalRecordAsync(int id, UpdateMedicalRecordRequest updateMedicalRecordRequest)
        {
            var medicalRecord = await _medicalRecordRepo.GetAllAsync();
            var updatedmedicalRecord = medicalRecord.FirstOrDefault(d => d.RecordId == id);

            if (medicalRecord == null)
            {
                return null;
            }

            if (updatedmedicalRecord == null)
            {
                throw new ArgumentException("Invalid record ID.");
            }
            if (updatedmedicalRecord.Status == 1)
            {
                throw new ArgumentException("You cannot change MedicalRecord Status when it's complete");

            }


            updatedmedicalRecord.Diagnosis = updateMedicalRecordRequest.Diagnosis;
            updatedmedicalRecord.Note = updateMedicalRecordRequest.Note;

            await _medicalRecordRepo.UpdateAsync(updatedmedicalRecord);
            return updatedmedicalRecord;
        }
    }
}
