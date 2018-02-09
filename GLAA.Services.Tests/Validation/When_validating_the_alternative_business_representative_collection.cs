using System;
using System.Collections.Generic;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Services.Tests.Validation
{
    [TestClass]
    public class When_validating_the_alternative_business_representative_collection
    {
        private AlternativeBusinessRepresentativeViewModel validModel;
        private AlternativeBusinessRepresentativeViewModel invalidModel;

        private AlternativeBusinessRepresentativeCollectionViewModel collectionModel;

        [TestInitialize]
        public void Setup()
        {
            collectionModel = new AlternativeBusinessRepresentativeCollectionViewModel
            {
                HasAlternativeBusinessRepresentatives = false,
                AlternativeBusinessRepresentatives = new List<AlternativeBusinessRepresentativeViewModel>()
            };

            // valid validModel
            validModel = new AlternativeBusinessRepresentativeViewModel
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

            invalidModel = new AlternativeBusinessRepresentativeViewModel
            {
                FullName = new FullNameViewModel
                {
                    FullName = null
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
                }
            };
        }

        [TestMethod]
        public void an_empty_collection_with_no_abrs_is_valid()
        {            
            collectionModel.Validate();

            Assert.AreEqual(true, collectionModel.IsValid);
        }

        [TestMethod]
        public void a_collection_with_one_item_is_valid()
        {
            collectionModel.HasAlternativeBusinessRepresentatives = true;
            collectionModel.AlternativeBusinessRepresentatives = new List<AlternativeBusinessRepresentativeViewModel>
            {
                validModel
            };

            collectionModel.Validate();

            Assert.AreEqual(true, collectionModel.IsValid);
        }

        [TestMethod]
        public void a_collection_with_two_items_is_valid()
        {
            collectionModel.HasAlternativeBusinessRepresentatives = true;
            collectionModel.AlternativeBusinessRepresentatives = new List<AlternativeBusinessRepresentativeViewModel>
            {
                validModel,
                validModel
            };

            collectionModel.Validate();

            Assert.AreEqual(true, collectionModel.IsValid);
        }

        [TestMethod]
        public void a_collection_with_two_items_where_one_is_invalid_is_valid()
        {
            collectionModel.HasAlternativeBusinessRepresentatives = true;
            collectionModel.AlternativeBusinessRepresentatives = new List<AlternativeBusinessRepresentativeViewModel>
            {
                validModel,
                invalidModel
            };

            collectionModel.Validate();

            Assert.AreEqual(false, collectionModel.IsValid);
        }

        [TestMethod]
        public void a_collection_with_three_items_is_invalid()
        {
            collectionModel.HasAlternativeBusinessRepresentatives = true;
            collectionModel.AlternativeBusinessRepresentatives = new List<AlternativeBusinessRepresentativeViewModel>
            {
                validModel,
                validModel,
                validModel
            };

            collectionModel.Validate();

            Assert.AreEqual(false, collectionModel.IsValid);
        }
    }
}
