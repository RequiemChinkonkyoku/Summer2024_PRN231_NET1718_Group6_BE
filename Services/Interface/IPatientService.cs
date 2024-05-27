using Models;
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
        Task<Patient> PatientLogin(string email, string password);
    }
}
