using System;
using System.Text;
using System.Collections.Generic;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Services.Tests.Validation
{
    [TestClass]
    public class When_validating_a_director_or_partner_collection
    {
        private DirectorOrPartnerViewModel validModel;

        [TestInitialize]
        public void Setup()
        {
            validModel = new DirectorOrPartnerViewModel
            {
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
                NationalInsuranceNumber = new NationalInsuranceNumberViewModel
                {
                    NationalInsuranceNumber = "JT123456A",
                    IsUk = true // This property it mapped via automapper in real life
                },
                IsPreviousPrincipalAuthority = new IsPreviousPrincipalAuthorityViewModel
                {
                    IsPreviousPrincipalAuthority = false
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
            var vm = new DirectorOrPartnerCollectionViewModel();

            vm.Validate();

            Assert.IsFalse(vm.IsValid);
        }

        [TestMethod]
        public void a_model_with_no_count_is_invalid()
        {
            var vm = new DirectorOrPartnerCollectionViewModel
            {
                NumberOfDirectorsOrPartners = null,
                DirectorsOrPartners = new[] {validModel}
            };

            vm.Validate();

            Assert.IsFalse(vm.IsValid);
        }

        [TestMethod]
        public void a_model_with_required_directors_and_no_count_is_invalid()
        {
            var vm = new DirectorOrPartnerCollectionViewModel
            {
                DirectorsRequired = true,
                NumberOfDirectorsOrPartners = null,
                DirectorsOrPartners = new[] {validModel}
            };

            vm.Validate();

            Assert.IsFalse(vm.IsValid);
        }

        [TestMethod]
        public void a_model_with_required_directors_and_no_directors_is_invalid()
        {
            var vm = new DirectorOrPartnerCollectionViewModel
            {
                DirectorsRequired = true,
                NumberOfDirectorsOrPartners = 1,
                DirectorsOrPartners = new DirectorOrPartnerViewModel[0]
            };

            vm.Validate();

            Assert.IsFalse(vm.IsValid);
        }

        [TestMethod]
        public void a_model_without_required_directors_and_no_count_is_invalid()
        {
            var vm = new DirectorOrPartnerCollectionViewModel
            {
                DirectorsRequired = false,
                NumberOfDirectorsOrPartners = null,
                DirectorsOrPartners = new[] {validModel}
            };

            vm.Validate();

            Assert.IsFalse(vm.IsValid);
        }

        [TestMethod]
        public void a_model_with_a_mismatched_number_of_directors_is_invalid()
        {
            var vm = new DirectorOrPartnerCollectionViewModel
            {
                DirectorsRequired = false,
                NumberOfDirectorsOrPartners = 0,
                DirectorsOrPartners = new[] {validModel}
            };

            vm.Validate();

            Assert.IsFalse(vm.IsValid);
        }

        [TestMethod]
        public void a_model_with_more_than_one_principal_authority_is_invalid()
        {
            validModel.IsPreviousPrincipalAuthority.IsPreviousPrincipalAuthority = true;
            var vm = new DirectorOrPartnerCollectionViewModel
            {
                DirectorsRequired = false,
                NumberOfDirectorsOrPartners = 2,
                DirectorsOrPartners = new[] {validModel, validModel}
            };

            vm.Validate();

            Assert.IsFalse(vm.IsValid);
        }

        [TestMethod]
        public void a_model_with_required_directors_and_with_a_matching_count_and_incomplete_directors_is_invalid()
        {
            validModel.FullName.FullName = null;
            var vm = new DirectorOrPartnerCollectionViewModel
            {
                DirectorsRequired = true,
                NumberOfDirectorsOrPartners = 1,
                DirectorsOrPartners = new[] {validModel}
            };

            vm.Validate();

            Assert.IsFalse(vm.IsValid);
        }

        [TestMethod]
        public void a_model_without_required_directors_and_with_a_matching_count_and_incomplete_directors_is_invalid()
        {
            validModel.FullName.FullName = null;
            var vm = new DirectorOrPartnerCollectionViewModel
            {
                DirectorsRequired = false,
                NumberOfDirectorsOrPartners = 1,
                DirectorsOrPartners = new[] {validModel}
            };

            vm.Validate();

            Assert.IsFalse(vm.IsValid);
        }

        [TestMethod]
        public void a_model_without_required_directors_and_with_a_count_is_valid()
        {
            var vm = new DirectorOrPartnerCollectionViewModel
            {
                DirectorsRequired = false,
                NumberOfDirectorsOrPartners = 0,
                DirectorsOrPartners = new DirectorOrPartnerViewModel[0]
            };

            vm.Validate();

            Assert.IsTrue(vm.IsValid);
        }

        [TestMethod]
        public void a_model_without_required_directors_and_with_a_matching_count_is_valid()
        {
            var vm = new DirectorOrPartnerCollectionViewModel
            {
                DirectorsRequired = false,
                NumberOfDirectorsOrPartners = 1,
                DirectorsOrPartners = new[] { validModel }
            };

            vm.Validate();

            Assert.IsTrue(vm.IsValid);
        }

        [TestMethod]
        public void a_model_with_required_directors_and_with_a_matching_count_is_valid()
        {
            var vm = new DirectorOrPartnerCollectionViewModel
            {
                DirectorsRequired = true,
                NumberOfDirectorsOrPartners = 1,
                DirectorsOrPartners = new[] { validModel }
            };

            vm.Validate();

            Assert.IsTrue(vm.IsValid);
        }
    }
}
