using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Services.Interface;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace DentalClinic_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MomoController : Controller
    {
        private readonly IMomoService _momoService;
        private readonly IAppointmentService _appService;
        private readonly ITransactionService _transactionService;

        public MomoController(IMomoService service, IAppointmentService appService, ITransactionService transactionService)
        {
            _momoService = service;
            _appService = appService;
            _transactionService = transactionService;
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
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

            var app = await _appService.GetAppointmentById(request.AppointmentId);

            if (app == null)
            {
                return BadRequest("The appointment with the ID " + request.AppointmentId + " does not exist.");
            }

            if (app.CustomerId != userID)
            {
                return BadRequest("The appointment does not belong to this account.");
            }

            request.FullName = app.Patient.Name;
            request.Amount = app.TotalPrice.Value;

            var response = await _momoService.CreatePaymentAsync(request);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPost("payment-execute/{id}")]
        public async Task<IActionResult> PaymentExecuteAsync(int id)
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

            if (id != null)
            {
                var updateResponse = await _appService.UpdateAppointmentStatus(id);

                if (!updateResponse.Success)
                {
                    _transactionService.CreateTransaction(id, updateResponse.Success);

                    return BadRequest(updateResponse.ErrrorMessage);
                }

                _transactionService.CreateTransaction(id, updateResponse.Success);

                return Ok(updateResponse);
            }

            return BadRequest("There has been an error during the payment process");
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok("test");
        }
    }
}