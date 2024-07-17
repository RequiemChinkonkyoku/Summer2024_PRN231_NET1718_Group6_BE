using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using Services.Implement;
using Services.Interface;
using System.Security.Claims;

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

        [HttpGet("get-all-customers")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<ActionResult<List<Account>>> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("get-current-customer")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<List<Account>>> GetCurrentCustomer()
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

            var customers = await _customerService.GetCustomerById(userId);
            return Ok(customers);
        }
    }
}
