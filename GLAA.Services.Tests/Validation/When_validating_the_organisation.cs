using System;
using System.Collections.Generic;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Services.Tests.Core.Validation
{
    [TestClass]
    public class When_validating_the_organisation
    {
        private OrganisationViewModel model;

        [TestInitialize]
        public void Setup()
        {
            model = new OrganisationViewModel
            {
                AccommodatingWorkersViewModel = new AccommodatingWorkersViewModel
                {
                    AccommodatesWorkers = false
                },
                BannedFromTradingViewModel = new BannedFromTradingViewModel
                {
                    HasBeenBanned = false
                },
                IsShellfish = false,
                MultipleBranchViewModel = new MultipleBranchViewModel
                {
                    HasMultiples = false
                },
                OutsideSectorsViewModel = new OutsideSectorsViewModel
                {
                    SuppliesWorkersOutsideLicensableAreas = false
                },
                PreviouslyWorkedInShellfishViewModel = new PreviouslyWorkedInShellfishViewModel
                {
                    IsShellfish = false
                },
                PscControlledViewModel = new PSCControlledViewModel
                {
                    IsPSCControlled = false
                },
                ShellfishWorkerNationalityViewModel = new ShellfishWorkerNationalityViewModel
                {
                    IsShellfish = false
                },
                ShellfishWorkerNumberViewModel = new ShellfishWorkerNumberViewModel
                {
                    IsShellfish = false
                },
                SourcingWorkersViewModel = new SourcingWorkersViewModel
                {
                    WorkerSource = WorkerSource.EEA
                },
                SubcontractorViewModel = new SubcontractorViewModel
                {
                    UsesSubcontractors = false
                },
                TransportingWorkersViewModel = new TransportingWorkersViewModel
                {
                    TransportsWorkersToWorkplace = false
                },
                WorkerContractViewModel = new WorkerContractViewModel
                {
                    SelectedContract = WorkerContract.ContractForServices
                },
                WorkerEmploymentStatusViewModel = new WorkerEmploymentStatusViewModel
                {
                    SelectedStatuses = new List<CheckboxListItem>
                    {
                        new CheckboxListItem
                        {
                            Id = 1,
                            Checked = true,
                            Name = "Employee"
                        }
                    }
                },
                WrittenAgreementViewModel = new WrittenAgreementViewModel
                {
                    HasWrittenAgreementsInPlace = true
                }
            };
        }

        [TestMethod]
        public void a_blank_model_is_invalid()
        {
            model = new OrganisationViewModel();

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_complete_model_is_valid()
        {
            model.Validate();

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public void a_model_providing_accommodation_and_null_deduction_is_invalid()
        {
            model.AccommodatingWorkersViewModel.AccommodatesWorkers = true;
            model.AccommodatingWorkersViewModel.AccommodationDeductedFromPay = null;
            model.AccommodatingWorkersViewModel.AccommodationWorkersChoose = true;
            model.AccommodatingWorkersViewModel.NumberOfProperties = 1;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_providing_accommodation_and_null_choice_is_invalid()
        {
            model.AccommodatingWorkersViewModel.AccommodatesWorkers = true;
            model.AccommodatingWorkersViewModel.AccommodationDeductedFromPay = true;
            model.AccommodatingWorkersViewModel.AccommodationWorkersChoose = null;
            model.AccommodatingWorkersViewModel.NumberOfProperties = 1;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_providing_accommodation_and_no_number_is_invalid()
        {
            model.AccommodatingWorkersViewModel.AccommodatesWorkers = true;
            model.AccommodatingWorkersViewModel.AccommodationDeductedFromPay = true;
            model.AccommodatingWorkersViewModel.AccommodationWorkersChoose = true;
            model.AccommodatingWorkersViewModel.NumberOfProperties = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_providing_accommodation_is_valid()
        {
            model.AccommodatingWorkersViewModel.AccommodatesWorkers = true;
            model.AccommodatingWorkersViewModel.AccommodationDeductedFromPay = true;
            model.AccommodatingWorkersViewModel.AccommodationWorkersChoose = true;
            model.AccommodatingWorkersViewModel.NumberOfProperties = 1;

            model.Validate();

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_is_banned_from_trading_with_no_description_is_invalid()
        {
            model.BannedFromTradingViewModel.HasBeenBanned = true;
            model.BannedFromTradingViewModel.DateOfBan = new DateViewModel {Date = DateTime.Now};
            model.BannedFromTradingViewModel.BanDescription = string.Empty;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_is_banned_from_trading_with_no_date_is_invalid()
        {
            model.BannedFromTradingViewModel.HasBeenBanned = true;
            model.BannedFromTradingViewModel.BanDescription = "Description";
            model.BannedFromTradingViewModel.DateOfBan = new DateViewModel();

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_is_banned_from_trading_is_valid()
        {
            model.BannedFromTradingViewModel.HasBeenBanned = true;
            model.BannedFromTradingViewModel.BanDescription = "Description";
            model.BannedFromTradingViewModel.DateOfBan = new DateViewModel {Date = DateTime.Now};

            model.Validate();

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_has_multiple_and_no_selected_types_is_invalid()
        {
            model.MultipleBranchViewModel.HasMultiples = true;
            model.MultipleBranchViewModel.NumberOfMultiples = 1;
            model.MultipleBranchViewModel.SelectedMultiples = new List<CheckboxListItem>();

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_has_multiple_and_no_number_is_invalid()
        {
            model.MultipleBranchViewModel.HasMultiples = true;
            model.MultipleBranchViewModel.SelectedMultiples = new List<CheckboxListItem>
            {
                new CheckboxListItem {Id = 1, Name = "Multiple", Checked = true},
                new CheckboxListItem {Id = 2, Name = "Franchise", Checked = false},
                new CheckboxListItem {Id = 3, Name = "Other", Checked = false}
            };
            model.MultipleBranchViewModel.NumberOfMultiples = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_has_an_other_multiple_and_no_description_is_invalid()
        {
            model.MultipleBranchViewModel.HasMultiples = true;
            model.MultipleBranchViewModel.NumberOfMultiples = 1;
            model.MultipleBranchViewModel.SelectedMultiples = new List<CheckboxListItem>
            {
                new CheckboxListItem {Id = 1, Name = "Multiple", Checked = true},
                new CheckboxListItem {Id = 2, Name = "Franchise", Checked = false},
                new CheckboxListItem {Id = 3, Name = "Other", Checked = true}
            };
            model.MultipleBranchViewModel.OtherMultiple = string.Empty;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_has_an_other_multiple_is_valid()
        {
            model.MultipleBranchViewModel.HasMultiples = true;
            model.MultipleBranchViewModel.NumberOfMultiples = 1;
            model.MultipleBranchViewModel.SelectedMultiples = new List<CheckboxListItem>
            {
                new CheckboxListItem {Id = 1, Name = "Multiple", Checked = true},
                new CheckboxListItem {Id = 2, Name = "Franchise", Checked = false},
                new CheckboxListItem {Id = 3, Name = "Other", Checked = true}
            };
            model.MultipleBranchViewModel.OtherMultiple = "desc";

            model.Validate();

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_has_outside_sectors_and_no_selectors_selected_is_invalid()
        {
            model.OutsideSectorsViewModel.SuppliesWorkersOutsideLicensableAreas = true;
            model.OutsideSectorsViewModel.SelectedSectors = new List<CheckboxListItem>();

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_has_other_outside_sector_and_no_description_is_invalid()
        {
            model.OutsideSectorsViewModel.SuppliesWorkersOutsideLicensableAreas = true;
            model.OutsideSectorsViewModel.SelectedSectors = new List<CheckboxListItem>
            {
                new CheckboxListItem {Id = 1, Name = "Agriculture", Checked = false},
                new CheckboxListItem {Id = 2, Name = "Horticulture", Checked = false},
                new CheckboxListItem {Id = 3, Name = "Food Packaging and Processing", Checked = false},
                new CheckboxListItem {Id = 4, Name = "Shellfish Gathering", Checked = false},
                new CheckboxListItem {Id = 5, Name = "Other", Checked = true},
            };
            model.OutsideSectorsViewModel.OtherSector = string.Empty;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_has_outside_sectors_is_valid()
        {
            model.OutsideSectorsViewModel.SuppliesWorkersOutsideLicensableAreas = true;
            model.OutsideSectorsViewModel.SelectedSectors = new List<CheckboxListItem>
            {
                new CheckboxListItem {Id = 1, Name = "Agriculture", Checked = false},
                new CheckboxListItem {Id = 2, Name = "Horticulture", Checked = false},
                new CheckboxListItem {Id = 3, Name = "Food Packaging and Processing", Checked = false},
                new CheckboxListItem {Id = 4, Name = "Shellfish Gathering", Checked = false},
                new CheckboxListItem {Id = 5, Name = "Other", Checked = true},
            };
            model.OutsideSectorsViewModel.OtherSector = "desc";

            model.Validate();

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_is_shellfish_and_has_no_previous_answer_is_invalid()
        {
            model.PreviouslyWorkedInShellfishViewModel.IsShellfish = true;
            model.PreviouslyWorkedInShellfishViewModel.PreviouslyWorkedInShellfish = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_previously_worked_in_shellfish_and_has_no_location_is_invalid()
        {
            model.PreviouslyWorkedInShellfishViewModel.IsShellfish = true;
            model.PreviouslyWorkedInShellfishViewModel.PreviouslyWorkedInShellfish = true;
            model.PreviouslyWorkedInShellfishViewModel.GatheringDate = new DateViewModel {Date = DateTime.Now};
            model.PreviouslyWorkedInShellfishViewModel.GatheringLocation = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_previously_worked_in_shellfish_and_has_no_date_is_invalid()
        {
            model.PreviouslyWorkedInShellfishViewModel.IsShellfish = true;
            model.PreviouslyWorkedInShellfishViewModel.PreviouslyWorkedInShellfish = true;
            model.PreviouslyWorkedInShellfishViewModel.GatheringLocation = "desc";
            model.PreviouslyWorkedInShellfishViewModel.GatheringDate = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_previously_worked_in_shellfish_is_valid()
        {
            model.PreviouslyWorkedInShellfishViewModel.IsShellfish = true;
            model.PreviouslyWorkedInShellfishViewModel.PreviouslyWorkedInShellfish = true;
            model.PreviouslyWorkedInShellfishViewModel.GatheringDate = new DateViewModel { Date = DateTime.Now };
            model.PreviouslyWorkedInShellfishViewModel.GatheringLocation = "Desc";

            model.Validate();

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_is_psc_controlled_and_has_no_description_is_invalid()
        {
            model.PscControlledViewModel.IsPSCControlled = true;
            model.PscControlledViewModel.PSCDetails = string.Empty;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_is_psc_controlled_is_valid()
        {
            model.PscControlledViewModel.IsPSCControlled = true;
            model.PscControlledViewModel.PSCDetails = "details";

            model.Validate();

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_is_shellfish_with_no_nationality_is_invalid()
        {
            model.ShellfishWorkerNationalityViewModel.IsShellfish = true;
            model.ShellfishWorkerNationalityViewModel.NationalityOfWorkers = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_is_shellfish_with_no_number_is_invalid()
        {
            model.ShellfishWorkerNumberViewModel.IsShellfish = true;
            model.ShellfishWorkerNumberViewModel.NumberOfWorkers = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        public void a_model_that_has_shellfish_workers_is_valid()
        {
            model.ShellfishWorkerNumberViewModel.IsShellfish = true;
            model.ShellfishWorkerNumberViewModel.NumberOfWorkers = 1;
            model.ShellfishWorkerNationalityViewModel.IsShellfish = true;
            model.ShellfishWorkerNationalityViewModel.NationalityOfWorkers = "nationality";

            model.Validate();

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        private void a_model_with_no_source_is_invalid()
        {
            model.SourcingWorkersViewModel.WorkerSource = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_uses_subcontractors_with_no_description_is_invalid()
        {
            model.SubcontractorViewModel.UsesSubcontractors = true;
            model.SubcontractorViewModel.SubcontractorNames = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_uses_subcontractors_is_valid()
        {
            model.SubcontractorViewModel.UsesSubcontractors = true;
            model.SubcontractorViewModel.SubcontractorNames = "names";

            model.Validate();

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_uses_transport_and_no_number_is_invalid()
        {
            model.TransportingWorkersViewModel.TransportsWorkersToWorkplace = true;
            model.TransportingWorkersViewModel.NumberOfVehicles = null;
            model.TransportingWorkersViewModel.TransportDeductedFromPay = true;
            model.TransportingWorkersViewModel.TransportWorkersChoose = true;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_uses_transport_and_no_deduction_is_invalid()
        {
            model.TransportingWorkersViewModel.TransportsWorkersToWorkplace = true;
            model.TransportingWorkersViewModel.NumberOfVehicles = 1;
            model.TransportingWorkersViewModel.TransportDeductedFromPay = null;
            model.TransportingWorkersViewModel.TransportWorkersChoose = true;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_uses_transport_and_no_choice_is_invalid()
        {
            model.TransportingWorkersViewModel.TransportsWorkersToWorkplace = true;
            model.TransportingWorkersViewModel.NumberOfVehicles = 1;
            model.TransportingWorkersViewModel.TransportDeductedFromPay = true;
            model.TransportingWorkersViewModel.TransportWorkersChoose = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_uses_transport_is_valid()
        {
            model.TransportingWorkersViewModel.TransportsWorkersToWorkplace = true;
            model.TransportingWorkersViewModel.NumberOfVehicles = 1;
            model.TransportingWorkersViewModel.TransportDeductedFromPay = true;
            model.TransportingWorkersViewModel.TransportWorkersChoose = true;

            model.Validate();

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public void a_model_with_no_contract_is_invalid()
        {
            model.WorkerContractViewModel.SelectedContract = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_with_other_supply_method_and_no_description_is_invalid()
        {
            model.WorkerEmploymentStatusViewModel.SelectedStatuses = new List<CheckboxListItem>
            {
                new CheckboxListItem
                {
                    Id = 5,
                    Checked = true,
                    Name = "Other"
                }
            };
            model.WorkerEmploymentStatusViewModel.OtherEmploymentStatus = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_with_other_supply_method_is_valid()
        {
            model.WorkerEmploymentStatusViewModel.SelectedStatuses = new List<CheckboxListItem>
            {
                new CheckboxListItem
                {
                    Id = 5,
                    Checked = true,
                    Name = "Other"
                }
            };
            model.WorkerEmploymentStatusViewModel.OtherEmploymentStatus = "Other";

            model.Validate();

            Assert.IsTrue(model.IsValid);
        }

        [TestMethod]
        public void a_model_with_no_written_agreement_is_invalid()
        {
            model.WrittenAgreementViewModel.HasWrittenAgreementsInPlace = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }
    }
}
