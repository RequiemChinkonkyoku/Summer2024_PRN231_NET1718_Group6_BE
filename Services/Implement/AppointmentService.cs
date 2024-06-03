using Microsoft.AspNetCore.Http;
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
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepositoryBase<Appointment> _appRepo;
        private readonly IRepositoryBase<AppointmentDetail> _appDetailRepo;
        private readonly IRepositoryBase<Treatment> _treatmentRepo;
        private readonly IRepositoryBase<Patient> _patientRepo;
        private readonly IRepositoryBase<Dentist> _dentistRepo;

        public AppointmentService(IRepositoryBase<Appointment> appointmentRepo,
                                  IRepositoryBase<AppointmentDetail> appDetailRepo,
                                  IRepositoryBase<Treatment> treatmentRepo,
                                  IRepositoryBase<Patient> patientRepo,
                                  IRepositoryBase<Dentist> dentistRepo)
        {
            _appRepo = appointmentRepo;
            _appDetailRepo = appDetailRepo;
            _treatmentRepo = treatmentRepo;
            _patientRepo = patientRepo;
            _dentistRepo = dentistRepo;
        }

        public async Task<Appointment> CancelAppointment(int appID)
        {
            var appointment = await _appRepo.FindByIdAsync(appID);

            if (appointment != null)
            {
                appointment.Status = 0;
                await _appRepo.UpdateAsync(appointment);
                return appointment;
            }

            return null;
        }

        public async Task<CreateAppointmentResponse> CreateAppointment(CreateAppointmentRequest request, int accountID)
        {
            if (request.PatientId == 0 ||
                request.ArrivalDate == default ||
                request.TimeSlot == 0 ||
                request.BookingPrice < 0 ||
                request.ScheduleId == 0 ||
                request.TreatmentId == 0 ||
                request.DentistId == 0)
            {
                return new CreateAppointmentResponse { Success = false, ErrrorMessage = "All field must be filled in" };
            }

            var patient = await _patientRepo.FindByIdAsync(request.PatientId);

            if (patient == null)
            {
                return new CreateAppointmentResponse { Success = false, ErrrorMessage = "Patient does not exists" };
            }

            if (patient.CustomerId != accountID)
            {
                return new CreateAppointmentResponse { Success = false, ErrrorMessage = "The patient is not linked to the current account" };
            }

            var treatment = await _treatmentRepo.FindByIdAsync(request.TreatmentId);

            if (treatment == null)
            {
                return new CreateAppointmentResponse { Success = false, ErrrorMessage = "Treatment does not exists" };
            }

            var dentist = await _dentistRepo.FindByIdAsync(request.DentistId);

            if (dentist == null)
            {
                return new CreateAppointmentResponse { Success = false, ErrrorMessage = "Dentist does not exists" };
            }

            var appointment = new Appointment
            {
                CreateDate = DateTime.Now,
                ArrivalDate = request.ArrivalDate,
                TimeSlot = request.TimeSlot,
                Status = -1,
                BookingPrice = request.BookingPrice,
                ServicePrice = treatment.Price,
                CustomerId = accountID,
                PatientId = request.PatientId,
                TotalPrice = request.BookingPrice + treatment.Price,
            };

            try
            {
                await _appRepo.AddAsync(appointment);

                return new CreateAppointmentResponse { Success = true, Appointment = appointment };
            }
            catch (Exception ex)
            {
                return new CreateAppointmentResponse { Success = false, ErrrorMessage = $"Error adding appointment: {ex.Message}" };
            }
        }

        public async Task<List<Appointment>> GetAllAppointments()
        {
            return await _appRepo.GetAllAsync();
        }

        public async Task<List<Appointment>> GetCustomerAppointments(int customerID)
        {
            var appList = await _appRepo.GetAllAsync();

            var customerAppoinemts = appList.Where(app => app.CustomerId == customerID).ToList();

            if (!customerAppoinemts.IsNullOrEmpty())
            {
                return customerAppoinemts;
            }

            return null;
        }

        public async Task<List<Appointment>> GetDentistAppointments(int dentistID)
        {
            var appList = await _appRepo.GetAllAsync();
            var appDetailList = await _appDetailRepo.GetAllAsync();

            var dentistAppointments = (from app in appList
                                       join detail in appDetailList
                                       on app.AppointmentId equals detail.AppointmentId
                                       where detail.DentistId == dentistID
                                       select app).ToList();

            return dentistAppointments;
        }
    }
}
