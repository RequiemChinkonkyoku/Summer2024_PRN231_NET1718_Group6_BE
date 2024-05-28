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
        Task<Account> AccountLogin(string email, string password);
    }
}
