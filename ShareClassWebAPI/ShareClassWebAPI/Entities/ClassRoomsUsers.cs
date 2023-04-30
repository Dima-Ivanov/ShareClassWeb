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
    [Table("ClassRoomsUsers")]
    public class ClassRoomsUsers : IEntity<ClassRoomsUsers>
    {
        [Key]
        public int ID { get; set; }

        public virtual ClassRoom ClassRoom { get; set; }
        public virtual User User { get; set; }

        public void CopyPropertiesWithoutId(ClassRoomsUsers classRoomsUsers)
        {
            classRoomsUsers.ClassRoom = this.ClassRoom;
            classRoomsUsers.User = this.User;
        }
    }
}