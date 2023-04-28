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
    public class SolutionFileRepository : IRepository<SolutionFile>
    {
        private DataContext dataContext;

        public SolutionFileRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<SolutionFile>> GetListAsync()
        {
            return await dataContext.DBSolutionFile.ToListAsync();
        }

        public async Task<SolutionFile> GetItemAsync(int ID)
        {
            return await dataContext.DBSolutionFile.FirstOrDefaultAsync(i => i.ID == ID);
        }

        public async Task CreateAsync(SolutionFile solutionFile)
        {
            await dataContext.DBSolutionFile.AddAsync(solutionFile);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(SolutionFile solutionFile)
        {
            var itemToReplace = dataContext.DBSolutionFile.FirstOrDefault(i => i.ID == solutionFile.ID);

            if (itemToReplace != null)
            {
                solutionFile.CopyPropertiesWithoutId(itemToReplace);
            }

            await dataContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var itemToDelete = dataContext.DBSolutionFile.FirstOrDefault(i => i.ID == ID);

            if (itemToDelete != null)
            {
                dataContext.DBSolutionFile.Remove(itemToDelete);
                await dataContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}