using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IPatientService
    {
        Task<List<Patient>> GetAllPatient();
        Task<Patient> GetPatientByID(int id);
        Task<Patient> AddPatientAsync(AddPatientRequest addPatientRequest, int accountId);
        Task<Patient> UpdatePatientAsync(int patientId, UpdatePatientRequest updatePatientRequest, int accountId);
        Task<MedicalRecord> ViewMedicalRecord(int id);
        Task<List<Schedule>> ViewClinicScheduleAsync();
        Task<List<GetPatientListResponse>> GetPatientListByCustomer(int customerId);
    }
}
