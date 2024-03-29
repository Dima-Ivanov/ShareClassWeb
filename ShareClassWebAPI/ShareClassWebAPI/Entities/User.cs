﻿using ShareClassWebAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareClassWebAPI.Entities
{
    [Table("User")]
    public class User : IdentityUser<int>, IEntity<User>
    {
        public User()
        {
            this.ClassRoomsUsers = new HashSet<ClassRoomsUsers>();
            this.Reaction = new HashSet<Reaction>();
        }

        public string Login { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassRoomsUsers> ClassRoomsUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reaction> Reaction { get; set; }

        public void CopyPropertiesWithoutId(User user)
        {
            user.Login = this.Login;
            user.PasswordHash = this.PasswordHash;
            user.Name = this.Name;
            user.UserName = this.UserName;
        }
    }
}