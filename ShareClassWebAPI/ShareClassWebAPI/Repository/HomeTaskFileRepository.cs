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
        private int maxID;

        public HomeTaskFileRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;

            var list = GetList();

            maxID = 0;
            foreach (var item in list)
            {
                maxID = Math.Max(maxID, item.ID);
            }
        }

        public List<HomeTaskFile> GetList()
        {
            return dataContext.DBHomeTaskFile.ToList();
        }

        public HomeTaskFile GetItem(int ID)
        {
            return dataContext.DBHomeTaskFile.FirstOrDefault(i => i.ID == ID);
        }

        public int Create(HomeTaskFile HomeTaskFile)
        {
            HomeTaskFile.ID = ++maxID;
            dataContext.DBHomeTaskFile.Add(HomeTaskFile);
            return maxID;
        }

        public void Update(HomeTaskFile HomeTaskFile)
        {
            var itemToReplace = dataContext.DBHomeTaskFile.FirstOrDefault(i => i.ID == HomeTaskFile.ID);

            if (itemToReplace != null)
            {
                itemToReplace = HomeTaskFile;
            }
        }

        public bool Delete(int ID)
        {
            var itemToDelete = dataContext.DBHomeTaskFile.FirstOrDefault(i => i.ID == ID);

            if (itemToDelete != null)
            {
                dataContext.DBHomeTaskFile.Remove(itemToDelete);
                return true;
            }
            return false;
        }
    }
}