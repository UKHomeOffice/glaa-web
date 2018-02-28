using System;
using System.ComponentModel.DataAnnotations;

namespace GLAA.Domain.Models
{
    public class PreviousTradingName : IId, ILinkedToLicence, IDeletable
    {
        [Key]
        public int Id { get; set; }

        public string BusinessName { get; set; }

        public string Town { get; set; }

        public string Country { get; set; }

        public virtual Licence Licence { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
