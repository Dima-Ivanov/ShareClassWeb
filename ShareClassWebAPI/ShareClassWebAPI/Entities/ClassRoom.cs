using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShareClassWebAPI.Interfaces;

namespace ShareClassWebAPI.Entities
{
    [Table("ClassRoom")]
    public class ClassRoom : IEntity<ClassRoom>
    {
        public ClassRoom()
        {
            this.ClassRoomsUsers = new HashSet<ClassRoomsUsers>();
            this.HomeTask = new HashSet<HomeTask>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public Guid InvitationCode { get; set; }
        public string Description { get; set; }
        public string Teacher_Name { get; set; }
        public int Students_Count { get; set; }
        public System.DateTime Creation_Date { get; set; }
        public int Administrator_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassRoomsUsers> ClassRoomsUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HomeTask> HomeTask { get; set; }

        public void CopyPropertiesWithoutId(ClassRoom classRoom)
        {
            classRoom.Name = this.Name;
            classRoom.InvitationCode = this.InvitationCode;
            classRoom.Description = this.Description;
            classRoom.Students_Count = this.Students_Count;
            classRoom.Creation_Date = this.Creation_Date;
            classRoom.Administrator_ID = this.Administrator_ID;
        }
    }
}
