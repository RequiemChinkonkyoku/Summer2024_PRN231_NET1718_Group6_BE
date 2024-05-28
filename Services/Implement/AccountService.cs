using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories.Implement;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Account> AccountLogin(string email, string password)
        {
            var accounts = await _accountRepo.GetAllAsync();

            if (!accounts.IsNullOrEmpty())
            {
                var account = accounts.FirstOrDefault(a => a.Email.Equals(email) &&
                                                           a.Password.Equals(password));

                if (account != null)
                {
                    return account;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
