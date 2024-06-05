using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface  ITreatmentService
    {
        Task<List<Treatment>> GetAllTreatment();
        Task<Treatment> AddTreatmentAsync(AddTreatmentRequest addTreatmentRequest);
        Task<Treatment> UpdateTreatmentAsync(int treatmentID, UpdateTreatmentRequest updateTreatmentRequest);
        Task<Treatment> DeleteTreatmentAsync(int id);
    }
}
