using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories.Implement;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class AccountService : IAccountService
    {
        private readonly IRepositoryBase<Account> _accountRepo;

        public AccountService(IRepositoryBase<Account> accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<string> AccountLogin(string email, string password)
        {
            // Validate user credentials
            if (!(AccountValidate(email, password) == null))
            {
                // Generate JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                IConfiguration config = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", true, true)
                  .Build();
                var strConn = config["ConnectionStrings:DentalClinicDB"];
                var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, email),
                    new Claim(ClaimTypes.Email, email)
                }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = config["Jwt:Issuer"],
                    Audience = config["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            // If credentials are invalid
            return null;
        }

        private async Task<Account> AccountValidate(string email, string password)
        {
            var accounts = await _accountRepo.GetAllAsync();
            if (!accounts.IsNullOrEmpty())
            {
                var account = accounts.FirstOrDefault(x => x.Email.Equals(email)
                                                      && x.Password.Equals(password));
                if (!(account == null))
                {
                    return account;
                }
            }
            return null;
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _accountRepo.GetAllAsync();
        }
    }
}
