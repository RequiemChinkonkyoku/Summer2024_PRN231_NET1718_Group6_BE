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
    public class TreatmentService : ITreatmentService
    {
        private IRepositoryBase<Treatment> _treatmentRepo;

        public TreatmentService(IRepositoryBase<Treatment> treatmentRepository)
        {
            _treatmentRepo = treatmentRepository;
        }

        public async Task<List<Treatment>> GetAllTreatment()
        {
            return await _treatmentRepo.GetAllAsync();
        }

        public async Task<Treatment> GetTreatmentByID(int id)
        {
            var treatments = await _treatmentRepo.GetAllAsync();

            if (!treatments.IsNullOrEmpty())
            {
                var treatment = treatments.FirstOrDefault(p => p.TreatmentId == id);

                if (treatment != null)
                {
                    return treatment;
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
        public async Task<Treatment> AddTreatmentAsync(AddTreatmentRequest addTreatmentRequest)
        {
            var treatment = new Treatment()
            {
                Name = addTreatmentRequest.Name,
                Price = addTreatmentRequest.Price,
                Description = addTreatmentRequest.Description,
                Status = 1,
            };
            await _treatmentRepo.AddAsync(treatment);
            return treatment;
        }

        public async Task<Treatment> UpdateTreatmentAsync(int treatmentID, UpdateTreatmentRequest updateTreatmentRequest)
        {
            var treatments = await _treatmentRepo.GetAllAsync();
            var updateTreatment = treatments.FirstOrDefault(d => d.TreatmentId == treatmentID);

            if (updateTreatment != null)
            {
                if (updateTreatment.TreatmentId != treatmentID)
                {
                    throw new Exception("You do not have permission to update this patient");
                }
                updateTreatment.Name = updateTreatmentRequest.Name;
                updateTreatment.Price = updateTreatmentRequest.Price;
                updateTreatment.Description = updateTreatmentRequest.Description;
                updateTreatment.Status = updateTreatmentRequest.Status;
            }
            else return null;

            await _treatmentRepo.UpdateAsync(updateTreatment);
            return updateTreatment;
        }

        public async Task<Treatment> DeleteTreatmentAsync(int id)
        {
            var treatment = await _treatmentRepo.GetAllAsync();
            var deleteTreatment = treatment.FirstOrDefault(d => d.TreatmentId == id);
            if (treatment == null)
            {
                return null;
            }

            deleteTreatment.Status = 0;
            await _treatmentRepo.UpdateAsync(deleteTreatment);
            return deleteTreatment;
        }
    }
}
