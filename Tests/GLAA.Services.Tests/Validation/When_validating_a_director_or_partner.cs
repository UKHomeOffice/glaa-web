﻿using System;
using System.Collections.Generic;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Services.Tests.Validation
{
    [TestClass]
    public class When_validating_a_director_or_partner
    {
        private DirectorOrPartnerViewModel model;

        [TestInitialize]
        public void Setup()
        {
            // valid model
            model = new DirectorOrPartnerViewModel
            {
                IsPreviousPrincipalAuthority = new IsPreviousPrincipalAuthorityViewModel
                {
                    IsPreviousPrincipalAuthority = true
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
                BirthDetailsViewModel = new BirthDetailsViewModel
                {
                    TownOfBirthViewModel = new TownOfBirthViewModel
                    {
                        TownOfBirth = "town"
                    },
                    CountryOfBirthViewModel = new CountryOfBirthViewModel
                    {
                        CountryOfBirthId = 1
                    },
                    NationalInsuranceNumberViewModel = new NationalInsuranceNumberViewModel
                    {
                        NationalInsuranceNumber = "JT123456A",
                        IsUk = true // This property it mapped via automapper in real life
                    }
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
                    CountyId = 1,
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
                Nationality = new NationalityViewModel
                {
                    Nationality = "British"
                },
                PassportViewModel = new PassportViewModel
                {
                    HasPassport = true
                },
                RightToWorkViewModel = new RightToWorkViewModel
                {
                    RequiresVisa = true,
                    VisaDescription = "description"
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
            var vm = new AlternativeBusinessRepresentativeViewModel();

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
    }
}
