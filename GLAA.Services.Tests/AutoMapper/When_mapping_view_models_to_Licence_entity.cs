using System;
using AutoMapper;
using GLAA.Domain;
using GLAA.Domain.Models;
using GLAA.Services.Automapper;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Services.Tests.AutoMapper
{
    [TestClass]
    public class When_mapping_view_models_to_Licence_entity
    {
        private IMapper mapper;

        [TestInitialize]
        public void Setup()
        {
            var config = new AutoMapperConfig().Configure();

            this.mapper = config.CreateMapper();
        }

        private void AddPersonData(IPersonViewModel inputPerson)
        {
            var expectedAddress = new AddressViewModel
            {
                AddressLine1 = "1",
                AddressLine2 = "2",
                AddressLine3 = "3",
                County = "county",
                CountryId = 1,
                Town = "town",
                Postcode = "postcode",
                Countries = new[] {new SelectListItem {Value = "1", Text = "country"}}
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

            inputPerson.FullName.FullName = expectedFullName;
            inputPerson.AlternativeName.AlternativeName = expectedAltName;
            inputPerson.AlternativeName.HasAlternativeName = expectedHasAltName;
            inputPerson.DateOfBirth = new DateOfBirthViewModel
            {
                DateOfBirth = new DateViewModel
                {
                    Date = expectedDateOfBirth
                }
            };
            inputPerson.TownOfBirth.TownOfBirth = expectedTown;
            inputPerson.CountryOfBirth.CountryOfBirth = expectedCountry;
            inputPerson.JobTitle.JobTitle = expectedJobTitle;
            inputPerson.Address = expectedAddress;
            inputPerson.BusinessExtension.BusinessExtension = expectedBusExt;
            inputPerson.BusinessPhoneNumber.BusinessPhoneNumber = expectedBusPhoneNo;
            inputPerson.PersonalEmailAddress.PersonalEmailAddress = expectedEmail;
            inputPerson.PersonalMobileNumber.PersonalMobileNumber = expectedMobile;
            inputPerson.NationalInsuranceNumber.NationalInsuranceNumber = expectedNatIns;
        }

        private void AssertPerson(IPersonViewModel expected, IPerson actual)
        {
            Assert.AreEqual(expected.FullName.FullName, actual.FullName);
            Assert.AreEqual(expected.AlternativeName.AlternativeName, actual.AlternativeName);
            Assert.AreEqual(expected.AlternativeName.HasAlternativeName, actual.HasAlternativeName);
            Assert.AreEqual(expected.DateOfBirth.DateOfBirth.Date, actual.DateOfBirth);
            Assert.AreEqual(expected.TownOfBirth.TownOfBirth, actual.TownOfBirth);
            Assert.AreEqual(expected.CountryOfBirth.CountryOfBirth, actual.CountryOfBirth);
            Assert.AreEqual(expected.JobTitle.JobTitle, actual.JobTitle);
            Assert.AreEqual(expected.BusinessPhoneNumber.BusinessPhoneNumber, actual.BusinessPhoneNumber);
            Assert.AreEqual(expected.BusinessExtension.BusinessExtension, actual.BusinessExtension);
            Assert.AreEqual(expected.PersonalEmailAddress.PersonalEmailAddress, actual.PersonalEmailAddress);
            Assert.AreEqual(expected.PersonalMobileNumber.PersonalMobileNumber, actual.PersonalMobileNumber);
            Assert.AreEqual(expected.NationalInsuranceNumber.NationalInsuranceNumber, actual.NationalInsuranceNumber);

            AssertAddress(expected.Address, actual.Address);
        }

        private void AssertAddress(Address expected, AddressViewModel actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.AddressLine1, actual.AddressLine1);
            Assert.AreEqual(expected.AddressLine2, actual.AddressLine2);
            Assert.AreEqual(expected.AddressLine3, actual.AddressLine3);
            Assert.AreEqual(expected.CountryId, actual.CountryId);
            Assert.AreEqual(expected.County, actual.County);
            Assert.AreEqual(expected.Postcode, actual.Postcode);
            Assert.AreEqual(expected.Town, actual.Town);
        }

        private void AssertAddress(AddressViewModel expected, Address actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.AddressLine1, actual.AddressLine1);
            Assert.AreEqual(expected.AddressLine2, actual.AddressLine2);
            Assert.AreEqual(expected.AddressLine3, actual.AddressLine3);
            Assert.AreEqual(expected.CountryId, actual.CountryId);
            Assert.AreEqual(expected.County, actual.County);
            Assert.AreEqual(expected.Postcode, actual.Postcode);
            Assert.AreEqual(expected.Town, actual.Town);
        }

        [TestMethod]
        public void it_should_map_the_country_view_model_to_the_country_entity()
        {
            var vm = new WorkerCountryViewModel
            {
                Id = 1,
                Name = "Test Country"
            };

            var result = this.mapper.Map<WorkerCountry>(vm);

            Assert.AreEqual(vm.Id, result.Id);
            Assert.AreEqual(vm.Name, result.Name);
        }

        [TestMethod]
        public void it_should_map_the_industry_view_model_to_the_industry_entity()
        {
            var vm = new IndustryViewModel
            {
                Id = 1,
                Name = "Test Industry"
            };

            var result = this.mapper.Map<Industry>(vm);

            Assert.AreEqual(vm.Id, result.Id);
            Assert.AreEqual(vm.Name, result.Name);
        }

        [TestMethod]
        public void it_should_map_the_sector_view_model_to_the_sector_entity()
        {
            var vm = new SectorViewModel
            {
                Id = 1,
                Name = "Test Sector"
            };

            var result = this.mapper.Map<Sector>(vm);

            Assert.AreEqual(vm.Id, result.Id);
            Assert.AreEqual(vm.Name, result.Name);
        }

        [TestMethod]
        public void it_should_map_the_multiple_entity_to_the_multiple_view_model()
        {
            var vm = new MultipleViewModel
            {
                Id = 1,
                Name = "Test Sector"
            };

            var result = this.mapper.Map<Multiple>(vm);

            Assert.AreEqual(vm.Id, result.Id);
            Assert.AreEqual(vm.Name, result.Name);
        }

        [TestMethod]
        public void it_should_map_the_date_view_model_to_a_datetime()
        {
            var expectedYear = 2001;
            var expectedMonth = 10;
            var expectedDay = 20;
            var date = new DateTime(expectedYear, expectedMonth, expectedDay);

            var vm = new DateViewModel
            {
                Date = date
            };

            var result = this.mapper.Map<DateTime>(vm);

            Assert.AreEqual(expectedYear, result.Year);
            Assert.AreEqual(expectedMonth, result.Month);
            Assert.AreEqual(expectedDay, result.Day);

            Assert.AreEqual(date, result.Date);
        }

        [TestMethod]
        public void it_should_map_the_address_view_model_to_the_address_entity()
        {
            var expectedAddress = new AddressViewModel
            {
                AddressLine1 = "1",
                AddressLine2 = "2",
                AddressLine3 = "3",
                County = "county",
                CountryId = 1,
                Town = "town",
                Postcode = "postcode"
            };

            var result = this.mapper.Map<Address>(expectedAddress);

            AssertAddress(result, expectedAddress);
        }

        [TestMethod]
        public void it_should_map_the_principal_authority_view_model_to_the_principal_authority_entity()
        {
            var expectedIsDirector = true;
            var expectedPreviousExperience = "previous XP";

            var input = new PrincipalAuthorityViewModel();
            AddPersonData(input);

            input.IsDirector.IsDirector = expectedIsDirector;
            input.PreviousExperience.PreviousExperience = expectedPreviousExperience;

            var result = this.mapper.Map<PrincipalAuthority>(input);

            AssertPerson(input, result);

            Assert.AreEqual(input.IsDirector.IsDirector, result.IsDirector);
            Assert.AreEqual(input.PreviousExperience.PreviousExperience, result.PreviousExperience);
        }

        [TestMethod]
        public void it_should_convert_a_length_of_uk_work_for_the_principal_authority_in_months_to_years_and_months()
        {
            var input = new PrincipalAuthorityViewModel();

            input.PrincipalAuthorityRightToWorkViewModel.LengthOfUKWork.Months = 18;
            input.PrincipalAuthorityRightToWorkViewModel.LengthOfUKWork.Years = 1;

            var result = this.mapper.Map<PrincipalAuthority>(input);

            Assert.AreEqual(2, result.LengthOfUKWorkYears);
            Assert.AreEqual(6, result.LengthOfUKWorkMonths);

            input = new PrincipalAuthorityViewModel();

            input.PrincipalAuthorityRightToWorkViewModel.LengthOfUKWork.Months = 18;
            input.PrincipalAuthorityRightToWorkViewModel.LengthOfUKWork.Years = null;

            result = this.mapper.Map<PrincipalAuthority>(input);

            Assert.AreEqual(1, result.LengthOfUKWorkYears);
            Assert.AreEqual(6, result.LengthOfUKWorkMonths);

            input = new PrincipalAuthorityViewModel();

            input.PrincipalAuthorityRightToWorkViewModel.LengthOfUKWork.Months = 6;
            input.PrincipalAuthorityRightToWorkViewModel.LengthOfUKWork.Years = null;

            result = this.mapper.Map<PrincipalAuthority>(input);

            Assert.AreEqual(null, result.LengthOfUKWorkYears);
            Assert.AreEqual(6, result.LengthOfUKWorkMonths);

            input = new PrincipalAuthorityViewModel();

            input.PrincipalAuthorityRightToWorkViewModel.LengthOfUKWork.Months = null;
            input.PrincipalAuthorityRightToWorkViewModel.LengthOfUKWork.Years = 1;

            result = this.mapper.Map<PrincipalAuthority>(input);

            Assert.AreEqual(1, result.LengthOfUKWorkYears);
            Assert.AreEqual(null, result.LengthOfUKWorkMonths);
        }

        [TestMethod]
        public void it_should_map_the_alternative_business_representative_view_model_to_the_alternative_business_representative_entity()
        {
            var input = new AlternativeBusinessRepresentativeViewModel();

            AddPersonData(input);

            var result = this.mapper.Map<AlternativeBusinessRepresentative>(input);

            AssertPerson(input, result);
        }

        [TestMethod]
        public void it_should_map_the_director_or_partner_view_model_to_the_director_or_partner_entity()
        {
            var input = new DirectorOrPartnerViewModel();

            AddPersonData(input);

            var result = this.mapper.Map<DirectorOrPartner>(input);

            AssertPerson(input, result);
        }

        [TestMethod]
        public void it_should_map_the_is_previous_authority_view_model_to_the_director_or_partner_entity()
        {
            var input = new IsPreviousPrincipalAuthorityViewModel { IsPreviousPrincipalAuthority = true };
            var result = new DirectorOrPartner();

            this.mapper.Map(input, result);

            Assert.IsNotNull(result.IsPreviousPrincipalAuthority);
            Assert.AreEqual(true, result.IsPreviousPrincipalAuthority);
        }

        [TestMethod]
        public void it_should_map_the_named_individual_view_model_to_the_named_individual_entity()
        {
            var input = new NamedIndividualViewModel
            {
                FullName = new FullNameViewModel
                {
                    FullName = "Fullname"
                },
                DateOfBirth = new DateOfBirthViewModel
                {
                    DateOfBirth = new DateViewModel
                    {
                        Date = new DateTime(1980, 01, 01)
                    }
                },
                BusinessExtension = new BusinessExtensionViewModel
                {
                    BusinessExtension = "123"
                },
                BusinessPhoneNumber = new BusinessPhoneNumberViewModel
                {
                    BusinessPhoneNumber = "456"
                }
            };

            var result = this.mapper.Map<NamedIndividual>(input);

            Assert.AreEqual(input.FullName.FullName, result.FullName);
            Assert.AreEqual(input.DateOfBirth.DateOfBirth.Date, result.DateOfBirth);
            Assert.AreEqual(input.BusinessExtension.BusinessExtension, result.BusinessExtension);
            Assert.AreEqual(input.BusinessPhoneNumber.BusinessPhoneNumber, result.BusinessPhoneNumber);

        }

        [TestMethod]
        public void it_should_map_the_named_job_title_view_model_to_the_named_job_title_entity()
        {
            var input = new NamedJobTitleViewModel
            {
                JobTitle = "Some job",
                JobTitleNumber = 10
            };

            var result = this.mapper.Map<NamedJobTitle>(input);

            Assert.AreEqual(input.JobTitle, result.JobTitle);
            Assert.AreEqual(input.JobTitleNumber, result.JobTitleNumber);
        }

        [TestMethod]
        public void it_should_map_the_organisation_details_view_model_to_the_licence_entity()
        {
            var expectedAddress = new AddressViewModel
            {
                AddressLine1 = "1",
                AddressLine2 = "2",
                AddressLine3 = "3",
                County = "county",
                CountryId = 1,
                Town = "town",
                Postcode = "postcode"
            };

            var input = new OrganisationDetailsViewModel
            {
                BusinessName = new BusinessNameViewModel
                {
                    BusinessName = "org name",
                    HasTradingName = true,
                    TradingName = "trading name",
                    HasPreviousTradingName = true
                },
                BusinessPhoneNumber = new BusinessPhoneNumberViewModel
                {
                    BusinessPhoneNumber = "1"
                },
                BusinessMobileNumber = new BusinessMobileNumberViewModel
                {
                    BusinessMobileNumber = "2"
                },
                BusinessWebsite = new BusinessWebsiteViewModel
                {
                    BusinessWebsite = "www"
                },
                BusinessEmailAddress = new BusinessEmailAddressViewModel
                {
                    BusinessEmailAddress = "e@mail.com"
                },
                Address = expectedAddress,
                LegalStatus = new LegalStatusViewModel
                {
                    LegalStatus = LegalStatusEnum.LimitedCompany,
                    CompaniesHouseNumber = "6",
                    CompanyRegistrationDate = new DateViewModel
                    {
                        Date = new DateTime(2000, 4, 4)
                    }
                }
            };

            var result = this.mapper.Map<Licence>(input);

            Assert.AreEqual(input.BusinessName.BusinessName, result.BusinessName);
            Assert.AreEqual(input.BusinessName.HasTradingName, result.HasTradingName);
            Assert.AreEqual(input.BusinessName.TradingName, result.TradingName);
            Assert.AreEqual(input.BusinessName.HasPreviousTradingName, result.HasPreviousTradingName);
            Assert.AreEqual(input.BusinessPhoneNumber.BusinessPhoneNumber, result.BusinessPhoneNumber);
            Assert.AreEqual(input.BusinessMobileNumber.BusinessMobileNumber, result.BusinessMobileNumber);
            Assert.AreEqual(input.BusinessWebsite.BusinessWebsite, result.BusinessWebsite);
            Assert.AreEqual(input.BusinessEmailAddress.BusinessEmailAddress, result.BusinessEmailAddress);

            AssertAddress(expectedAddress, result.Address);

            Assert.AreEqual(input.LegalStatus.LegalStatus, result.LegalStatus);
            Assert.AreEqual(input.LegalStatus.CompaniesHouseNumber, result.CompaniesHouseNumber);
            Assert.AreEqual(input.LegalStatus.CompanyRegistrationDate.Date, result.CompanyRegistrationDate);
        }

        [TestMethod]
        public void it_should_map_the_organisation_view_model_to_the_licence_entity()
        {

            var input = new OrganisationViewModel
            {
                OutsideSectorsViewModel = new OutsideSectorsViewModel
                {
                    SuppliesWorkersOutsideLicensableAreas = true,
                    OtherSector = "other"
                },
                WrittenAgreementViewModel = new WrittenAgreementViewModel
                {
                    HasWrittenAgreementsInPlace = true
                },
                PscControlledViewModel = new PSCControlledViewModel
                {
                    IsPSCControlled = true,
                    PSCDetails = "psc deets"
                },
                MultipleBranchViewModel = new MultipleBranchViewModel
                {
                    HasMultiples = true,
                    OtherMultiple = "other multiple",
                    NumberOfMultiples = 10
                }
            };

            var result = this.mapper.Map<Licence>(input);

            Assert.AreEqual(input.OutsideSectorsViewModel.SuppliesWorkersOutsideLicensableAreas, result.SuppliesWorkersOutsideLicensableAreas);
            Assert.AreEqual(input.OutsideSectorsViewModel.OtherSector, result.OtherSector);
            Assert.AreEqual(input.WrittenAgreementViewModel.HasWrittenAgreementsInPlace, result.HasWrittenAgreementsInPlace);
            Assert.AreEqual(input.PscControlledViewModel.IsPSCControlled, result.IsPSCControlled);
            Assert.AreEqual(input.PscControlledViewModel.PSCDetails, result.PSCDetails);
            Assert.AreEqual(input.MultipleBranchViewModel.HasMultiples, result.HasMultiples);
            Assert.AreEqual(input.MultipleBranchViewModel.OtherMultiple, result.OtherMultiple);
            Assert.AreEqual(input.MultipleBranchViewModel.NumberOfMultiples, result.NumberOfMultiples);

        }

        [TestMethod]
        public void it_should_map_the_transporting_workers_view_model_to_the_licence_entity()
        {

            var input = new TransportingWorkersViewModel
            {
                TransportsWorkersToWorkplace = true,
                TransportDeductedFromPay = true,
                TransportWorkersChoose = true
            };

            var result = this.mapper.Map<Licence>(input);

            Assert.AreEqual(input.TransportsWorkersToWorkplace, result.TransportsWorkersToWorkplace);
            Assert.AreEqual(input.TransportDeductedFromPay, result.TransportDeductedFromPay);
            Assert.AreEqual(input.TransportWorkersChoose, result.TransportWorkersChoose);
        }

        [TestMethod]
        public void it_should_map_the_accommodating_workers_view_model_to_the_licence_entity()
        {

            var input = new AccommodatingWorkersViewModel
            {
                AccommodatesWorkers = true,
                AccommodationDeductedFromPay = true,
                AccommodationWorkersChoose = true
            };

            var result = this.mapper.Map<Licence>(input);

            Assert.AreEqual(input.AccommodatesWorkers, result.AccommodatesWorkers);
            Assert.AreEqual(input.AccommodationDeductedFromPay, result.AccommodationDeductedFromPay);
            Assert.AreEqual(input.AccommodationWorkersChoose, result.AccommodationWorkersChoose);
        }

        [TestMethod]
        public void it_should_map_the_declaration_view_model_to_the_licence_entity()
        {
            var input = new DeclarationViewModel
            {
                AgreedToStatementOne = true,
                AgreedToStatementTwo = true,
                AgreedToStatementThree = true,
                AgreedToStatementFour = true,
                AgreedToStatementFive = true,
                AgreedToStatementSix = true,
                SignatoryName = "Demo Signatory",
                SignatureDate = new DateViewModel
                {
                    Date = new DateTime(2017, 1, 1)
                }
            };

            var result = this.mapper.Map<Licence>(input);

            Assert.AreEqual(input.AgreedToStatementOne, result.AgreedToStatementOne);
            Assert.AreEqual(input.AgreedToStatementTwo, result.AgreedToStatementTwo);
            Assert.AreEqual(input.AgreedToStatementThree, result.AgreedToStatementThree);
            Assert.AreEqual(input.AgreedToStatementFour, result.AgreedToStatementFour);
            Assert.AreEqual(input.AgreedToStatementFive, result.AgreedToStatementFive);
            Assert.AreEqual(input.AgreedToStatementSix, result.AgreedToStatementSix);
            Assert.AreEqual(input.SignatoryName, result.SignatoryName);
            Assert.AreEqual(input.SignatureDate.Date, result.SignatureDate);
        }
    }
}
