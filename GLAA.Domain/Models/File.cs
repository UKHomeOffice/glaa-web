using System;
using System.ComponentModel.DataAnnotations;

namespace GLAA.Domain.Models
{
    public class File : IDeletable, IId
    {
        [Key]
        public int Id { get; set; }
        public Guid Key { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DateDeleted { get; set; }
        public Licence Licence { get; set; }
        
    }
}
