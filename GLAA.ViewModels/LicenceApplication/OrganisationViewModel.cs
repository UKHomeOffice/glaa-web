using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GLAA.Domain.Models;
using GLAA.ViewModels.Attributes;
using GLAA.ViewModels.Core;
using GLAA.ViewModels.Core.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.LicenceApplication
{
    public class OrganisationViewModel : Validatable, IShellfishSection, IIsSubmitted
    {
        public OrganisationViewModel()
        {
            OutsideSectorsViewModel = new OutsideSectorsViewModel();
            WrittenAgreementViewModel = new WrittenAgreementViewModel();
            PscControlledViewModel = new PSCControlledViewModel();
            MultipleBranchViewModel = new MultipleBranchViewModel();
            TransportingWorkersViewModel = new TransportingWorkersViewModel();
            AccommodatingWorkersViewModel = new AccommodatingWorkersViewModel();
            SourcingWorkersViewModel = new SourcingWorkersViewModel();
            WorkerSupplyMethodViewModel = new WorkerSupplyMethodViewModel();
            WorkerContractViewModel = new WorkerContractViewModel();
            BannedFromTradingViewModel = new BannedFromTradingViewModel();
            SubcontractorViewModel = new SubcontractorViewModel();
            ShellfishWorkerNumberViewModel = new ShellfishWorkerNumberViewModel();
            ShellfishWorkerNationalityViewModel = new ShellfishWorkerNationalityViewModel();
            PreviouslyWorkedInShellfishViewModel = new PreviouslyWorkedInShellfishViewModel();
        }

        public TransportingWorkersViewModel TransportingWorkersViewModel { get; set; }
        public AccommodatingWorkersViewModel AccommodatingWorkersViewModel { get; set; }
        public SourcingWorkersViewModel SourcingWorkersViewModel { get; set; }
        public WorkerSupplyMethodViewModel WorkerSupplyMethodViewModel { get; set; }
        public WorkerContractViewModel WorkerContractViewModel { get; set; }
        public BannedFromTradingViewModel BannedFromTradingViewModel { get; set; }
        public SubcontractorViewModel SubcontractorViewModel { get; set; }
        public OutsideSectorsViewModel OutsideSectorsViewModel { get; set; }
        public WrittenAgreementViewModel WrittenAgreementViewModel { get; set; }
        public PSCControlledViewModel PscControlledViewModel { get; set; }
        public MultipleBranchViewModel MultipleBranchViewModel { get; set; }
        public ShellfishWorkerNumberViewModel ShellfishWorkerNumberViewModel { get; set; }
        public ShellfishWorkerNationalityViewModel ShellfishWorkerNationalityViewModel { get; set; }
        public PreviouslyWorkedInShellfishViewModel PreviouslyWorkedInShellfishViewModel { get; set; }
        public bool IsShellfish { get; set; }
        public bool CanView(OrganisationViewModel parent)
        {
            return true;
        }

        public bool IsSubmitted { get; set; }
    }

    public class ShellfishWorkerNumberViewModel : ShellfishSection
    {
        [RequiredForShellfish]
        [Display(Name = "What is the average number of workers that you will use to gather shellfish")]
        public int? NumberOfWorkers { get; set; }
    }

    public class ShellfishWorkerNationalityViewModel : ShellfishSection
    {
        [RequiredForShellfish]
        [Display(Name = "What are the nationality of the workers that will be gathering shellfish")]
        public string NationalityOfWorkers { get; set; }
    }

    public class PreviouslyWorkedInShellfishViewModel : YesNoViewModel, IShellfishSection, IRequiredIf
    {
        [RequiredForShellfish]
        [Display(Name = "Has your business previously worked in shellfish gathering?")]
        public bool? PreviouslyWorkedInShellfish { get; set; }

        [RequiredIf(ErrorMessage = "You must enter the location in which you have previously gathered shellfish")]
        public string GatheringLocation { get; set; }

        [UIHint("_NullableDateTime")]
        [RequiredIf(ErrorMessage = "You must enter the date when you have previously gathered shellfish")]
        public DateViewModel GatheringDate { get; set; }

        public bool IsShellfish { get; set; }
        public bool CanView(OrganisationViewModel parent)
        {
            return parent.IsShellfish;
        }

        public bool IsRequired => PreviouslyWorkedInShellfish ?? false;
    }

    public class AccommodatingWorkersViewModel : YesNoViewModel, IRequiredIf
    {
        [Required]
        [Display(Name = "Will your workers be provided with accommodation?")]
        public bool? AccommodatesWorkers { get; set; }

        [RequiredIf(ErrorMessage = "You must specify whether you deduct accomodation costs from workers pay")]
        [Display(Name = "Will the charges for accommodation be deducted from workers pay")]
        public bool? AccommodationDeductedFromPay { get; set; }

        [RequiredIf(ErrorMessage = "You must enter the number of properties your workers will be housed in")]
        [Display(Name = "How many properties will your workers be housed in")]
        public int? NumberOfProperties { get; set; }

        [RequiredIf(ErrorMessage = "You must specify whether workers have a choice to use your accomodation")]
        [Display(Name = "Will workers have a choice about using the transport")]
        public bool? AccommodationWorkersChoose { get; set; }

        public bool IsRequired => AccommodatesWorkers ?? false;
    }

    public class TransportingWorkersViewModel : YesNoViewModel, IRequiredIf
    {
        [Required]
        [Display(Name = "Will you be transporting your workers to the workplace")]
        public bool? TransportsWorkersToWorkplace { get; set; }

        [RequiredIf(ErrorMessage = "You must enter the number of vehicles you use to transport workers")]
        [Display(Name = "Number of vehicles")]
        public int? NumberOfVehicles { get; set; }

        [RequiredIf(ErrorMessage = "You must specify whether you deduct transport costs from workers pay")]
        [Display(Name = "Will any charges for transport be deducted from workers pay")]
        public bool? TransportDeductedFromPay { get; set; }

        [RequiredIf(ErrorMessage = "You must specify whether workers have a choice to use your transport")]
        [Display(Name = "Will workers have a choice about using the transport")]
        public bool? TransportWorkersChoose { get; set; }

        public bool IsRequired => TransportsWorkersToWorkplace ?? false;
    }

    public class SourcingWorkersViewModel
    {
        [Required]
        [Display(Name = "Where the organisation intend to source its workers from")]
        public WorkerSource? WorkerSource { get; set; }

        public List<SelectListItem> AvailableSources { get; set; } = new List<SelectListItem>
        {
            new SelectListItem
            {
                Text = "EEA States",
                Value = "EEA"
            },
            new SelectListItem
            {
                Text = "Non-EEA States",
                Value = "NonEEA"
            }
        };
    }

    public class WorkerSupplyMethodViewModel : IRequiredIf
    {
        [Required]
        [Display(Name = "What basis on which workers will be supplied to labour users")]
        public WorkerSupplyMethod? WorkerSupplyMethod { get; set; }

        public List<SelectListItem> AvailableSources { get; set; } = new List<SelectListItem>
        {
            new SelectListItem
            {
                Text = "Employee",
                Value = "Employee"
            },
            new SelectListItem
            {
                Text = "Self Employed",
                Value = "SelfEmployed"
            },
            new SelectListItem
            {
                Text = "Other",
                Value = "Other"
            }
        };

        [RequiredIf(ErrorMessage = "You must enter the basis on which you will supply workers")]
        [Display(Name = "Describe how you will provider workers to labour users")]
        public string WorkerSupplyOther { get; set; }

        public bool IsRequired => WorkerSupplyMethod == Domain.Models.WorkerSupplyMethod.Other;
    }

    public class WorkerContractViewModel
    {
        [Required]
        [Display(Name = "What type of contract do you intend to issue to your workers")]
        public WorkerContract? SelectedContract { get; set; }

        public List<AvailableContract> AvailableContracts { get; set; } = new List<AvailableContract>
        {
            new AvailableContract
            {
                Id = 1,
                Name = "Contract of employment",
                Checked = false,
                EnumMappedTo =  WorkerContract.ContractOfEmployment
            },
            new AvailableContract
            {
                Id = 2,
                Name = "Contract for Services",
                Checked = false,
                EnumMappedTo = WorkerContract.ContractForServices
            },
            new AvailableContract
            {
                Id = 3,
                Name = "None",
                Checked = false,
                EnumMappedTo = WorkerContract.None
            }
        };
    }

    public class BannedFromTradingViewModel : YesNoViewModel, IRequiredIf
    {
        [Required]
        [Display(Name = "Has your organisation ever been banned from trading")]
        public bool? HasBeenBanned { get; set; }

        [RequiredIf(ErrorMessage = "The date of the ban must be provided")]
        [UIHint("_NullableDateTime")]
        [Display(Name = "Date of ban", Description = "For example 31 3 1980")]
        public DateViewModel DateOfBan { get; set; }

        [RequiredIf(ErrorMessage = "The reason for the ban must be provided")]
        [Display(Name = "Description of the reason for the ban")]
        public string BanDescription { get; set; }

        public bool IsRequired => HasBeenBanned ?? false;
    }

    public class SubcontractorViewModel : YesNoViewModel, IRequiredIf
    {
        [Required]
        [Display(Name = "Has the organisation used subcontractors in the last 12 months")]
        public bool? UsesSubcontractors { get; set; }

        [RequiredIf(ErrorMessage = "Subcontractor names are required if you have used them in the last 12 months")]
        public string SubcontractorNames { get; set; }

        public bool IsRequired => UsesSubcontractors ?? false;
    }

    public class OutsideSectorsViewModel : YesNoViewModel, ICollectionViewModel, IRequiredIf
    {
        [Required]
        [Display(Name = "Do you supply workers to industries outside the licensable sectors")]
        public bool? SuppliesWorkersOutsideLicensableAreas { get; set; }

        [CollectionRequiredIf(ErrorMessage = "You must select a sector if you supply workers outside of the valid sectors")]
        public List<CheckboxListItem> SelectedSectors { get; set; }

        public OutsideSectorsViewModel()
        {
            // TODO: The value here should be set from db? existing model?
            SelectedSectors = GetAvailableSectors();
        }

        private List<CheckboxListItem> GetAvailableSectors()
        {
            return new List<CheckboxListItem>
            {
                new CheckboxListItem {Id = 1, Name = "Agriculture", Checked = false},
                new CheckboxListItem {Id = 2, Name = "Horticulture", Checked = false},
                new CheckboxListItem {Id = 3, Name = "Food Packaging and Processing", Checked = false},
                new CheckboxListItem {Id = 4, Name = "Shellfish Gathering", Checked = false},
                new CheckboxListItem {Id = 5, Name = "Other", Checked = false}
            };
        }

        [Display(Name = "Other")]
        [AssertThat("OtherSectorIsValid", ErrorMessage = "The Other Sector field is required")]
        public string OtherSector { get; set; }

        public bool OtherSectorIsValid()
        {
            //TODO: Magic string
            if (SelectedSectors.Any(s => s.Id == 5 && s.Checked))
            {
                return !string.IsNullOrWhiteSpace(OtherSector);
            }
            return true;
        }

        public bool IsRequired => SuppliesWorkersOutsideLicensableAreas ?? false;
    }

    public class WrittenAgreementViewModel : YesNoViewModel
    {
        [Required]
        [Display(Name = "Will the business have a written agreement, to supply workers, with all its customers in the Regulated Sectors")]
        public bool? HasWrittenAgreementsInPlace { get; set; }
    }

    public class PSCControlledViewModel : YesNoViewModel, IRequiredIf
    {
        [Required]
        [Display(Name = "Is the business being significantly controlled by another individual who meets the criteria of People with Significant Control (PSC)")]
        public bool? IsPSCControlled { get; set; }

        [RequiredIf(ErrorMessage = "You must provide details of the PSC")]
        public string PSCDetails { get; set; }

        public bool IsRequired => IsPSCControlled ?? false;
    }

    public class MultipleBranchViewModel : YesNoViewModel, ICollectionViewModel, IRequiredIf
    {

        public MultipleBranchViewModel()
        {
            SelectedMultiples = GetAvailableMultiples();
        }

        [Required]
        [Display(Name = "Do you have multiple branches, franchises, businesses that are ultimately controlled by the applicant business")]
        public bool? HasMultiples { get; set; }

        [CollectionRequiredIf(ErrorMessage = "You must select a multiple type if you have multiples in your organisation")]
        public List<CheckboxListItem> SelectedMultiples { get; set; }

        private List<CheckboxListItem> GetAvailableMultiples()
        {
            return new List<CheckboxListItem>
            {
                new CheckboxListItem {Id = 1, Name = "Multiple", Checked = false},
                new CheckboxListItem {Id = 2, Name = "Franchise", Checked = false},
                new CheckboxListItem {Id = 3, Name = "Other", Checked = false}
            };
        }

        [Display(Name = "Other")]
        [AssertThat("OtherMultipleIsValid", ErrorMessage = "The Other field is required")]//Can't have multiple different conditions for IRequiredIf
        public string OtherMultiple { get; set; }

        [RequiredIf(ErrorMessage = "Number of multiples is required")]
        [Display(Name = "Number of multiples")]
        public int? NumberOfMultiples { get; set; }

        public bool OtherMultipleIsValid()
        {
            if (HasMultiples.HasValue && HasMultiples.Value && SelectedMultiples.Any(s => s.Id == 3 && s.Checked))
            {
                return !string.IsNullOrWhiteSpace(OtherMultiple);
            }
            return true;
        }

        public bool IsRequired => HasMultiples ?? false;
    }
}