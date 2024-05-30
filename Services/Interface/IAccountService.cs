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
        Task<string> AccountLogin(string email, string password);

        Task<List<Account>> GetAllAccountsAsync();
    }
}
