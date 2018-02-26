using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLAA.Domain.Models
{
    public class NamedIndividual : IId, ILinkedToLicence, IDeletable
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string BusinessPhoneNumber { get; set; }

        public string BusinessExtension { get; set; }

        public int LicenceId { get; set; }

        [ForeignKey(nameof(LicenceId))]
        public virtual Licence Licence { get; set; }

        public bool? RequiresVisa { get; set; }

        public string VisaDescription { get; set; }

        public bool? IsUndischargedBankrupt { get; set; }

        public DateTime? BankruptcyDate { get; set; }

        public string BankruptcyNumber { get; set; }

        public bool? IsDisqualifiedDirector { get; set; }

        public string DisqualificationDetails { get; set; }

        public bool? HasRestraintOrders { get; set; }

        [CascadeDelete]
        public virtual ICollection<RestraintOrder> RestraintOrders { get; set; }

        public bool? HasUnspentConvictions { get; set; }

        [CascadeDelete]
        public virtual ICollection<Conviction> UnspentConvictions { get; set; }

        public bool? HasOffencesAwaitingTrial { get; set; }

        [CascadeDelete]
        public virtual ICollection<OffenceAwaitingTrial> OffencesAwaitingTrial { get; set; }

        public bool? HasPreviouslyHeldLicence { get; set; }

        public string PreviousLicenceDescription { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
