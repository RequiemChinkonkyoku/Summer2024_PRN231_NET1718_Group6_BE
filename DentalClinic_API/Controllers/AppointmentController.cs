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
        public async Task<IActionResult> GetAllAppointments()
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

            if (appointments != null)
            {
                return Ok(appointments);
            }
            else
            {
                return BadRequest("There has been an error");
            }
        }


        [HttpGet("get-account-appointments")]
        [Authorize(Roles = "Customer, Dentist")]
        public async Task<IActionResult> GetAccountAppointments()
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
            else
            {
                return NotFound("You dont have access");
            }

            if (appointmentList != null)
            {
                return Ok(appointmentList);
            }
            else
            {
                return BadRequest("No appointment found");
            }
        }

        [HttpPost("get-current-appointment-list")]
        [Authorize(Roles = "Customer, Dentist")]
        public async Task<IActionResult> GetCurrentAppointmentList()
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
            else
            {
                return NotFound("You dont have access");
            }

            if (appointmentList != null)
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
        [Authorize(Roles = "Customer, Dentist")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            int userID = 0;

            try
            {
                userID = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Unauthorized();
            }

            var response = await _appService.CreateAppointment(request, userID);

            if (!response.Success)
            {
                return BadRequest(response.ErrrorMessage);
            }

            return Ok(response.Appointment);
        }

        [HttpPost("update-appointment")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateAppointment([FromBody] UpdateAppointmentRequest request)
        {
            int userID = 0;

            try
            {
                userID = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Unauthorized();
            }

            var response = await _appService.UpdateAppointment(request);

            if (response.Success)
            {
                return Ok(response.Appointment);
            }
            else
            {
                return BadRequest(response.ErrrorMessage);
            }
        }

        [HttpGet("get-app-by-id/{id}")]
        [Authorize]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            int userID = 0;

            try
            {
                userID = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Unauthorized();
            }

            var app = await _appService.GetAppointmentById(id);

            if (app != null)
            {
                return Ok(app);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
