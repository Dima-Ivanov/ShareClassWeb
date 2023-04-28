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
    [Table("SolutionFile")]
    public class SolutionFile : IEntity<SolutionFile>
    {
        [Key]
        public int ID { get; set; }
        public string File_Name { get; set; }

        public virtual Solution Solution { get; set; }

        public void CopyPropertiesWithoutId(SolutionFile solutionFile)
        {
            solutionFile.File_Name = this.File_Name;
            solutionFile.Solution = this.Solution;
        }
    }
}