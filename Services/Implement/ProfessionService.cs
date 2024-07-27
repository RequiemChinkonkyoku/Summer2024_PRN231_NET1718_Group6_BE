using Azure;
using Microsoft.AspNetCore.Mvc;
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
    public class ProfessionService : IProfessionService
    {
        private readonly IRepositoryBase<Profession> _profRepo;
        private readonly IRepositoryBase<Treatment> _treatRepo;
        private readonly IRepositoryBase<Dentist> _dentistRepo;

        public ProfessionService(IRepositoryBase<Profession> profRepo,
                                 IRepositoryBase<Treatment> treatRepo,
                                 IRepositoryBase<Dentist> dentistRepo)
        {
            _profRepo = profRepo;
            _treatRepo = treatRepo;
            _dentistRepo = dentistRepo;
        }

        public async Task<List<Profession>> GetAllProfessions()
        {
            var profList = await _profRepo.GetAllAsync();

            return profList;
        }

        public async Task<AddProfessionResponse> AddNewProfession(AddProfessionRequest request)
        {
            var response = new List<Profession>();

            if (request == null)
            {
                return new AddProfessionResponse { Success = false, ErrorMessage = "Request cannot be null" };
            }

            if (request.TreatmentId == null || request.TreatmentId == 0)
            {
                return new AddProfessionResponse { Success = false, ErrorMessage = "Invalid TreatmentId" };
            }

            var treatment = await _treatRepo.FindByIdAsync(request.TreatmentId);

            if (treatment == null)
            {
                return new AddProfessionResponse { Success = false, ErrorMessage = "Treatment does not exist" };
            }

            if (request.DentistId == null)
            {
                return new AddProfessionResponse { Success = false, ErrorMessage = "There are no dentistId given" };
            }

            var dentist = await _dentistRepo.FindByIdAsync(request.DentistId);

            if (dentist == null)
            {
                return new AddProfessionResponse { Success = false, ErrorMessage = "There are no dentist with id " + request.DentistId };
            }

            var profList = await _profRepo.GetAllAsync();
            var existingProf = profList.FirstOrDefault(p => p.TreatmentId == request.TreatmentId && p.DentistId == request.DentistId);

            if (existingProf == null)
            {
                try
                {
                    var profession = new Profession { TreatmentId = request.TreatmentId, DentistId = request.DentistId };

                    await _profRepo.AddAsync(profession);

                    response.Add(profession);
                }
                catch (Exception error)
                {
                    return new AddProfessionResponse { Success = false, ErrorMessage = "There has been an error " + error.Message };
                }

            }

            return new AddProfessionResponse { Success = true, Professions = response };
        }
    }
}
