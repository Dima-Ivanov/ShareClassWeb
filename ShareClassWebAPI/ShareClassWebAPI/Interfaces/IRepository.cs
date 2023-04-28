using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareClassWebAPI.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetListAsync();
        Task<T> GetItemAsync(int ID);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task<bool> DeleteAsync(int ID);
    }
}