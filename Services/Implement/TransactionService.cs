using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Models;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepositoryBase<Transaction> _transRepo;
        private readonly IRepositoryBase<Appointment> _appRepo;
        private readonly IRepositoryBase<Customer> _cusRepo;

        public TransactionService(IRepositoryBase<Transaction> transRepo,
                                  IRepositoryBase<Appointment> appRepo,
                                  IRepositoryBase<Customer> cusRepo)
        {
            _transRepo = transRepo;
            _appRepo = appRepo;
            _cusRepo = cusRepo;
        }

        public async Task<Transaction> CreateTransaction(int id, bool success)
        {

            var app = await _appRepo.FindByIdAsync(id);

            if (app == null)
            {
                return null;
            }

            var trans = new Transaction
            {
                AppointmentId = id,
                CustomerId = app.CustomerId,
                Price = app.TotalPrice,
                TransactionTime = DateTime.UtcNow,
                Status = (success == true) ? 1 : 0
            };

            await _transRepo.AddAsync(trans);

            return trans;
        }
    }
}
