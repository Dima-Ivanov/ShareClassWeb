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
    [Table("HomeTask")]
    public class HomeTask : IEntity<HomeTask>
    {
        public HomeTask()
        {
            this.HomeTaskFile = new HashSet<HomeTaskFile>();
            this.Solution = new HashSet<Solution>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime Creation_Date { get; set; }
        public System.DateTime Deadline_Date { get; set; }

        public virtual ClassRoom ClassRoom { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HomeTaskFile> HomeTaskFile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Solution> Solution { get; set; }

        public void CopyPropertiesWithoutId(HomeTask homeTask)
        {
            homeTask.Name = this.Name;
            homeTask.Description = this.Description;
            homeTask.Creation_Date = this.Creation_Date;
            homeTask.Deadline_Date = this.Deadline_Date;
            homeTask.ClassRoom = this.ClassRoom;
        }
    }
}