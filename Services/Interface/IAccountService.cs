using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAccountService
    {
        Task<string> CustomerLogin(string email, string password);
        Task<string> DentistLogin(string email, string password);
        Task<string> AdminLogin(string email, string password);
        Task<string> ManagerLogin(string email, string password);
        Task<string> GetValueFromToken(string token, string claimType);
        Task<Customer> CustomerRegister(string email, string password);
    }
}
