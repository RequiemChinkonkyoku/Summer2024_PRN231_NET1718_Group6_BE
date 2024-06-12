using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAppointmentService
    {
        Task<Appointment> CancelAppointment(int appID);
        Task<CreateAppointmentResponse> CreateAppointment(CreateAppointmentRequest request, int accountID);
        Task<List<Appointment>> GetAllAppointments();
        Task<List<Appointment>> GetCustomerAppointments(int accountID);
        Task<List<Appointment>> GetDentistAppointments(int dentistID);
        Task<List<Appointment>> GetCurrentAppointmentList(int id);
    }
}
