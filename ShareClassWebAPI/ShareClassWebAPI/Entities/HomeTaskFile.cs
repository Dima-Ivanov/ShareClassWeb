using ShareClassWebAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareClassWebAPI.Entities
{
    [Table("HomeTaskFile")]
    public class HomeTaskFile : IEntity<HomeTaskFile>
    {
        [Key]
        public int ID { get; set; }
        public string File_Name { get; set; }

        public virtual HomeTask HomeTask { get; set; }

        public void CopyPropertiesWithoutId(HomeTaskFile homeTaskFile)
        {
            homeTaskFile.File_Name = this.File_Name;
            homeTaskFile.HomeTask = this.HomeTask;
        }
    }
}