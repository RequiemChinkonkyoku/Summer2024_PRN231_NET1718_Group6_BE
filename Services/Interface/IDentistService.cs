﻿using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IDentistService
    {
        Task<List<Dentist>> GetAllDentistAsync();
        Task<Dentist> GetDentistByID(int id);
        Task<Dentist> DentistAdd(AddDentistRequest addDentistRequest);
        Task<Dentist> UpdateDentist(int id,UpdateDentistRequest updateDentistRequest);
        Task<Dentist> DeleteDentist(int id);
        Task<List<Schedule>> ViewSchedule(int id);
        Task<List<ProfessionDetail>> ViewProfession(int id);
        Task<Dentist> UpdateDentistAccount(int id, UpdateDentistAccountRequest updateDentistAccountRequest);
        Task<Dentist> DeleteDentistAccount(int id);
        Task<List<GetDentistsForAppResponse>> GetDentistsForApp(GetDentistsForAppRequest request);
        Task<List<Dentist>> GetDentistWithTreatment(int treatmentId);
    }
}
