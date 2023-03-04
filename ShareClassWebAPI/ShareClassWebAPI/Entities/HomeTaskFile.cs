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
    public class HomeTaskFile
    {
        [Key]
        public int ID { get; set; }
        public string File_Name { get; set; }
        public int HomeTask_ID { get; set; }

        public virtual HomeTask HomeTask { get; set; }
    }
}