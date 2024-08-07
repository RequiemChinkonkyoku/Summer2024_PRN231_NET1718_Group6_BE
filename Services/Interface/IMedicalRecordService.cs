﻿using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface  IMedicalRecordService
    {
        public Task<MedicalRecord> ViewMedicalRecord(int id);

        public Task<List<MedicalRecord>> GetAllMedicalRecordAsync();

        public Task<MedicalRecord> AddMedicalRecordAsync(AddMedicalRecordRequest addMedicalRecordRequest, int appointmentID);

        public Task<MedicalRecord> UpdateMedicalRecordAsync(int id, UpdateMedicalRecordRequest updateMedicalRecordRequest);

        public Task<MedicalRecord> GetMedicalRecordByAppId(int id);
    }
}
