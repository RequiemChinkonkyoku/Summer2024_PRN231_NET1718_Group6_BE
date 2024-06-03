using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ICustomerService
    {
        Task<string> CustomerLogin(string email, string password);

        Task<List<Account>> GetAllCustomersAsync();
    }
}
