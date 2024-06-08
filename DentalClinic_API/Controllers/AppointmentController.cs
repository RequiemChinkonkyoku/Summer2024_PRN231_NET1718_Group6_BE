using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using Repositories.Interface;
using Services.Interface;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DentalClinic_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appService = appointmentService;
        }

        [HttpGet("get-all-appointment")]
        public async Task<ActionResult<List<Appointment>>> GetAllAppointments()
        {
            int userId = 0;

            try
            {
                userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            var appointments = await _appService.GetAllAppointments();

            return Ok(appointments);
        }


        [HttpGet("get-account-appointment")]
        public async Task<ActionResult<List<Appointment>>> GetAccountAppointments()
        {
            int userId = 0;
            string userRole = "";

            try
            {
                userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
                userRole = User.FindFirst(ClaimTypes.Role)?.Value.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            List<Appointment> appointmentList = null;

            if (userRole.Equals("Customer"))
            {
                appointmentList = await _appService.GetCustomerAppointments(userId);
            }
            else if (userRole.Equals("Dentist"))
            {
                appointmentList = await _appService.GetDentistAppointments(userId);
            }

            if (!appointmentList.IsNullOrEmpty())
            {
                return Ok(appointmentList);
            }
            else
            {
                return BadRequest("No appointment found");
            }
        }

        [HttpDelete("cancel-appointment/{id}")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            int userId = 0;
            try
            {
                userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            var appointment = await _appService.CancelAppointment(id);

            if (appointment == null)
            {
                return BadRequest("Unable to cancel appointment with ID: " + id);
            }

            return Ok(appointment);
        }

        [HttpPost("create-appointment")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            int userId = 0;
            try
            {
                userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            var response = await _appService.CreateAppointment(request, userId);

            if (!response.Success)
            {
                return BadRequest(response.ErrrorMessage);
            }

            return Ok(response.Appointment);
        }
    }
}
