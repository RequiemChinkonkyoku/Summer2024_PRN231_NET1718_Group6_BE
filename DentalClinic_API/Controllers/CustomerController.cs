using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using Services.Implement;
using Services.Interface;

namespace DentalClinic_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("customer-login")]
        public async Task<ActionResult<Customer>> CustomerLogin([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid request");
            }

            var customer = await _customerService.CustomerLogin(loginRequest.Email, loginRequest.Password);

            if (customer != null)
            {
                HttpContext.Session.SetInt32("AccountID", customer.CustomerId);
                HttpContext.Session.SetString("AccountType", "Customer");
                return Ok(new { customer, redirectUrl = "https://www.youtube.com/watch?v=v56H49q_elw&list=RDMM1iDk_rHA7FM&index=6" });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            Response.Cookies.Delete(".CCP.Session");

            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("get-session")]
        public IActionResult GetSession()
        {
            var _session = HttpContext.Session;
            var accountID = _session.GetInt32("AccountID");

            if (accountID != null)
            {
                return Ok(new { AccountID = accountID });
            }
            else
            {
                return NotFound("Session data not found.");
            }
        }
    }
}
