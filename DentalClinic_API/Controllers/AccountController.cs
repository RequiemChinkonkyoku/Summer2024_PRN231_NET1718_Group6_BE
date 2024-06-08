using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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

        //[HttpPost("log-out")]
        //public async Task<IActionResult> Logout(string token)
        //{
        //    await _accountService.BlacklistToken(token);
        //    return Ok();
        //}

        //[HttpPost("check-token-blacklisted")]
        //public async Task<IActionResult> CheckTokenBlacklisted(string token)
        //{
        //    bool result = await _accountService.CheckTokenBlacklisted(token);
        //    return Ok(result);
        //}
    }
}
