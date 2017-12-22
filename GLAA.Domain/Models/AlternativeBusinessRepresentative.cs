using System.ComponentModel.DataAnnotations.Schema;

namespace GLAA.Domain.Models
{
    public class AlternativeBusinessRepresentative : Person, IPerson, IId, ILinkedToLicence
    {        
        [ForeignKey(nameof(LicenceId))]
        public virtual Licence Licence { get; set; }
        
        public int LicenceId { get; set; }
    }
}
