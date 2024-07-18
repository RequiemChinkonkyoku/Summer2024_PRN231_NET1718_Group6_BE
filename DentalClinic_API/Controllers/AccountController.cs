using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("customer-login")]
        public async Task<IActionResult> CustomerLogin([FromBody] LoginRequest loginRequest)
        {
            var token = await _accountService.CustomerLogin(loginRequest.Email, loginRequest.Password);

            if (token == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { Token = token });
        }

        [HttpPost("dentist-login")]
        public async Task<IActionResult> DentistLogin([FromBody] LoginRequest loginRequest)
        {
            var token = await _accountService.DentistLogin(loginRequest.Email, loginRequest.Password);

            if (token == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { Token = token });
        }

        [HttpPost("admin-login")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginRequest loginRequest)
        {
            var token = await _accountService.AdminLogin(loginRequest.Email, loginRequest.Password);

            if (token == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { Token = token });
        }

        [HttpPost("manager-login")]
        public async Task<IActionResult> ManagerLogin([FromBody] LoginRequest loginRequest)
        {
            var token = await _accountService.ManagerLogin(loginRequest.Email, loginRequest.Password);

            if (token == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { Token = token });
        }

        [HttpPost("employee-login")]
        public async Task<IActionResult> EmployeeLogin([FromBody] LoginRequest loginRequest)
        {
            var token = await _accountService.AdminLogin(loginRequest.Email, loginRequest.Password);

            if (token == null)
            {
                token = await _accountService.ManagerLogin(loginRequest.Email, loginRequest.Password);

                if (token == null)
                {
                    token = await _accountService.DentistLogin(loginRequest.Email, loginRequest.Password);
                }
            }

            if (token == null)
            {
                return Unauthorized("Invalid credentials");
            }
            else
            {
                return Ok(new { Token = token });
            }
        }

        [HttpPost("get-value-from-token")]
        public async Task<IActionResult> GetValueFromToken([FromBody] TokenRequest tokenRequest)
        {
            var result = await _accountService.GetValueFromToken(tokenRequest.Token, tokenRequest.ClaimType);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost("customer-register")]
        public async Task<IActionResult> CustomerRegister([FromBody] LoginRequest request)
        {
            var response = await _accountService.CustomerRegister(request.Email, request.Password);

            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPut("customer-change-password")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerChangePassword([FromBody] ChangePasswordRequest request)
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

            var response = await _accountService.CustomerChangePassword(userId, request.CurrentPassword, request.NewPassword);

            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
