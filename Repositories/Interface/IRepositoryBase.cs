using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        void Add(T item);

        void Update(T item);

        void Delete(T item);

        Task<T> FindByIdAsync(T id);

    }
}
