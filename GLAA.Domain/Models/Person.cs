using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLAA.Domain.Models
{
    public class Person : IAddressable
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }

        public bool? HasAlternativeName { get; set; }

        public string AlternativeName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string TownOfBirth { get; set; }

        public string CountyOfBirth { get; set; }

        public int? CountryOfBirthId { get; set; }
        [ForeignKey(nameof(CountryOfBirthId))]
        public virtual Country CountryOfBirth { get; set; }

        public string JobTitle { get; set; }

        public string BusinessPhoneNumber { get; set; }

        public string BusinessExtension { get; set; }

        public string PersonalMobileNumber { get; set; }

        public string PersonalEmailAddress { get; set; }

        public string NationalInsuranceNumber { get; set; }

        [ForeignKey(nameof(AddressId))] 
        public virtual Address Address { get; set; }

        public int? AddressId { get; set; }

        public string Nationality { get; set; }

        public bool? HasPassport { get; set; }

        public bool? RequiresVisa { get; set; }

        public string VisaDescription { get; set; }

        public bool? IsUndischargedBankrupt { get; set; }

        public DateTime? BankruptcyDate { get; set; }

        public string BankruptcyNumber { get; set; }

        public bool? IsDisqualifiedDirector { get; set; }

        public string DisqualificationDetails { get; set; }

        public bool? HasRestraintOrders { get; set; }

        public virtual ICollection<RestraintOrder> RestraintOrders { get; set; }

        public bool? HasUnspentConvictions { get; set; }

        public virtual ICollection<Conviction> UnspentConvictions { get; set; }

        public bool? HasOffencesAwaitingTrial { get; set; }

        public virtual ICollection<OffenceAwaitingTrial> OffencesAwaitingTrial { get; set; }

        public bool? HasPreviouslyHeldLicence { get; set; }

        public string PreviousLicenceDescription { get; set; }
    }
}
