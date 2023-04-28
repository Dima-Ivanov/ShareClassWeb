using Microsoft.EntityFrameworkCore;
using ShareClassWebAPI.Entities;
using ShareClassWebAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareClassWebAPI.Repository
{
    public class UserRepository : IRepository<User>
    {
        private DataContext dataContext;

        public UserRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<User>> GetListAsync()
        {
            return await dataContext.DBUser.ToListAsync();
        }

        public async Task<User> GetItemAsync(int ID)
        {
            return await dataContext.DBUser.FirstOrDefaultAsync(i => i.ID == ID);
        }

        public async Task CreateAsync(User user)
        {
            await dataContext.DBUser.AddAsync(user);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var itemToReplace = dataContext.DBUser.FirstOrDefault(i => i.ID == user.ID);

            if (itemToReplace != null)
            {
                itemToReplace = user;
            }

            await dataContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var itemToDelete = dataContext.DBUser.FirstOrDefault(i => i.ID == ID);

            if (itemToDelete != null)
            {
                dataContext.DBUser.Remove(itemToDelete);
                await dataContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}