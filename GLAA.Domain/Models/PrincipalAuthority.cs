using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLAA.Domain.Models
{
    public class PrincipalAuthority : Person, IPerson, IId, ILinkedToLicence
    {        
        public bool IsCurrent { get; set; }

        public bool? IsDirector { get; set; }

        public bool? WillProvideConfirmation { get; set; }

        public string PreviousExperience { get; set; }

        [ForeignKey(nameof(LicenceId))]
        public virtual Licence Licence { get; set; }
        
        public int LicenceId { get; set; }
        
        public virtual DirectorOrPartner DirectorOrPartner { get; set; }

        [ForeignKey(nameof(DirectorOrPartner))]
        public int? DirectorOrPartnerId { get; set; }

        public PermissionToWorkEnum? PermissionToWorkStatus { get; set; }

        public string VisaNumber { get; set; }

        public int? LengthOfUKWorkYears { get; set; }

        public int? LengthOfUKWorkMonths { get; set; }

        public string ImmigrationStatus { get; set; }

        public DateTime? LeaveToRemainTo { get; set; }

        public bool? HasPreviousTradingNames { get; set; }

        public virtual ICollection<PreviousTradingName> PreviousTradingNames { get; set; }
    }
}
