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

            var dentistList = request.DentistIds;

            if (dentistList.IsNullOrEmpty())
            {
                return new AddProfessionResponse { Success = false, ErrorMessage = "There are no dentistId given" };
            }

            foreach (var dentistId in dentistList)
            {
                var dentist = await _dentistRepo.FindByIdAsync(dentistId);

                if (dentist == null)
                {
                    return new AddProfessionResponse { Success = false, ErrorMessage = "There are no dentist with id " + dentistId };
                }

                var profession = new Profession { TreatmentId = request.TreatmentId, DentistId = dentistId };

                try
                {
                    await _profRepo.AddAsync(profession);
                }
                catch (Exception error)
                {
                    return new AddProfessionResponse { Success = false, ErrorMessage = "There has been an error " + error.Message };
                }

                response.Add(profession); ;
            }

            return new AddProfessionResponse { Success = true, Professions = response };
        }
    }
}
