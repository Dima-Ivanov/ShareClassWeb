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
    public class ReactionTypeRepository : IRepository<ReactionType>
    {
        private DataContext dataContext;

        public ReactionTypeRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<ReactionType>> GetListAsync()
        {
            return await dataContext.DBReactionType.ToListAsync();
        }

        public async Task<ReactionType> GetItemAsync(int ID)
        {
            return await dataContext.DBReactionType.FirstOrDefaultAsync(i => i.ID == ID);
        }

        public async Task CreateAsync(ReactionType reactionType)
        {
            await dataContext.DBReactionType.AddAsync(reactionType);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReactionType reactionType)
        {
            var itemToReplace = dataContext.DBReactionType.FirstOrDefault(i => i.ID == reactionType.ID);

            if (itemToReplace != null)
            {
                reactionType.CopyPropertiesWithoutId(itemToReplace);
            }

            await dataContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var itemToDelete = dataContext.DBReactionType.FirstOrDefault(i => i.ID == ID);

            if (itemToDelete != null)
            {
                dataContext.DBReactionType.Remove(itemToDelete);
                await dataContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}