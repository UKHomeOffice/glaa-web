using System;
using System.Collections.Generic;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Services.Tests.Validation
{
    [TestClass]
    public class When_validating_the_named_individual_collection
    {
        private NamedIndividualViewModel validModel;
        private NamedIndividualViewModel invalidModel;

        private NamedIndividualCollectionViewModel collectionModel;

        [TestInitialize]
        public void Setup()
        {
            collectionModel = new NamedIndividualCollectionViewModel
            {               
                NamedIndividuals = new List<NamedIndividualViewModel>()
            };

            // valid validModel
            validModel = new NamedIndividualViewModel
            {
                FullName = new FullNameViewModel
                {
                    FullName = "name"
                },
                DateOfBirth = new DateOfBirthViewModel
                {
                    DateOfBirth = new DateViewModel
                    {
                        Date = DateTime.Now
                    }
                },
                BusinessPhoneNumber = new BusinessPhoneNumberViewModel
                {
                    BusinessPhoneNumber = "123"
                },
                BusinessExtension = new BusinessExtensionViewModel
                {
                    BusinessExtension = "456"
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

            invalidModel = new NamedIndividualViewModel
            {
                FullName = new FullNameViewModel
                {
                    FullName = null
                },
                DateOfBirth = new DateOfBirthViewModel
                {
                    DateOfBirth = new DateViewModel
                    {
                        Date = DateTime.Now
                    }
                },
                BusinessPhoneNumber = new BusinessPhoneNumberViewModel
                {
                    BusinessPhoneNumber = "123"
                },
                BusinessExtension = new BusinessExtensionViewModel
                {
                    BusinessExtension = "456"
                }
            };
        }

        [TestMethod]
        public void an_empty_collection_is_invalid()
        {
            collectionModel.Validate();

            Assert.AreEqual(false, collectionModel.IsValid);
        }

        [TestMethod]
        public void a_collection_with_one_item_is_valid()
        {
            collectionModel.NamedIndividuals = new List<NamedIndividualViewModel>
            {
                validModel
            };

            collectionModel.Validate();

            Assert.AreEqual(true, collectionModel.IsValid);
        }

        [TestMethod]
        public void a_collection_with_two_items_where_one_is_invalid_is_invalid()
        {
            collectionModel.NamedIndividuals = new List<NamedIndividualViewModel>
            {
                validModel,
                invalidModel
            };

            collectionModel.Validate();

            Assert.AreEqual(false, collectionModel.IsValid);
        }
    }
}
