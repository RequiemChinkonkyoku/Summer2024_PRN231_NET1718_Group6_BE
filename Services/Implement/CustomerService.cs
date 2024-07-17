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
    public class CustomerService : ICustomerService
    {
        private readonly IRepositoryBase<Customer> _customerRepo;

        public CustomerService(IRepositoryBase<Customer> customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepo.GetAllAsync();
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            var customers = await _customerRepo.GetAllAsync();

            if (!customers.IsNullOrEmpty())
            {
                var customer = customers.FirstOrDefault(p => p.CustomerId == id);

                if (customer != null)
                {
                    return customer;
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
