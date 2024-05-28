using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;
using Services.Implement;
using Services.Interface;

namespace DentalClinic_API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("login", Name = "AccountLogin")]
        public async Task<ActionResult<Account>> PatientLogin([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid request");
            }

            var account = await _accountService.AccountLogin(loginRequest.Email, loginRequest.Password);

            if (account != null)
            {
                HttpContext.Session.SetInt32("PatientID", account.AccountId);
                return Ok(new { account, redirectUrl = "https://www.youtube.com/watch?v=v56H49q_elw&list=RDMM1iDk_rHA7FM&index=6" });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("session", Name = "GetCurrentSession")]
        public IActionResult GetSession()
        {
            var _session = HttpContext.Session;
            var accountID = _session.GetInt32("PatientID");

            if (accountID != null)
            {
                return Ok(new { AccountID = accountID});
            }
            else
            {
                return NotFound("Session data not found.");
            }
        }
    }
}
