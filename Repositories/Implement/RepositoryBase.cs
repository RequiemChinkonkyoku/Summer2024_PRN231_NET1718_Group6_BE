using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implement
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {

        DentalClinicDbContext context;
        DbSet<T> dbSet;

        public RepositoryBase()
        {
            context = new DentalClinicDbContext();
            dbSet = context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public void Add(T item)
        {
            dbSet.Add(item);
            context.SaveChanges();
        }

        public void Delete(T item)
        {
            dbSet.Remove(item);
            context.SaveChanges();
        }

        public void Update(T item)
        {
            dbSet.Update(item);
            context.SaveChanges();
        }

        public async Task<T> FindByIdAsync(T id)
        {
            return await dbSet.FindAsync(id);
        }

    }
}
