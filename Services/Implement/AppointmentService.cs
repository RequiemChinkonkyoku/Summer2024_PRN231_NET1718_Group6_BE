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
        private readonly IRepositoryBase<Schedule> _scheduleRepo;
        private readonly IRepositoryBase<Profession> _professionRepo;
        private readonly IRepositoryBase<Customer> _cusRepo;

        public AppointmentService(IRepositoryBase<Appointment> appointmentRepo,
                                  IRepositoryBase<AppointmentDetail> appDetailRepo,
                                  IRepositoryBase<Treatment> treatmentRepo,
                                  IRepositoryBase<Patient> patientRepo,
                                  IRepositoryBase<Dentist> dentistRepo,
                                  IRepositoryBase<Profession> professionRepo,
                                  IRepositoryBase<Schedule> scheduleRepo,
                                  IRepositoryBase<Customer> cusRepo)
        {
            _appRepo = appointmentRepo;
            _appDetailRepo = appDetailRepo;
            _treatmentRepo = treatmentRepo;
            _patientRepo = patientRepo;
            _dentistRepo = dentistRepo;
            _professionRepo = professionRepo;
            _scheduleRepo = scheduleRepo;
            _cusRepo = cusRepo;
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
                request.ScheduleId == 0 ||
                request.TreatmentId == 0 ||
                request.DentistId == 0)
            {
                return new CreateAppointmentResponse { Success = false, ErrrorMessage = "All field must be filled in" };
            }

            var patient = await _patientRepo.FindByIdAsync(request.PatientId);

            if (patient == null)
            {
                return new CreateAppointmentResponse { Success = false, ErrrorMessage = "Unable to find patient" };
            }
            else
            {
                if (patient.CustomerId != accountID)
                {
                    return new CreateAppointmentResponse { Success = false, ErrrorMessage = "The patient is not linked to this account" };
                }
            }

            var treatment = await _treatmentRepo.FindByIdAsync(request.TreatmentId);

            if (treatment == null)
            {
                return new CreateAppointmentResponse { Success = false, ErrrorMessage = "Unable to find treatment" };
            }

            var dentist = await _dentistRepo.FindByIdAsync(request.DentistId);

            if (dentist == null)
            {
                return new CreateAppointmentResponse { Success = false, ErrrorMessage = "Unable to find dentist" };
            }
            else
            {
                var profession = _professionRepo.GetAllAsync().Result
                                                .Where(p => p.DentistId == dentist.DentistId)
                                                .FirstOrDefault(p => p.TreatmentId == treatment.TreatmentId);

                if (profession == null)
                {
                    return new CreateAppointmentResponse { Success = false, ErrrorMessage = "The dentist doesnt provide that treatment" };
                }
            }

            var schedule = await _scheduleRepo.FindByIdAsync(request.ScheduleId);

            if (schedule == null)
            {
                return new CreateAppointmentResponse { Success = false, ErrrorMessage = "Unable to find schedule" };
            }
            else
            {
                if (schedule.DentistId != dentist.DentistId)
                {
                    return new CreateAppointmentResponse { Success = false, ErrrorMessage = "The dentist doesnt work at that time" };
                }
            }

            var appointment = new Appointment
            {
                CreateDate = DateTime.Now,
                ArrivalDate = schedule.WorkDate,
                TimeSlot = schedule.TimeSlot,
                Status = -1,
                BookingPrice = 50000,
                ServicePrice = treatment.Price,
                CustomerId = accountID,
                PatientId = request.PatientId,
                TotalPrice = 50000 + treatment.Price,
            };

            var appointmentDetail = new AppointmentDetail
            {
                TreatmentId = request.TreatmentId,
                DentistId = request.DentistId,
                ScheduleId = request.ScheduleId
            };

            try
            {
                await _appRepo.AddAsync(appointment);
                appointmentDetail.AppointmentId = appointment.AppointmentId;
                await _appDetailRepo.AddAsync(appointmentDetail);
                appointment.AppointmentDetails.Add(appointmentDetail);

                return new CreateAppointmentResponse { Success = true, Appointment = appointment };
            }
            catch (Exception ex)
            {
                return new CreateAppointmentResponse { Success = false, ErrrorMessage = $"Error creating appointment: {ex.Message}" };
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
                foreach (var app in customerAppoinemts)
                {
                    var patient = await _patientRepo.FindByIdAsync(app.PatientId.Value);

                    app.Patient = patient;
                }

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

            foreach (var app in dentistAppointments)
            {
                var patient = await _patientRepo.FindByIdAsync(app.PatientId.Value);

                app.Patient = patient;
            }

            return dentistAppointments;
        }

        public async Task<List<Appointment>> GetCurrentAppointmentList(int id)
        {
            var appList = await _appRepo.GetAllAsync();
            var patList = await _patientRepo.GetAllAsync();
            List<Appointment> result = new List<Appointment>();

            foreach (var app in appList)
            {
                if (app.CustomerId == id)
                {
                    Appointment appointment = new Appointment();
                    appointment = app;
                    appointment.Patient = patList.FirstOrDefault(p => p.CustomerId == id);
                    result.Add(appointment);
                }
            }

            if (result.Any())
            {
                return result;
            }
            return null;
        }

        public async Task<UpdateAppointmentResponse> UpdateAppointment(UpdateAppointmentRequest request)
        {
            if (request.AppointmentId == 0 ||
                request.PatientId == 0 ||
                request.ArrivalDate == default ||
                request.TreatmentId == 0 ||
                request.DentistId == 0 ||
                request.ScheduleId == 0)
            {
                return new UpdateAppointmentResponse { Success = false, ErrrorMessage = "All field must be filled in" };
            }

            var appointment = await _appRepo.FindByIdAsync(request.AppointmentId.Value);

            if (appointment == null)
            {
                return new UpdateAppointmentResponse { Success = false, ErrrorMessage = "Unable to find appointment" };
            }

            var patient = await _patientRepo.FindByIdAsync(request.PatientId.Value);

            if (patient == null)
            {
                return new UpdateAppointmentResponse { Success = false, ErrrorMessage = "Unable to find patient" };
            }
            else
            {
                if (patient.CustomerId != appointment.CustomerId)
                {
                    return new UpdateAppointmentResponse { Success = false, ErrrorMessage = "The patient is not linked to this account" };
                }
            }

            var treatment = await _treatmentRepo.FindByIdAsync(request.TreatmentId.Value);

            if (treatment == null)
            {
                return new UpdateAppointmentResponse { Success = false, ErrrorMessage = "Unable to find treatment" };
            }

            var dentist = await _dentistRepo.FindByIdAsync(request.DentistId.Value);

            if (dentist == null)
            {
                return new UpdateAppointmentResponse { Success = false, ErrrorMessage = "Unable to find dentist" };
            }
            else
            {
                var profession = _professionRepo.GetAllAsync().Result
                                                .Where(p => p.DentistId == dentist.DentistId)
                                                .FirstOrDefault(p => p.TreatmentId == treatment.TreatmentId);

                if (profession == null)
                {
                    return new UpdateAppointmentResponse { Success = false, ErrrorMessage = "The dentist doesnt provide that treatment" };
                }
            }

            var schedule = await _scheduleRepo.FindByIdAsync(request.ScheduleId.Value);

            if (schedule == null)
            {
                return new UpdateAppointmentResponse { Success = false, ErrrorMessage = "Unable to find schedule" };
            }
            else
            {
                if (schedule.DentistId != dentist.DentistId)
                {
                    return new UpdateAppointmentResponse { Success = false, ErrrorMessage = "The dentist doesnt work at that time" };
                }
            }

            appointment.PatientId = request.PatientId;
            appointment.ArrivalDate = request.ArrivalDate;

            var appointmentDetail = _appDetailRepo.GetAllAsync().Result
                                                  .FirstOrDefault(ad => ad.AppointmentId == appointment.AppointmentId);

            try
            {
                await _appRepo.UpdateAsync(appointment);
                await _appDetailRepo.UpdateAsync(appointmentDetail);

                appointment.AppointmentDetails.Add(appointmentDetail);

                return new UpdateAppointmentResponse { Success = true, Appointment = appointment };
            }
            catch (Exception ex)
            {
                return new UpdateAppointmentResponse { Success = false, ErrrorMessage = "An error has occured " + ex.Message };
            }
        }

        public async Task<Appointment> GetAppointmentById(int id)
        {
            var app = await _appRepo.FindByIdAsync(id);

            if (app != null)
            {
                var cus = await _cusRepo.FindByIdAsync(app.CustomerId.Value);
                var patient = await _patientRepo.FindByIdAsync(app.PatientId.Value);
                var appDetails = _appDetailRepo.GetAllAsync().Result.Where(ad => ad.AppointmentId == app.AppointmentId);

                foreach (var appDetail in appDetails)
                {
                    var treatmeant = await _treatmentRepo.FindByIdAsync(appDetail.TreatmentId.Value);

                    appDetail.Treatment = treatmeant;
                }

                app.Customer = cus;
                app.Patient = patient;
                app.AppointmentDetails = appDetails.ToList();

                return app;
            }

            return null;
        }

        public async Task<CreateAppointmentResponse> UpdateAppointmentStatus(int id)
        {
            var appointment = await _appRepo.FindByIdAsync(id);

            if (appointment == null)
            {
                return null;
            }

            appointment.Status = 1;

            try
            {
                await _appRepo.UpdateAsync(appointment);
            }
            catch (Exception ex)
            {
                return new CreateAppointmentResponse { Success = false, ErrrorMessage = "There has been an error updating the appointment" };
            }

            return new CreateAppointmentResponse { Success = true, Appointment = appointment };
        }
    }
}
