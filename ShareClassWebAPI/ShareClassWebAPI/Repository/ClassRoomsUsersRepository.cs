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
    public class ClassRoomsUsersRepository : IRepository<ClassRoomsUsers>
    {
        private DataContext dataContext;

        public ClassRoomsUsersRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<ClassRoomsUsers>> GetListAsync()
        {
            return await dataContext.DBClassRoomsUsers.Include(i => i.ClassRoom).Include(i => i.User).ToListAsync();
        }

        public async Task<ClassRoomsUsers> GetItemAsync(int ID)
        {
            return await dataContext.DBClassRoomsUsers.FirstOrDefaultAsync(i => i.ID == ID);
        }

        public async Task CreateAsync(ClassRoomsUsers classRoomsUsers)
        {
            await dataContext.DBClassRoomsUsers.AddAsync(classRoomsUsers);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ClassRoomsUsers classRoomsUsers)
        {
            var itemToReplace = dataContext.DBClassRoomsUsers.FirstOrDefault(i => i.ID == classRoomsUsers.ID);

            if (itemToReplace != null)
            {
                classRoomsUsers.CopyPropertiesWithoutId(itemToReplace);
            }

            await dataContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var itemToDelete = dataContext.DBClassRoomsUsers.FirstOrDefault(i => i.ID == ID);

            if (itemToDelete != null)
            {
                dataContext.DBClassRoomsUsers.Remove(itemToDelete);
                await dataContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}