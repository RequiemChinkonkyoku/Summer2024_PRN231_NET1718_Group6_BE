using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories.Interface;
using Services.Interface;

namespace Services.Implement
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private IRepositoryBase<MedicalRecord> _medicalRecordRepo;

        public MedicalRecordService (IRepositoryBase<MedicalRecord> medicalRecordRepo)
        {
            _medicalRecordRepo = medicalRecordRepo;
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
