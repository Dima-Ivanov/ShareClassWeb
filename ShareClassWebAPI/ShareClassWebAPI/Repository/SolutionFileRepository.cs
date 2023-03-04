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
        private int maxID;

        public SolutionFileRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;

            var list = GetList();

            maxID = 0;
            foreach (var item in list)
            {
                maxID = Math.Max(maxID, item.ID);
            }
        }

        public List<SolutionFile> GetList()
        {
            return dataContext.DBSolutionFile.ToList();
        }

        public SolutionFile GetItem(int ID)
        {
            return dataContext.DBSolutionFile.FirstOrDefault(i => i.ID == ID);
        }

        public int Create(SolutionFile classRoom)
        {
            classRoom.ID = ++maxID;
            dataContext.DBSolutionFile.Add(classRoom);
            return maxID;
        }

        public void Update(SolutionFile classRoom)
        {
            var itemToReplace = dataContext.DBSolutionFile.FirstOrDefault(i => i.ID == classRoom.ID);

            if (itemToReplace != null)
            {
                itemToReplace = classRoom;
            }
        }

        public bool Delete(int ID)
        {
            var itemToDelete = dataContext.DBSolutionFile.FirstOrDefault(i => i.ID == ID);

            if (itemToDelete != null)
            {
                dataContext.DBSolutionFile.Remove(itemToDelete);
                return true;
            }
            return false;
        }
    }
}