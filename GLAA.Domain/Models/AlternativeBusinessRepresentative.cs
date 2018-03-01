using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLAA.Domain.Models
{
    public class AlternativeBusinessRepresentative : Person, IPerson, IId, ILinkedToLicence, IDeletable
    {        
        [ForeignKey(nameof(LicenceId))]
        public virtual Licence Licence { get; set; }
        
        public int LicenceId { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
