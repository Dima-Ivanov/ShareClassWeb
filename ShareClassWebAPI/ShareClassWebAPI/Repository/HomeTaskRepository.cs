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
    public class HomeTaskRepository : IRepository<HomeTask>
    {
        private DataContext dataContext;

        public HomeTaskRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<HomeTask>> GetListAsync()
        {
            return await dataContext.DBHomeTask.Include(i => i.HomeTaskFile).Include(i => i.Solution).Include(i => i.ClassRoom).ToListAsync();
        }

        public async Task<HomeTask> GetItemAsync(int ID)
        {
            return await dataContext.DBHomeTask.FirstOrDefaultAsync(i => i.ID == ID);
        }

        public async Task CreateAsync(HomeTask homeTask)
        {
            await dataContext.DBHomeTask.AddAsync(homeTask);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(HomeTask homeTask)
        {
            var itemToReplace = dataContext.DBHomeTask.FirstOrDefault(i => i.ID == homeTask.ID);

            if (itemToReplace != null)
            {
                homeTask.CopyPropertiesWithoutId(itemToReplace);
            }

            await dataContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var itemToDelete = dataContext.DBHomeTask.FirstOrDefault(i => i.ID == ID);

            if (itemToDelete != null)
            {
                dataContext.DBHomeTask.Remove(itemToDelete);
                await dataContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}