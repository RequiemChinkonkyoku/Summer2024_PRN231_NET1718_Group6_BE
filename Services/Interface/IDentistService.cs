using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IDentistService
    {
        Task<Dentist> DentistLogin(string email, string password);
        Task<List<Dentist>> GetAllDentistAsync();
        Task<Dentist> GetDentistByID(int id);
    }
}
