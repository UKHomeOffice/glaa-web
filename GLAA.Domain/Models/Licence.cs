using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GLAA.Domain.Core.Models;

namespace GLAA.Domain.Models
{
    public class Licence : IId, IAddressable
    {
        [Key]
        public int Id { get; set; }
        public string ApplicationId { get; set; }
        public bool? AgreedToTermsAndConditions { get; set; }
        public virtual ICollection<LicenceStatusChange> LicenceStatusHistory { get; set; }
        public bool? SuppliesWorkers { get; set; }
        public bool? ContinueApplication { get; set; }
        public bool EmailAlreadyRegistered { get; set; }

        #region Declaration
        
        public bool? AgreedToStatementOne { get; set; }        
        public bool? AgreedToStatementTwo { get; set; }        
        public bool? AgreedToStatementThree { get; set; }        
        public bool? AgreedToStatementFour { get; set; }        
        public bool? AgreedToStatementFive { get; set; }        
        public bool? AgreedToStatementSix { get; set; }        
        public string SignatoryName { get; set; }
        public DateTime? SignatureDate { get; set; }
        #endregion

        #region OrganisationDetails

        public string OrganisationName { get; set; }
        public string TradingName { get; set; }

        public TurnoverBand? TurnoverBand { get; set; }

        public CommunicationPreference? CommunicationPreference { get; set; }

        public LegalStatusEnum? LegalStatus { get; set; }

        public string OtherLegalStatus { get; set; }
        public string BusinessPhoneNumber { get; set; }
        public string BusinessMobileNumber { get; set; }
        public string BusinessEmailAddress { get; set; }
        public string BusinessEmailAddressConfirmation { get; set; }
        public string BusinessWebsite { get; set; }

        public string CompaniesHouseNumber { get; set; }
        public DateTime? CompanyRegistrationDate { get; set; }
        public bool? HasPAYEERNNumber { get; set; }
        public string PAYEERNNumber { get; set; }
        public DateTime? PAYEERNRegistrationDate { get; set; }
        public bool? HasVATNumber { get; set; }
        public string VATNumber { get; set; }
        public DateTime? VATRegistrationDate { get; set; }
        public string TaxReferenceNumber { get; set; }

        public virtual ICollection<LicenceIndustry> OperatingIndustries { get; set; } = new Collection<LicenceIndustry>();

        public string OtherOperatingIndustry { get; set; }

        public virtual ICollection<LicenceCountry> OperatingCountries { get; set; }

        public int? AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public virtual Address Address { get; set; }

        #endregion

        #region Organisation
        public bool? SuppliesWorkersOutsideLicensableAreas { get; set; }
        public virtual ICollection<LicenceSector> SelectedSectors { get; set; }
        public string OtherSector { get; set; }
        public bool? HasWrittenAgreementsInPlace { get; set; }
        public bool? IsPSCControlled { get; set; }
        public string PSCDetails { get; set; }
        public bool? HasMultiples { get; set; }
        public virtual ICollection<LicenceMultiple> SelectedMultiples { get; set; }
        public string OtherMultiple { get; set; }
        public int NumberOfMultiples { get; set; }

        public WorkerSource WorkerSource { get; set; }

        public WorkerSupplyMethod WorkerSupplyMethod { get; set; }

        public string WorkerSupplyOther { get; set; }
        public WorkerContract WorkerContract { get; set; }

        public bool? TransportsWorkersToWorkplace { get; set; }
        public int? NumberOfVehicles { get; set; }
        public bool? TransportDeductedFromPay { get; set; }
        public bool? TransportWorkersChoose { get; set; }

        public bool? AccommodatesWorkers { get; set; }
        public int? NumberOfProperties { get; set; }
        public bool? AccommodationDeductedFromPay { get; set; }
        public bool? AccommodationWorkersChoose { get; set; }

        public bool? HasBeenBanned { get; set; }
        public DateTime? DateOfBan { get; set; }
        public string BanDescription { get; set; }

        public bool? UsesSubcontractors { get; set; }
        public string SubcontractorNames { get; set; }

        // shellfish section
        public bool IsShellfish { get; set; }
        public int? NumberOfShellfishWorkers { get; set; }
        public string NationalityOfShellfishWorkers { get; set; }
        public bool? PreviouslyWorkedInShellfish { get; set; }
        public string GatheringLocation { get; set; }
        public DateTime? GatheringDate { get; set; }

        #endregion

        public bool? HasAlternativeBusinessRepresentatives { get; set; }

        public bool? HasNamedIndividuals { get; set; }

        public int? NumberOfDirectorsOrPartners { get; set; }

        public NamedIndividualType NamedIndividualType { get; set; }

        public virtual GLAAUser User { get; set; }

        public string UserId { get; set; }

        public virtual ICollection<PrincipalAuthority> PrincipalAuthorities { get; set; } = new List<PrincipalAuthority>();

        public virtual ICollection<AlternativeBusinessRepresentative> AlternativeBusinessRepresentatives { get; set; } = new List<AlternativeBusinessRepresentative>();

        public virtual ICollection<DirectorOrPartner> DirectorOrPartners { get; set; } = new List<DirectorOrPartner>();

        public virtual ICollection<NamedIndividual> NamedIndividuals { get; set; } = new List<NamedIndividual>();

        public virtual ICollection<NamedJobTitle> NamedJobTitles { get; set; } = new List<NamedJobTitle>();
    }
}
