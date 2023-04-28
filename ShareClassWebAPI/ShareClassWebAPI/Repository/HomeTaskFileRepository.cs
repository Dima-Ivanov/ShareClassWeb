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
    public class HomeTaskFileRepository : IRepository<HomeTaskFile>
    {
        private DataContext dataContext;

        public HomeTaskFileRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<HomeTaskFile>> GetListAsync()
        {
            return await dataContext.DBHomeTaskFile.ToListAsync();
        }

        public async Task<HomeTaskFile> GetItemAsync(int ID)
        {
            return await dataContext.DBHomeTaskFile.FirstOrDefaultAsync(i => i.ID == ID);
        }

        public async Task CreateAsync(HomeTaskFile homeTaskFile)
        {
            await dataContext.DBHomeTaskFile.AddAsync(homeTaskFile);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(HomeTaskFile homeTaskFile)
        {
            var itemToReplace = dataContext.DBHomeTaskFile.FirstOrDefault(i => i.ID == homeTaskFile.ID);

            if (itemToReplace != null)
            {
                homeTaskFile.CopyPropertiesWithoutId(itemToReplace);
            }

            await dataContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var itemToDelete = dataContext.DBHomeTaskFile.FirstOrDefault(i => i.ID == ID);

            if (itemToDelete != null)
            {
                dataContext.DBHomeTaskFile.Remove(itemToDelete);
                await dataContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}