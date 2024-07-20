using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IProfessionService
    {
        Task<AddProfessionResponse> AddNewProfession(AddProfessionRequest request);
        Task<List<Profession>> GetAllProfessions();
    }
}
