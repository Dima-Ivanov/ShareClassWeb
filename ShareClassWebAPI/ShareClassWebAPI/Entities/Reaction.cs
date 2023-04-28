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
    [Table("Reaction")]
    public class Reaction : IEntity<Reaction>
    {
        [Key]
        public int ID { get; set; }

        public virtual ReactionType ReactionType { get; set; }
        public virtual Solution Solution { get; set; }
        public virtual User User { get; set; }

        public void CopyPropertiesWithoutId(Reaction reaction)
        {
            reaction.ReactionType = this.ReactionType;
            reaction.Solution = this.Solution;
            reaction.User = this.User;
        }
    }
}