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
    public class ReactionRepository : IRepository<Reaction>
    {
        private DataContext dataContext;

        public ReactionRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<Reaction>> GetListAsync()
        {
            return await dataContext.DBReaction.ToListAsync();
        }

        public async Task<Reaction> GetItemAsync(int ID)
        {
            return await dataContext.DBReaction.FirstOrDefaultAsync(i => i.ID == ID);
        }

        public async Task CreateAsync(Reaction reaction)
        {
            await dataContext.DBReaction.AddAsync(reaction);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reaction reaction)
        {
            var itemToReplace = dataContext.DBReaction.FirstOrDefault(i => i.ID == reaction.ID);

            if (itemToReplace != null)
            {
                reaction.CopyPropertiesWithoutId(itemToReplace);
            }

            await dataContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var itemToDelete = dataContext.DBReaction.FirstOrDefault(i => i.ID == ID);

            if (itemToDelete != null)
            {
                dataContext.DBReaction.Remove(itemToDelete);
                await dataContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}