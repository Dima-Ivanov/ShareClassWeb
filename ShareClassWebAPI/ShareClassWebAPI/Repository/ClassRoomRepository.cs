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
    public class ClassRoomRepository : IRepository<ClassRoom>
    {
        private DataContext dataContext;

        public ClassRoomRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<ClassRoom>> GetListAsync()
        {
            return await dataContext.DBClassRoom.ToListAsync();
        }

        public async Task<ClassRoom> GetItemAsync(int ID)
        {
            return await dataContext.DBClassRoom.FirstOrDefaultAsync(i => i.ID == ID);
        }

        public async Task CreateAsync(ClassRoom classRoom)
        {
            await dataContext.DBClassRoom.AddAsync(classRoom);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ClassRoom classRoom)
        {
            var itemToReplace = dataContext.DBClassRoom.FirstOrDefault(i => i.ID == classRoom.ID);

            if (itemToReplace != null)
            {
                itemToReplace.Name = classRoom.Name;
                itemToReplace.InvitationCode = classRoom.InvitationCode;
                itemToReplace.Description = classRoom.Description;
                itemToReplace.Teacher_Name = classRoom.Teacher_Name;
                itemToReplace.Students_Count = classRoom.Students_Count;
                itemToReplace.Creation_Date = classRoom.Creation_Date;
                itemToReplace.Administrator_ID = classRoom.Administrator_ID;
            }

            await dataContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var itemToDelete = dataContext.DBClassRoom.FirstOrDefault(i => i.ID == ID);

            if (itemToDelete != null)
            {
                dataContext.DBClassRoom.Remove(itemToDelete);
                await dataContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}