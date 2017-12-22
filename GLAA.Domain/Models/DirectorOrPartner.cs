using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLAA.Domain.Models
{
    public class DirectorOrPartner : Person, IPerson, IId, ILinkedToLicence
    {
        public bool? IsPreviousPrincipalAuthority { get; set; }

        [ForeignKey(nameof(LicenceId))]
        public virtual Licence Licence { get; set; }

        public int LicenceId { get; set; }
        
        public virtual PrincipalAuthority PrincipalAuthority { get; set; }

        [ForeignKey(nameof(PrincipalAuthority))]
        public int? PrincipalAuthorityId { get; set; }
    }
}
