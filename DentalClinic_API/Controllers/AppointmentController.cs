using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using Repositories.Interface;
using Services.Interface;
using System.Net;

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
            var session = GetSession();

            if (session != null)
            {
                return Ok(await _appService.GetAllAppointments());
            }
            else
            {
                return Unauthorized("Unable to get session");
            }
        }


        [HttpGet("get-account-appointment")]
        public async Task<ActionResult<List<Appointment>>> GetAccountAppointments()
        {
            var session = GetSession();

            if (session != null)
            {
                string accountType = session.accountType;
                int accountID = session.accountID;

                List<Appointment> appointmentList = null;

                if (accountType.Equals("Customer"))
                {
                    appointmentList = await _appService.GetCustomerAppointments(accountID);
                }
                else if (accountType.Equals("Dentist"))
                {
                    appointmentList = await _appService.GetDentistAppointments(accountID);
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
            else
            {
                return Unauthorized("Unable to get session");
            }
        }

        [HttpDelete("cancel-appointment/{id}")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var session = GetSession();

            if (session != null)
            {
                var appointment = await _appService.CancelAppointment(id);

                if (appointment == null)
                {
                    return BadRequest("Unable to cancel appointment with ID: " + id);
                }

                return Ok(appointment);
            }
            else
            {
                return Unauthorized("Unable to get session");
            }
        }

        [HttpPost("create-appointment")]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            var session = GetSession();

            if (session == null)
            {
                return Unauthorized("Unable to get session");
            }

            if (request == null)
            {
                return BadRequest("Invalid request");
            }

            var response = await _appService.CreateAppointment(request, session.accountID);

            if (!response.Success)
            {
                return BadRequest(response.ErrrorMessage);
            }

            return Ok(response.Appointment);
        }

        private dynamic GetSession()
        {
            var _session = HttpContext.Session;

            if (_session.IsAvailable)
            {
                var accountType = _session.GetString("AccountType");
                var accountID = _session.GetInt32("AccountID");

                if (accountType != null && accountID.HasValue)
                {
                    return new { accountType, accountID };
                }
            }

            return null;
        }
    }
}
