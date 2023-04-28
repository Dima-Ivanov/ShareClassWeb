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
    public class SolutionRepository : IRepository<Solution>
    {
        private DataContext dataContext;

        public SolutionRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<Solution>> GetListAsync()
        {
            return await dataContext.DBSolution.ToListAsync();
        }

        public async Task<Solution> GetItemAsync(int ID)
        {
            return await dataContext.DBSolution.FirstOrDefaultAsync(i => i.ID == ID);
        }

        public async Task CreateAsync(Solution solution)
        {
            await dataContext.DBSolution.AddAsync(solution);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Solution solution)
        {
            var itemToReplace = dataContext.DBSolution.FirstOrDefault(i => i.ID == solution.ID);

            if (itemToReplace != null)
            {
                solution.CopyPropertiesWithoutId(itemToReplace);
            }

            await dataContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var itemToDelete = dataContext.DBSolution.FirstOrDefault(i => i.ID == ID);

            if (itemToDelete != null)
            {
                dataContext.DBSolution.Remove(itemToDelete);
                await dataContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}