﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("account-login")]
        public async Task<IActionResult> AccountLogin([FromBody] LoginRequest loginRequest)
        {
            var token = await _accountService.AccountLogin(loginRequest.Email, loginRequest.Password);

            if (token == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { Token = token });

            //if (loginRequest == null)
            //{
            //    return BadRequest("Invalid request");
            //}

            //var account = await _accountService.AccountLogin(loginRequest.Email, loginRequest.Password);

            //if (account != null)
            //{
            //    HttpContext.Session.SetInt32("AccountID", account.AccountId);
            //    return Ok(new { account, redirectUrl = "https://www.youtube.com/watch?v=v56H49q_elw&list=RDMM1iDk_rHA7FM&index=6" });
            //}
            //else
            //{
            //    return Unauthorized();
            //}
        }

        //[HttpGet("get-session")]
        //public IActionResult GetSession()
        //{
        //    var _session = HttpContext.Session;
        //    var accountID = _session.GetInt32("AccountID");

        //    if (accountID != null)
        //    {
        //        return Ok(new { AccountID = accountID });
        //    }
        //    else
        //    {
        //        return NotFound("Session data not found.");
        //    }
        //}

        [HttpGet("get-all-accounts")]
        [Authorize]
        public async Task<ActionResult<List<Account>>> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }
    }
}
