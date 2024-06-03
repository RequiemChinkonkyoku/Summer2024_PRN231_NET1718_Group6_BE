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
    }
}
