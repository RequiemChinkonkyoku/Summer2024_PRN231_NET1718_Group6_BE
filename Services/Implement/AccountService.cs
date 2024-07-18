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
        private readonly IConfiguration _config;
        public AccountService(IRepositoryBase<Customer> customerRepo, IRepositoryBase<Dentist> dentistRepo, IConfiguration config)
        {
            _customerRepo = customerRepo;
            _dentistRepo = dentistRepo;
            _config = config;
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
                var customer = customers.FirstOrDefault(x => x.Email.Equals(email));
                if (customer != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, customer.PasswordHash))
                    {
                        return customer;
                    }
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
                var dentist = dentists.FirstOrDefault(x => x.Email.Equals(email));
                if (dentist != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, dentist.PasswordHash))
                    {
                        return dentist;
                    }
                }
            }
            return null;
        }

        public async Task<string> AdminLogin(string email, string password)
        {
            var isValidAdmin = await AdminValidate(email, password);

            if (isValidAdmin)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "0"),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Role, "Admin")
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    Issuer = _config["Jwt:Issuer"],
                    Audience = _config["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            return null;
        }

        private Task<bool> AdminValidate(string email, string password)
        {
            var adminEmail = _config["Admin:Email"];
            var adminPassword = _config["Admin:Password"];

            if (email == adminEmail && password == adminPassword)
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public async Task<string> ManagerLogin(string email, string password)
        {
            var isValidManager = await ManagerValidate(email, password);

            if (isValidManager)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "0"),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Role, "Manager")
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    Issuer = _config["Jwt:Issuer"],
                    Audience = _config["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            return null;
        }

        private Task<bool> ManagerValidate(string email, string password)
        {
            var managerEmail = _config["Manager:Email"];
            var managerPassword = _config["Manager:Password"];

            if (email == managerEmail && password == managerPassword)
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
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

        public async Task<Customer> CustomerRegister(string email, string password)
        {
            var customer = new Customer();
            customer.Email = email;
            customer.Password = password;
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            customer.PasswordHash = passwordHash;
            customer.Status = 1;

            _customerRepo.Add(customer);
            return customer;
        }

        public async Task<Customer> CustomerChangePassword(int userId, string currentPassword, string newPassword)
        {
            var customers = await _customerRepo.GetAllAsync();
            if (!customers.IsNullOrEmpty())
            {
                var customer = customers.FirstOrDefault(x => x.CustomerId.Equals(userId));
                if (customer != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(currentPassword, customer.PasswordHash))
                    {
                        customer.Password = newPassword;
                        string temp = BCrypt.Net.BCrypt.HashPassword(newPassword);
                        customer.PasswordHash = temp;

                        _customerRepo.UpdateAsync(customer);
                        return customer;
                    }
                }
            }

            return null;
        }
    }
}
