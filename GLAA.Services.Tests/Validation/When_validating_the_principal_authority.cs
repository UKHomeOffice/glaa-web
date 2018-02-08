using System;
using System.Collections.Generic;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Services.Tests.Validation
{
    [TestClass]
    public class When_validating_the_principal_authority
    {
        private PrincipalAuthorityViewModel model;

        [TestInitialize]
        public void Setup()
        {
            // valid model
            model = new PrincipalAuthorityViewModel
            {
                IsDirector = new IsDirectorViewModel
                {
                    IsDirector = false
                },
                PreviousExperience = new PreviousExperienceViewModel
                {
                    PreviousExperience = "xp"
                },
                FullName = new FullNameViewModel
                {
                    FullName = "name"
                },
                AlternativeName = new AlternativeFullNameViewModel
                {
                    HasAlternativeName = false
                },
                DateOfBirth = new DateOfBirthViewModel
                {
                    DateOfBirth = new DateViewModel
                    {
                        Date = DateTime.Now
                    }
                },
                TownOfBirth = new TownOfBirthViewModel
                {
                    TownOfBirth = "town"
                },
                CountryOfBirth = new CountryOfBirthViewModel
                {
                    CountryOfBirth = "country"
                },
                JobTitle = new JobTitleViewModel
                {
                    JobTitle = "job"
                },
                Address = new AddressViewModel
                {
                    AddressLine1 = "1",
                    AddressLine2 = "2",
                    Town = "Perutown",
                    County = "County",
                    CountryId = 1,
                    Postcode = "postcode",
                    NonUK = false
                },
                BusinessPhoneNumber = new BusinessPhoneNumberViewModel
                {
                    BusinessPhoneNumber = "123"
                },
                BusinessExtension = new BusinessExtensionViewModel
                {
                    BusinessExtension = "456"
                },
                NationalInsuranceNumber = new NationalInsuranceNumberViewModel
                {
                    NationalInsuranceNumber = "JT123456A",
                    IsUk = true // This property it mapped via automapper in real life
                },
                PrincipalAuthorityConfirmation = new PrincipalAuthorityConfirmationViewModel
                {
                    WillProvideConfirmation = true
                },
                Nationality = new NationalityViewModel
                {
                    Nationality = "British"
                },
                PassportViewModel = new PassportViewModel
                {
                    HasPassport = true
                },
                PrincipalAuthorityRightToWorkViewModel = new PrincipalAuthorityRightToWorkViewModel
                {
                    RightToWorkInUk = PermissionToWorkEnum.HasVisa,
                    VisaNumber = "12341234",
                    ImmigrationStatus = "Status",
                    LeaveToRemainTo = new DateViewModel { Date = DateTime.Now },
                    LengthOfUKWork = new TimeSpanViewModel
                    {
                        Months = 1,
                        Years = 1
                    }
                },
                UndischargedBankruptViewModel = new UndischargedBankruptViewModel
                {
                    IsUndischargedBankrupt = true,
                    BankruptcyDate = new DateViewModel
                    {
                        Date = DateTime.Now
                    },
                    BankruptcyNumber = "1234567"
                },
                DisqualifiedDirectorViewModel = new DisqualifiedDirectorViewModel
                {
                    IsDisqualifiedDirector = true,
                    DisqualificationDetails = "Details"
                },
                RestraintOrdersViewModel = new RestraintOrdersViewModel
                {
                    HasRestraintOrders = true,
                    RestraintOrders = new[]
                    {
                        new RestraintOrderViewModel
                        {
                            Date = new DateViewModel {Date = DateTime.Now},
                            Description = "description"
                        }
                    }
                },
                UnspentConvictionsViewModel = new UnspentConvictionsViewModel
                {
                    HasUnspentConvictions = true,
                    UnspentConvictions = new[]
                    {
                        new UnspentConvictionViewModel
                        {
                            Date = new DateViewModel {Date = DateTime.Now},
                            Description = "description"
                        }
                    }
                },
                OffencesAwaitingTrialViewModel = new OffencesAwaitingTrialViewModel
                {
                    HasOffencesAwaitingTrial = true,
                    OffencesAwaitingTrial = new[]
                    {
                        new OffenceAwaitingTrialViewModel
                        {
                            Date = new DateViewModel {Date = DateTime.Now},
                            Description = "description"
                        }
                    }
                },
                PreviousLicenceViewModel = new PreviousLicenceViewModel
                {
                    HasPreviouslyHeldLicence = true,
                    PreviousLicenceDescription = "description"
                }
            };
        }

        [TestMethod]
        public void a_blank_model_is_invalid()
        {
            var vm = new PrincipalAuthorityViewModel();

            vm.Validate();

            Assert.AreEqual(false, vm.IsValid);
        }

        [TestMethod]
        public void a_complete_model_is_valid()
        {
            model.Validate();

            Assert.AreEqual(true, model.IsValid);
        }

        [TestMethod]
        public void a_model_with_a_missing_required_property_is_invalid()
        {
            model.FullName.FullName = null;

            model.Validate();

            Assert.AreEqual(false, model.IsValid);
        }

        [TestMethod]
        public void a_model_with_no_date_of_birth_is_invalid()
        {
            model.DateOfBirth.DateOfBirth = new DateViewModel();

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_with_a_uk_address_and_no_national_insurance_number_is_invalid()
        {
            // in this test, we're not checking the automapping of the address.NonUk > ni_number.IsUk
            // it's shown here for info
            model.Address.NonUK = false;
            model.NationalInsuranceNumber = new NationalInsuranceNumberViewModel
            {
                IsUk = true,
                NationalInsuranceNumber = null
            };

            model.Validate();

            Assert.AreEqual(false, model.IsValid);
        }

        [TestMethod]
        public void a_model_with_a_visa_and_no_visa_number_is_invalid()
        {
            model.PrincipalAuthorityRightToWorkViewModel.VisaNumber = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_with_a_visa_and_no_immigration_status_is_invalid()
        {
            model.PrincipalAuthorityRightToWorkViewModel.ImmigrationStatus = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_with_a_visa_and_no_leave_to_reamin_date_is_invalid()
        {
            model.PrincipalAuthorityRightToWorkViewModel.LeaveToRemainTo = new DateViewModel();

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_with_a_visa_and_no_length_of_uk_work_is_invalid()
        {
            model.PrincipalAuthorityRightToWorkViewModel.LengthOfUKWork = new TimeSpanViewModel();

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_is_an_undischarged_bankrupt_and_no_bankruptcy_date_is_invalid()
        {
            model.UndischargedBankruptViewModel.BankruptcyDate = new DateViewModel();

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_is_an_undischarged_bankrupt_and_no_bankruptcy_number_is_invalid()
        {
            model.UndischargedBankruptViewModel.BankruptcyNumber = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_is_an_undischarged_bankrupt_and_an_invalid_bankruptcy_number_is_invalid()
        {
            model.UndischargedBankruptViewModel.BankruptcyNumber = "Invalid";

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_is_a_disqualified_director_and_no_details_is_invalid()
        {
            model.DisqualifiedDirectorViewModel.DisqualificationDetails = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_has_restraint_orders_but_no_details_is_invalid()
        {
            model.RestraintOrdersViewModel.HasRestraintOrders = true;
            model.RestraintOrdersViewModel.RestraintOrders = new List<RestraintOrderViewModel>();

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_has_convictions_but_no_details_is_invalid()
        {
            model.UnspentConvictionsViewModel.HasUnspentConvictions = true;
            model.UnspentConvictionsViewModel.UnspentConvictions = new List<UnspentConvictionViewModel>();

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_has_offences_but_no_details_is_invalid()
        {
            model.OffencesAwaitingTrialViewModel.HasOffencesAwaitingTrial = true;
            model.OffencesAwaitingTrialViewModel.OffencesAwaitingTrial = new List<OffenceAwaitingTrialViewModel>();

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_that_has_previous_licences_but_no_details_is_invalid()
        {
            model.PreviousLicenceViewModel.HasPreviouslyHeldLicence = true;
            model.PreviousLicenceViewModel.PreviousLicenceDescription = null;

            model.Validate();

            Assert.IsFalse(model.IsValid);
        }

        [TestMethod]
        public void a_model_with_a_uk_address_and_a_national_insurance_number_is_valid()
        {
            // in this test, we're not checking the automapping of the address.NonUk > ni_number.IsUk
            // it's shown here for info
            model.Address.NonUK = false;
            model.NationalInsuranceNumber = new NationalInsuranceNumberViewModel
            {
                IsUk = true,
                NationalInsuranceNumber = "JT123456A"
            };

            model.Validate();

            Assert.AreEqual(true, model.IsValid);
        }

        [TestMethod]
        public void a_model_with_a_non_uk_address_and_no_national_insurance_number_is_valid()
        {
            // in this test, we're not checking the automapping of the address.NonUk > ni_number.IsUk
            // it's shown here for info
            model.Address.NonUK = true;
            model.NationalInsuranceNumber = new NationalInsuranceNumberViewModel
            {
                IsUk = false,
                NationalInsuranceNumber = null
            };

            model.Validate();

            Assert.AreEqual(true, model.IsValid);
        }
    }
}
