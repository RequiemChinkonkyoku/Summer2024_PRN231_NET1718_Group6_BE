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
        private readonly IRepositoryBase<Customer> _customerRepo;
        private readonly IRepositoryBase<Dentist> _dentistRepo;

        public AccountService(IRepositoryBase<Customer> customerRepo, IRepositoryBase<Dentist> dentistRepo)
        {
            _customerRepo = customerRepo;
            _dentistRepo = dentistRepo;
        }

        public async Task<string> CustomerLogin(string email, string password)
        {
            var customer = CustomerValidate(email, password).Result;

            // Validate user credentials
            if (customer != null)
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
                    new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, "Customer")
                }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
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

        private async Task<Customer> CustomerValidate(string email, string password)
        {
            var customers = await _customerRepo.GetAllAsync();
            if (!customers.IsNullOrEmpty())
            {
                var customer = customers.FirstOrDefault(x => x.Email.Equals(email)
                                                             && x.Password.Equals(password));
                if (customer != null)
                {
                    return customer;
                }
            }
            return null;
        }

        public async Task<string> DentistLogin(string email, string password)
        {
            var dentist = DentistValidate(email, password).Result;

            // Validate user credentials
            if (dentist != null)
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
                        new Claim(ClaimTypes.NameIdentifier, dentist.DentistId.ToString()),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Role, "Dentist")
                }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
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


        private async Task<Dentist> DentistValidate(string email, string password)
        {
            var dentists = await _dentistRepo.GetAllAsync();
            if (!dentists.IsNullOrEmpty())
            {
                var dentist = dentists.FirstOrDefault(x => x.Email.Equals(email)
                                                      && x.Password.Equals(password));
                if (!(dentist == null))
                {
                    return dentist;
                }
            }
            return null;
        }

        public async Task<string> GetValueFromToken(string token, string claimType)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var claim = jwtToken.Claims.FirstOrDefault(c => c.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase));
                    if (claim != null)
                    {
                        return claim.Value;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while extracting claim from JWT: {ex.Message}");
                return null;
            }
        }

        //public async Task BlacklistToken(string token)
        //{
        //    try
        //    {
        //        var handler = new JwtSecurityTokenHandler();

        //        if (handler.CanReadToken(token))
        //        {
        //            var jwtToken = handler.ReadJwtToken(token);

        //            //blacklist code
        //            BlacklistedToken _token = new BlacklistedToken();
        //            _token.TokenString = token;
        //            _token.BlacklistTime = DateTime.UtcNow;
        //            await _blacklistedTokenRepo.AddAsync(_token);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Exception while extracting claim from JWT: {ex.Message}");
        //    }
        //}

        //public async Task<bool> CheckTokenBlacklisted(string token)
        //{
        //    bool result = false;
        //    var blacklistedTokenList = await _blacklistedTokenRepo.GetAllAsync();
        //    var _token = blacklistedTokenList.FirstOrDefault(t => t.TokenString.Equals(token));

        //    if (_token != null)
        //    {
        //        result = true;
        //    }

        //    return result;
        //}
    }
}
