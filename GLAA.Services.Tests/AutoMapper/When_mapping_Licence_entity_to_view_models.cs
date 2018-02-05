using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain;
using GLAA.Domain.Core.Models;
using GLAA.Domain.Models;
using GLAA.Services.Automapper;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Services.Tests.AutoMapper
{
    [TestClass]
    public class When_mapping_entities_to_view_models
    {
        private IMapper mapper;

        [TestInitialize]
        public void Setup()
        {
            var config = new AutoMapperConfig().Configure();

            mapper = config.CreateMapper();
        }

        private void AddPersonData(IPerson inputPerson)
        {
            var expectedAddress = new Address
            {
                AddressLine1 = "1",
                AddressLine2 = "2",
                AddressLine3 = "3",
                County = "county",
                Country = "country",
                Town = "town",
                Postcode = "postcode"
            };

            var expectedFullName = "fullname";
            var expectedAltName = "altname";
            var expectedHasAltName = true;
            var expectedDateOfBirth = new DateTime(2000, 1, 1);
            var expectedTown = "town";
            var expectedCountry = "country";
            var expectedJobTitle = "job";

            var expectedBusPhoneNo = "1";
            var expectedBusExt = "2";
            var expectedEmail = "e@mail.com";
            var expectedMobile = "3";
            var expectedNatIns = "AB4C";

            var expectedNationality = "nationality";
            var expectedHasPassport = true;
            var expectedHasVisa = true;
            var expectedVisaDetails = "details";
            var expectedUndischargedBankrupt = true;
            var expectedBankruptcyDate = new DateTime(2000, 1, 1);
            var expectedBankruptcyNumber = "1234567";
            var expectedIsDisqualifiedDirector = true;
            var expectedDisqualificationDetails = "details";
            var expectedHasRestraintOrders = true;
            var expectedRestraintOrder = new RestraintOrder
            {
                Description = "details",
                Date = new DateTime(2000, 1, 1)
            };
            var expectedHasConvictions = true;
            var expectedConviction = new Conviction
            {
                Description = "details",
                Date = new DateTime(2000, 1, 1)
            };
            var hasOffences = true;
            var expectedOffence = new OffenceAwaitingTrial
            {
                Description = "details",
                Date = new DateTime(2000, 1, 1)
            };
            var expectedHasPreviousLicence = true;
            var expectedPreviousLicenceDetails = "details";

            inputPerson.FullName = expectedFullName;
            inputPerson.AlternativeName = expectedAltName;
            inputPerson.HasAlternativeName = expectedHasAltName;
            inputPerson.DateOfBirth = expectedDateOfBirth;
            inputPerson.TownOfBirth = expectedTown;
            inputPerson.CountryOfBirth = expectedCountry;
            inputPerson.JobTitle = expectedJobTitle;
            inputPerson.Address = expectedAddress;
            inputPerson.BusinessExtension = expectedBusExt;
            inputPerson.BusinessPhoneNumber = expectedBusPhoneNo;
            inputPerson.PersonalEmailAddress = expectedEmail;
            inputPerson.PersonalMobileNumber = expectedMobile;
            inputPerson.NationalInsuranceNumber = expectedNatIns;
            inputPerson.Nationality = expectedNationality;
            inputPerson.HasPassport = expectedHasPassport;
            inputPerson.RequiresVisa = expectedHasVisa;
            inputPerson.VisaDescription = expectedVisaDetails;
            inputPerson.IsUndischargedBankrupt = expectedUndischargedBankrupt;
            inputPerson.BankruptcyNumber = expectedBankruptcyNumber;
            inputPerson.BankruptcyDate = expectedBankruptcyDate;
            inputPerson.IsDisqualifiedDirector = expectedIsDisqualifiedDirector;
            inputPerson.DisqualificationDetails = expectedDisqualificationDetails;
            inputPerson.HasRestraintOrders = expectedHasRestraintOrders;
            inputPerson.RestraintOrders = new List<RestraintOrder> { expectedRestraintOrder };
            inputPerson.HasUnspentConvictions = expectedHasConvictions;
            inputPerson.UnspentConvictions = new List<Conviction> { expectedConviction };
            inputPerson.HasOffencesAwaitingTrial = hasOffences;
            inputPerson.OffencesAwaitingTrial = new List<OffenceAwaitingTrial> { expectedOffence };
            inputPerson.HasPreviouslyHeldLicence = expectedHasPreviousLicence;
            inputPerson.PreviousLicenceDescription = expectedPreviousLicenceDetails;
        }

        private void AssertPerson(IPerson expected, IPersonViewModel actual)
        {
            var expectedYesNoList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Yes"
                },
                new SelectListItem
                {
                    Text = "No"
                }
            };

            Assert.AreEqual(expected.FullName, actual.FullName.FullName);

            Assert.AreEqual(expected.AlternativeName, actual.AlternativeName.AlternativeName);
            Assert.AreEqual(expected.HasAlternativeName, actual.AlternativeName.HasAlternativeName);
            Assert.AreEqual(expectedYesNoList.Count, actual.AlternativeName.YesNo.Count);

            Assert.AreEqual(expected.DateOfBirth, actual.DateOfBirth.DateOfBirth.Date);
            Assert.AreEqual(expected.TownOfBirth, actual.TownOfBirth.TownOfBirth);
            Assert.AreEqual(expected.CountryOfBirth, actual.CountryOfBirth.CountryOfBirth);
            Assert.AreEqual(expected.JobTitle, actual.JobTitle.JobTitle);
            Assert.AreEqual(expected.BusinessPhoneNumber, actual.BusinessPhoneNumber.BusinessPhoneNumber);
            Assert.AreEqual(expected.BusinessExtension, actual.BusinessExtension.BusinessExtension);
            Assert.AreEqual(expected.PersonalEmailAddress, actual.PersonalEmailAddress.PersonalEmailAddress);
            Assert.AreEqual(expected.PersonalMobileNumber, actual.PersonalMobileNumber.PersonalMobileNumber);
            Assert.AreEqual(expected.NationalInsuranceNumber, actual.NationalInsuranceNumber.NationalInsuranceNumber);

            AssertAddress(expected.Address, actual.Address);

            Assert.AreEqual(expected.Nationality, actual.Nationality.Nationality);
            Assert.AreEqual(expected.HasPassport, actual.PassportViewModel.HasPassport);
            Assert.AreEqual(expected.IsUndischargedBankrupt, actual.UndischargedBankruptViewModel.IsUndischargedBankrupt);
            Assert.AreEqual(expected.BankruptcyNumber, actual.UndischargedBankruptViewModel.BankruptcyNumber);
            Assert.AreEqual(expected.BankruptcyDate, actual.UndischargedBankruptViewModel.BankruptcyDate.Date);
            Assert.AreEqual(expected.IsDisqualifiedDirector, actual.DisqualifiedDirectorViewModel.IsDisqualifiedDirector);
            Assert.AreEqual(expected.DisqualificationDetails, actual.DisqualifiedDirectorViewModel.DisqualificationDetails);
            Assert.AreEqual(expected.HasRestraintOrders, actual.RestraintOrdersViewModel.HasRestraintOrders);
            Assert.AreEqual(expected.RestraintOrders.Single().Description, actual.RestraintOrdersViewModel.RestraintOrders.Single().Description);
            Assert.AreEqual(expected.RestraintOrders.Single().Date, actual.RestraintOrdersViewModel.RestraintOrders.Single().Date.Date);
            Assert.AreEqual(expected.HasUnspentConvictions, actual.UnspentConvictionsViewModel.HasUnspentConvictions);
            Assert.AreEqual(expected.UnspentConvictions.Single().Description, actual.UnspentConvictionsViewModel.UnspentConvictions.Single().Description);
            Assert.AreEqual(expected.UnspentConvictions.Single().Date, actual.UnspentConvictionsViewModel.UnspentConvictions.Single().Date.Date);
            Assert.AreEqual(expected.HasOffencesAwaitingTrial, actual.OffencesAwaitingTrialViewModel.HasOffencesAwaitingTrial);
            Assert.AreEqual(expected.OffencesAwaitingTrial.Single().Description, actual.OffencesAwaitingTrialViewModel.OffencesAwaitingTrial.Single().Description);
            Assert.AreEqual(expected.OffencesAwaitingTrial.Single().Date, actual.OffencesAwaitingTrialViewModel.OffencesAwaitingTrial.Single().Date.Date);
            Assert.AreEqual(expected.HasPreviouslyHeldLicence, actual.PreviousLicenceViewModel.HasPreviouslyHeldLicence);
            Assert.AreEqual(expected.PreviousLicenceDescription, actual.PreviousLicenceViewModel.PreviousLicenceDescription);
        }

        private void AssertAddress(Address expected, AddressViewModel actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.AddressLine1, actual.AddressLine1);
            Assert.AreEqual(expected.AddressLine2, actual.AddressLine2);
            Assert.AreEqual(expected.AddressLine3, actual.AddressLine3);
            Assert.AreEqual(expected.Country, actual.Country);
            Assert.AreEqual(expected.County, actual.County);
            Assert.AreEqual(expected.Postcode, actual.Postcode);
            Assert.AreEqual(expected.Town, actual.Town);
        }

        [TestMethod]
        public void it_should_map_the_country_entity_to_the_country_view_model()
        {
            var country = new Country
            {
                Id = 1,
                Name = "Test Country"
            };

            var result = this.mapper.Map<CountryViewModel>(country);

            Assert.AreEqual(country.Id, result.Id);
            Assert.AreEqual(country.Name, result.Name);
        }

        [TestMethod]
        public void it_should_map_the_industry_entity_to_the_industry_view_model()
        {
            var industry = new Industry
            {
                Id = 1,
                Name = "Test Industry"
            };

            var result = this.mapper.Map<IndustryViewModel>(industry);

            Assert.AreEqual(industry.Id, result.Id);
            Assert.AreEqual(industry.Name, result.Name);
        }

        [TestMethod]
        public void it_should_map_the_sector_entity_to_the_sector_view_model()
        {
            var industry = new Sector
            {
                Id = 1,
                Name = "Test Sector"
            };

            var result = this.mapper.Map<SectorViewModel>(industry);

            Assert.AreEqual(industry.Id, result.Id);
            Assert.AreEqual(industry.Name, result.Name);
        }

        [TestMethod]
        public void it_should_map_the_multiple_entity_to_the_multiple_view_model()
        {
            var industry = new Multiple
            {
                Id = 1,
                Name = "Test Sector"
            };

            var result = this.mapper.Map<MultipleViewModel>(industry);

            Assert.AreEqual(industry.Id, result.Id);
            Assert.AreEqual(industry.Name, result.Name);
        }

        [TestMethod]
        public void it_should_map_a_datetime_to_the_datetime_view_model()
        {
            var expectedYear = 2001;
            var expectedMonth = 10;
            var expectedDay = 20;

            var date = new DateTime(expectedYear, expectedMonth, expectedDay);

            var result = this.mapper.Map<DateViewModel>(date);

            Assert.AreEqual(expectedYear, date.Year);
            Assert.AreEqual(expectedMonth, date.Month);
            Assert.AreEqual(expectedDay, date.Day);

            Assert.AreEqual(date, result.Date);
        }

        [TestMethod]
        public void it_should_map_the_address_entity_to_the_address_view_model()
        {
            var expectedAddress = new Address
            {
                Id = 1,
                AddressLine1 = "1",
                AddressLine2 = "2",
                AddressLine3 = "3",
                County = "county",
                Country = "country",
                Town = "town",
                Postcode = "postcode"
            };

            var result = this.mapper.Map<AddressViewModel>(expectedAddress);

            AssertAddress(expectedAddress, result);
        }

        [TestMethod]
        public void it_should_map_the_licence_entity_to_the_business_email_address_view_model()
        {
            var input = new Licence
            {
                Id = 1,
                BusinessEmailAddress = "e@mail.com",
                BusinessEmailAddressConfirmation = "e@mail.com"
            };

            var result = this.mapper.Map<BusinessEmailAddressViewModel>(input);

            Assert.AreEqual(input.BusinessEmailAddress, result.BusinessEmailAddress);
            Assert.AreEqual(input.BusinessEmailAddressConfirmation, result.BusinessEmailAddressConfirmation);
        }

        [TestMethod]
        public void it_should_map_the_principal_authority_entity_to_the_principal_authority_view_model()
        {

            var expectedIsDirector = true;
            var expectedPreviousExperience = "previous XP";
            var expectedHasPreviousTradingNames = true;
            var expectedPreviousTradingName = new PreviousTradingName
            {
                BusinessName = "name",
                Town = "town",
                Country = "country"
            };

            var input = new PrincipalAuthority();
            AddPersonData(input);

            input.IsDirector = expectedIsDirector;
            input.PreviousExperience = expectedPreviousExperience;

            var result = this.mapper.Map<PrincipalAuthorityViewModel>(input);

            AssertPerson(input, result);

            Assert.AreEqual(input.IsDirector, result.IsDirector.IsDirector);
            Assert.AreEqual(input.PreviousExperience, result.PreviousExperience.PreviousExperience);
        }

        [TestMethod]
        public void it_should_map_the_alternative_business_representative_entity_to_the_alternative_business_representative_view_model()
        {
            var input = new AlternativeBusinessRepresentative();

            AddPersonData(input);

            var result = this.mapper.Map<AlternativeBusinessRepresentativeViewModel>(input);

            AssertPerson(input, result);
            Assert.AreEqual(input.RequiresVisa, result.RightToWorkViewModel.RequiresVisa);
            Assert.AreEqual(input.VisaDescription, result.RightToWorkViewModel.VisaDescription);
        }

        [TestMethod]
        public void it_should_map_a_collection_of_alternative_business_representative_entities_to_the_alternative_business_representative_collection_view_model()
        {
            var input = new List<AlternativeBusinessRepresentative>();
            var person = new AlternativeBusinessRepresentative();
            AddPersonData(person);
            input.Add(person);

            var result = this.mapper.Map<AlternativeBusinessRepresentativeCollectionViewModel>(input);

            AssertPerson(person, result.AlternativeBusinessRepresentatives.Single());
            Assert.AreEqual(input.Single().RequiresVisa, result.AlternativeBusinessRepresentatives.Single().RightToWorkViewModel.RequiresVisa);
            Assert.AreEqual(input.Single().VisaDescription, result.AlternativeBusinessRepresentatives.Single().RightToWorkViewModel.VisaDescription);
        }

        [TestMethod]
        public void it_should_map_a_null_alternative_business_representative_entity_to_null()
        {
            var list = new List<AlternativeBusinessRepresentative>();
            var person = new AlternativeBusinessRepresentative();
            AddPersonData(person);
            list.Add(person);

            var result = this.mapper.Map<AlternativeBusinessRepresentativeViewModel>(list.FirstOrDefault(a => a.Id == 1));

            Assert.IsNull(result);
        }

        [TestMethod]
        public void it_should_map_the_director_or_partner_entity_to_the_director_or_partner_view_model()
        {
            var input = new DirectorOrPartner
            {
                IsPreviousPrincipalAuthority = true
            };

            AddPersonData(input);

            var result = this.mapper.Map<DirectorOrPartnerViewModel>(input);

            AssertPerson(input, result);

            Assert.AreEqual(input.IsPreviousPrincipalAuthority, result.IsPreviousPrincipalAuthority.IsPreviousPrincipalAuthority);
            Assert.AreEqual(input.RequiresVisa, result.RightToWorkViewModel.RequiresVisa);
            Assert.AreEqual(input.VisaDescription, result.RightToWorkViewModel.VisaDescription);
        }

        [TestMethod]
        public void it_should_map_director_or_partner_entities_and_principal_authority_entities_to_the_director_or_partner_collection_view_model()
        {
            var dop = new DirectorOrPartner();
            AddPersonData(dop);

            var pa = new PrincipalAuthority();
            AddPersonData(pa);
            pa.IsDirector = true;

            var input = new Licence
            {
                DirectorOrPartners = new List<DirectorOrPartner>
                {
                    dop
                },
                PrincipalAuthorities = new List<PrincipalAuthority>
                {
                    pa
                },
                NumberOfDirectorsOrPartners = 1
            };

            var output = new DirectorOrPartnerCollectionViewModel();
            var result = this.mapper.Map(input, output);

            Assert.AreEqual(1, result.NumberOfDirectorsOrPartners);

            foreach (var dopVm in result.DirectorsOrPartners)
            {
                AssertPerson(dop, dopVm);
                Assert.AreEqual(dop.RequiresVisa, dopVm.RightToWorkViewModel.RequiresVisa);
                Assert.AreEqual(dop.VisaDescription, dopVm.RightToWorkViewModel.VisaDescription);
            }
        }

        [TestMethod]
        public void it_should_not_map_the_principal_authority_entity_to_the_director_or_partner_view_model_if_they_are_not_a_director()
        {
            var pa = new PrincipalAuthority();
            AddPersonData(pa);
            pa.IsDirector = false;

            var input = new Licence
            {
                PrincipalAuthorities = new List<PrincipalAuthority>
                {
                    pa
                },
                NumberOfDirectorsOrPartners = 0
            };

            var output = new DirectorOrPartnerCollectionViewModel();
            var result = this.mapper.Map(input, output);

            Assert.AreEqual(0, result.NumberOfDirectorsOrPartners);
            Assert.IsFalse(result.DirectorsOrPartners.Any());
        }

        [TestMethod]
        public void it_should_map_the_named_individual_entity_to_the_named_individual_view_model()
        {
            var input = new NamedIndividual
            {
                FullName = "Fullname",
                DateOfBirth = new DateTime(1980, 01, 01),
                BusinessExtension = "123",
                BusinessPhoneNumber = "456",
                LicenceId = 1,
                RequiresVisa = true,
                VisaDescription = "description",
                BankruptcyDate = new DateTime(2000, 1, 1),
                BankruptcyNumber = "1234567",
                IsDisqualifiedDirector = true,
                DisqualificationDetails = "details",
                HasRestraintOrders = true,
                RestraintOrders = new[]
                {
                    new RestraintOrder
                    {
                        Description = "details",
                        Date = new DateTime(2000, 1, 1)
                    }
                },
                HasUnspentConvictions = true,
                UnspentConvictions = new[]
                {
                    new Conviction
                    {
                        Description = "details",
                        Date = new DateTime(2000, 1, 1)
                    }
                },
                HasOffencesAwaitingTrial = true,
                OffencesAwaitingTrial = new[]
                {
                    new OffenceAwaitingTrial
                    {
                        Description = "details",
                        Date = new DateTime(2000, 1, 1)
                    }
                },
                HasPreviouslyHeldLicence = true,
                PreviousLicenceDescription = "details"
            };

            var result = this.mapper.Map<NamedIndividualViewModel>(input);

            Assert.AreEqual(input.FullName, result.FullName.FullName);
            Assert.AreEqual(input.DateOfBirth, result.DateOfBirth.DateOfBirth.Date);
            Assert.AreEqual(input.BusinessExtension, result.BusinessExtension.BusinessExtension);
            Assert.AreEqual(input.BusinessPhoneNumber, result.BusinessPhoneNumber.BusinessPhoneNumber);
            Assert.AreEqual(input.RequiresVisa, result.RightToWorkViewModel.RequiresVisa);
            Assert.AreEqual(input.VisaDescription, result.RightToWorkViewModel.VisaDescription);
            Assert.AreEqual(input.IsUndischargedBankrupt, result.UndischargedBankruptViewModel.IsUndischargedBankrupt);
            Assert.AreEqual(input.BankruptcyNumber, result.UndischargedBankruptViewModel.BankruptcyNumber);
            Assert.AreEqual(input.BankruptcyDate, result.UndischargedBankruptViewModel.BankruptcyDate.Date);
            Assert.AreEqual(input.IsDisqualifiedDirector, result.DisqualifiedDirectorViewModel.IsDisqualifiedDirector);
            Assert.AreEqual(input.DisqualificationDetails, result.DisqualifiedDirectorViewModel.DisqualificationDetails);
            Assert.AreEqual(input.HasRestraintOrders, result.RestraintOrdersViewModel.HasRestraintOrders);
            Assert.AreEqual(input.RestraintOrders.Single().Description, result.RestraintOrdersViewModel.RestraintOrders.Single().Description);
            Assert.AreEqual(input.RestraintOrders.Single().Date, result.RestraintOrdersViewModel.RestraintOrders.Single().Date.Date);
            Assert.AreEqual(input.HasUnspentConvictions, result.UnspentConvictionsViewModel.HasUnspentConvictions);
            Assert.AreEqual(input.UnspentConvictions.Single().Description, result.UnspentConvictionsViewModel.UnspentConvictions.Single().Description);
            Assert.AreEqual(input.UnspentConvictions.Single().Date, result.UnspentConvictionsViewModel.UnspentConvictions.Single().Date.Date);
            Assert.AreEqual(input.HasOffencesAwaitingTrial, result.OffencesAwaitingTrialViewModel.HasOffencesAwaitingTrial);
            Assert.AreEqual(input.OffencesAwaitingTrial.Single().Description, result.OffencesAwaitingTrialViewModel.OffencesAwaitingTrial.Single().Description);
            Assert.AreEqual(input.OffencesAwaitingTrial.Single().Date, result.OffencesAwaitingTrialViewModel.OffencesAwaitingTrial.Single().Date.Date);
            Assert.AreEqual(input.HasPreviouslyHeldLicence, result.PreviousLicenceViewModel.HasPreviouslyHeldLicence);
            Assert.AreEqual(input.PreviousLicenceDescription, result.PreviousLicenceViewModel.PreviousLicenceDescription);
        }

        [TestMethod]
        public void it_should_map_the_named_job_title_entity_to_the_named_job_title_view_model()
        {
            var input = new NamedJobTitle
            {
                JobTitle = "My Job",
                JobTitleNumber = 10
            };

            var result = this.mapper.Map<NamedJobTitleViewModel>(input);

            Assert.AreEqual(input.JobTitle, result.JobTitle);
            Assert.AreEqual(input.JobTitleNumber, result.JobTitleNumber);
        }

        [TestMethod]
        public void it_should_map_the_licence_entity_to_the_named_individual_collection_view_model()
        {
            var input = new Licence
            {
                NamedIndividualType = NamedIndividualType.JobTitles,
                NamedJobTitles = new List<NamedJobTitle>
                {
                    new NamedJobTitle
                    {
                        JobTitle = "Job 1",
                        JobTitleNumber = 1
                    }
                },
                NamedIndividuals = new List<NamedIndividual>
                {
                    new NamedIndividual
                    {
                        FullName = "Person 1",
                        BusinessExtension = "123",
                        BusinessPhoneNumber = "01225 123456",
                        DateOfBirth = new DateTime(2000, 1, 1)
                    }
                }
            };

            var result = this.mapper.Map<NamedIndividualCollectionViewModel>(input);

            Assert.AreEqual(input.NamedJobTitles.Count, result.NamedJobTitles.Count());
            Assert.AreEqual(input.NamedIndividualType, result.NamedIndividualType);
            Assert.AreEqual(input.NamedIndividuals.Count, result.NamedIndividuals.Count());
        }

        [TestMethod]
        public void it_should_map_the_licence_entity_industries_and_countries_to_view_model_lists_correctly()
        {
            var licenceId = 1;
            var expectedIndustry = 1;
            var expectedCountry = 3;
            var expectedNumberOfIndustries = 5;
            var expectedNumberOfCountries = 4;

            var input = new Licence
            {
                Id = licenceId,
                OperatingIndustries = new List<LicenceIndustry>
                {
                    new LicenceIndustry
                    {
                        LicenceId = licenceId,
                        IndustryId = expectedIndustry
                    }
                },
                OperatingCountries = new List<LicenceCountry>
                {
                    new LicenceCountry
                    {
                        LicenceId = licenceId,
                        CountryId = expectedCountry
                    }
                }
            };

            var result = this.mapper.Map<OrganisationDetailsViewModel>(input);

            // check the vm got all possible records
            Assert.AreEqual(expectedNumberOfIndustries, result.OperatingIndustries.OperatingIndustries.Count);
            Assert.AreEqual(expectedNumberOfCountries, result.OperatingCountries.OperatingCountries.Count);

            // check that the 'checked' property got set for the right industries
            Assert.AreEqual(true, result.OperatingIndustries.OperatingIndustries.Single(x => x.Id == expectedIndustry).Checked);
            Assert.AreEqual(true, result.OperatingCountries.OperatingCountries.Single(x => x.Id == expectedCountry).Checked);

            // check the checked property is false for any we don't have in the db / licence
            foreach (var item in result.OperatingIndustries.OperatingIndustries.Where(x => x.Id != expectedIndustry))
            {
                Assert.AreEqual(false, item.Checked);
            }

            foreach (var item in result.OperatingCountries.OperatingCountries.Where(x => x.Id != expectedCountry))
            {
                Assert.AreEqual(false, item.Checked);
            }
        }

        [TestMethod]
        public void it_should_map_the_licence_entity_to_the_organisation_details_view_model()
        {
            var expectedAddress = new Address
            {
                AddressLine1 = "1",
                AddressLine2 = "2",
                AddressLine3 = "3",
                County = "county",
                Country = "country",
                Town = "town",
                Postcode = "postcode"
            };

            var input = new Licence
            {
                BusinessName = "org name",
                HasTradingName = true,
                TradingName = "trading name",
                HasPreviousTradingName = true,
                PreviousTradingNames = new[]
                {
                    new PreviousTradingName
                    {
                        BusinessName = "name",
                        Town = "town",
                        Country = "country"
                    }
                },
                BusinessPhoneNumber = "1",
                BusinessMobileNumber = "2",
                BusinessWebsite = "www",
                BusinessEmailAddress = "e@mail",
                BusinessEmailAddressConfirmation = "e@mail",
                Address = expectedAddress,
                PAYEERNNumber = "3",
                PAYEERNRegistrationDate = new DateTime(2000, 2, 2),
                HasPAYEERNNumber = true,
                VATNumber = "4",
                VATRegistrationDate = new DateTime(2000, 3, 3),
                HasVATNumber = true,
                TaxReferenceNumber = "5",
                LegalStatus = LegalStatusEnum.RegisteredCompany,
                CompaniesHouseNumber = "6",
                CompanyRegistrationDate = new DateTime(2000, 4, 4)
            };

            var result = this.mapper.Map<OrganisationDetailsViewModel>(input);

            Assert.AreEqual(input.BusinessName, result.BusinessName.BusinessName);
            Assert.AreEqual(input.TradingName, result.BusinessName.TradingName);
            Assert.AreEqual(input.HasPreviousTradingName, result.BusinessName.HasPreviousTradingName);
            Assert.AreEqual(input.PreviousTradingNames.Single().BusinessName, result.BusinessName.PreviousTradingNames.Single().BusinessName);
            Assert.AreEqual(input.PreviousTradingNames.Single().Town, result.BusinessName.PreviousTradingNames.Single().Town);
            Assert.AreEqual(input.PreviousTradingNames.Single().Country, result.BusinessName.PreviousTradingNames.Single().Country);
            Assert.AreEqual(input.BusinessPhoneNumber, result.BusinessPhoneNumber.BusinessPhoneNumber);
            Assert.AreEqual(input.BusinessMobileNumber, result.BusinessMobileNumber.BusinessMobileNumber);
            Assert.AreEqual(input.BusinessWebsite, result.BusinessWebsite.BusinessWebsite);
            Assert.AreEqual(input.BusinessEmailAddress, result.BusinessEmailAddress.BusinessEmailAddress);
            Assert.AreEqual(input.BusinessEmailAddressConfirmation, result.BusinessEmailAddress.BusinessEmailAddressConfirmation);
            Assert.AreEqual(input.TaxReferenceNumber, result.TaxReference.TaxReferenceNumber);

            AssertAddress(expectedAddress, result.Address);

            Assert.AreEqual(input.PAYEERNNumber, result.PAYEERNStatus.PAYENumber);
            Assert.AreEqual(input.PAYEERNRegistrationDate, result.PAYEERNStatus.PAYERegistrationDate.Date);
            Assert.AreEqual(input.HasPAYEERNNumber, result.PAYEERNStatus.HasPAYENumber);

            Assert.AreEqual(input.VATNumber, result.VATStatus.VATNumber);
            Assert.AreEqual(input.VATRegistrationDate, result.VATStatus.VATRegistrationDate.Date);
            Assert.AreEqual(input.HasVATNumber, result.VATStatus.HasVATNumber);

            Assert.AreEqual(input.LegalStatus, result.LegalStatus.LegalStatus);
            //Assert.AreEqual(input.CompaniesHouseNumber, result.LegalStatus.CompaniesHouseNumber);
            //Assert.AreEqual(input.CompanyRegistrationDate, result.LegalStatus.CompanyRegistrationDate.Date);
        }

        [TestMethod]
        public void it_should_map_the_sector_and_multiples_to_view_model_lists_correctly()
        {
            var licenceId = 1;
            var expectedSector = 1;
            var expectedMultiple = 2;
            var expectedNumberOfSectors = 5;
            var expectedNumberOfMultiples = 3;

            var input = new Licence
            {
                IsShellfish = true,
                SelectedSectors = new List<LicenceSector>
                {
                    new LicenceSector
                    {
                        LicenceId = licenceId,
                        SectorId = expectedSector,
                        Sector = new Sector
                        {
                            Id = expectedSector
                        }
                    }
                },
                SelectedMultiples = new List<LicenceMultiple>
                {
                    new LicenceMultiple
                    {
                        LicenceId = licenceId,
                        MultipleId = expectedMultiple,
                        Multiple = new Multiple
                        {
                            Id = expectedMultiple
                        }
                    }
                }
            };

            var result = this.mapper.Map<OrganisationViewModel>(input);

            // check the vm got all 4 possible records
            Assert.AreEqual(expectedNumberOfSectors, result.OutsideSectorsViewModel.SelectedSectors.Count);
            Assert.AreEqual(expectedNumberOfMultiples, result.MultipleBranchViewModel.SelectedMultiples.Count);

            // check that the 'checked' property got set for the right industries
            Assert.AreEqual(true, result.OutsideSectorsViewModel.SelectedSectors.Single(x => x.Id == expectedSector).Checked);
            Assert.AreEqual(true, result.MultipleBranchViewModel.SelectedMultiples.Single(x => x.Id == expectedMultiple).Checked);

            // check the checked property is false for any we don't have in the db / licence
            foreach (var item in result.OutsideSectorsViewModel.SelectedSectors.Where(x => x.Id != expectedSector))
            {
                Assert.AreEqual(false, item.Checked);
            }

            foreach (var item in result.MultipleBranchViewModel.SelectedMultiples.Where(x => x.Id != expectedMultiple))
            {
                Assert.AreEqual(false, item.Checked);
            }
        }

        [TestMethod]
        public void it_should_map_the_licence_entity_to_the_organisation_view_model()
        {
            var input = new Licence
            {
                SuppliesWorkersOutsideLicensableAreas = true,
                OtherSector = "other",
                HasWrittenAgreementsInPlace = true,
                IsPSCControlled = true,
                PSCDetails = "psc deets",
                HasMultiples = true,
                OtherMultiple = "other multiple",
                NumberOfMultiples = 10,
                IsShellfish = true
            };

            var result = this.mapper.Map<OrganisationViewModel>(input);

            Assert.AreEqual(input.SuppliesWorkersOutsideLicensableAreas, result.OutsideSectorsViewModel.SuppliesWorkersOutsideLicensableAreas);
            Assert.AreEqual(input.OtherSector, result.OutsideSectorsViewModel.OtherSector);
            Assert.AreEqual(input.HasWrittenAgreementsInPlace, result.WrittenAgreementViewModel.HasWrittenAgreementsInPlace);
            Assert.AreEqual(input.IsPSCControlled, result.PscControlledViewModel.IsPSCControlled);
            Assert.AreEqual(input.PSCDetails, result.PscControlledViewModel.PSCDetails);
            Assert.AreEqual(input.HasMultiples, result.MultipleBranchViewModel.HasMultiples);
            Assert.AreEqual(input.OtherMultiple, result.MultipleBranchViewModel.OtherMultiple);
            Assert.AreEqual(input.NumberOfMultiples, result.MultipleBranchViewModel.NumberOfMultiples);
        }

        [TestMethod]
        public void it_should_map_the_licence_entity_to_the_eligibility_view_model()
        {
            var selectedIndustry = 1;
            var licenceId = 1;
            var input = new Licence
            {
                Id = licenceId,
                TurnoverBand = TurnoverBand.OneToFiveMillion,
                SuppliesWorkers = true,
                OperatingIndustries = new List<LicenceIndustry>
                {
                    new LicenceIndustry
                    {
                        LicenceId = licenceId,
                        IndustryId = selectedIndustry,
                        Industry = new Industry
                        {
                            Id = selectedIndustry,
                            Name = "An industry"
                        }
                    }                    
                }
            };

            var result = this.mapper.Map<EligibilityViewModel>(input);

            Assert.AreEqual(input.TurnoverBand, result.Turnover.TurnoverBand);
            Assert.AreEqual(input.SuppliesWorkers, result.SuppliesWorkers.SuppliesWorkers);
            Assert.AreEqual(true, result.OperatingIndustries.OperatingIndustries.Single(x => x.Id == selectedIndustry).Checked);
            Assert.AreEqual(input.ContinueApplication, result.EligibilitySummary.ContinueApplication);
        }

        [TestMethod]
        public void it_should_map_the_licence_entity_to_the_declaration_view_model()
        {
            var input = new Licence
            {
                AgreedToStatementOne = true,
                AgreedToStatementTwo = true,
                AgreedToStatementThree = true,
                AgreedToStatementFour = true,
                AgreedToStatementFive = true,
                AgreedToStatementSix = true,
                SignatoryName = "Signatory Name",
                SignatureDate = new DateTime(2017, 4, 1)
            };

            var result = this.mapper.Map<DeclarationViewModel>(input);

            Assert.AreEqual(input.AgreedToStatementOne, result.AgreedToStatementOne);
            Assert.AreEqual(input.AgreedToStatementTwo, result.AgreedToStatementTwo);
            Assert.AreEqual(input.AgreedToStatementThree, result.AgreedToStatementThree);
            Assert.AreEqual(input.AgreedToStatementFour, result.AgreedToStatementFour);
            Assert.AreEqual(input.AgreedToStatementFive, result.AgreedToStatementFive);
            Assert.AreEqual(input.AgreedToStatementSix, result.AgreedToStatementSix);
            Assert.AreEqual(input.SignatoryName, result.SignatoryName);
            Assert.AreEqual(input.SignatureDate, result.SignatureDate.Date);
        }
    }
}
