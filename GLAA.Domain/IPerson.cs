using System;
using System.Collections.Generic;
using GLAA.Domain.Models;

namespace GLAA.Domain
{
    public interface IPerson
    {
        string FullName { get; set; }

        bool? HasAlternativeName { get; set; }

        string AlternativeName { get; set; }

        DateTime? DateOfBirth { get; set; }

        string TownOfBirth { get; set; }

        string CountyOfBirth { get; set; }

        string CountryOfBirth { get; set; }

        string JobTitle { get; set; }

        string BusinessPhoneNumber { get; set; }

        string BusinessExtension { get; set; }

        string MobileNumber { get; set; }

        string EmailAddress { get; set; }

        string NationalInsuranceNumber { get; set; }

        Address Address { get; set; }

        string Nationality { get; set; }

        bool? HasPassport { get; set; }

        bool? RequiresVisa { get; set; }

        string VisaDescription { get; set; }

        bool? IsUndischargedBankrupt { get; set; }

        DateTime? BankruptcyDate { get; set; }

        string BankruptcyNumber { get; set; }

        bool? IsDisqualifiedDirector { get; set; }

        string DisqualificationDetails { get; set; }

        bool? HasRestraintOrders { get; set; }

        ICollection<RestraintOrder> RestraintOrders { get; set; }

        bool? HasUnspentConvictions { get; set; }

        ICollection<Conviction> UnspentConvictions { get; set; }

        bool? HasOffencesAwaitingTrial { get; set; }

        ICollection<OffenceAwaitingTrial> OffencesAwaitingTrial { get; set; }

        bool? HasPreviouslyHeldLicence { get; set; }

        string PreviousLicenceDescription { get; set; }
    }
}
