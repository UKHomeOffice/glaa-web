using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Core.Models;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.Services.Automapper;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace GLAA.Services.Tests.PostDataHandler
{
    [TestClass]
    public class When_updating_a_licence_property_from_a_model
    {
        private IMapper mapper;
        private IEntityFrameworkRepository repository;
        private ILicenceRepository licenceRepository;
        private IDateTimeProvider dateTimeProvider;
        private IStatusRepository statusRepository;

        [TestInitialize]
        public void Setup()
        {
            var config = new AutoMapperConfig().Configure();

            mapper = config.CreateMapper();
            repository = Substitute.For<IEntityFrameworkRepository>();
            licenceRepository = Substitute.For<ILicenceRepository>();
            dateTimeProvider = Substitute.For<IDateTimeProvider>();
            statusRepository = Substitute.For<IStatusRepository>();
        }

        [TestMethod]
        public void it_should_update_the_licence_field_from_the_model()
        {
            const int expectedId = 1;
            const string expectedName = "Name";
            var licence = new Licence {Id = expectedId, BusinessName = string.Empty};
            var model = new BusinessNameViewModel {BusinessName = expectedName};

            licenceRepository.GetById(expectedId).Returns(licence);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            pdh.Update(expectedId, l => l, model);

            repository.Received(1).Upsert(Arg.Is<Licence>(l => l.BusinessName == expectedName));
        }

        [TestMethod]
        public void it_should_update_the_address_field_from_the_address_view_model()
        {
            const int expectedId = 1;
            const string expectedLine1 = "Line 1";
            const string expectedCounty = "Somerset";
            const int expectedCountry = 1;
            const string expectedPostcode = "BA2 3DQ";
            var licence = new Licence {Id = expectedId, Address = new Address()};
            var model = new AddressViewModel
            {
                AddressLine1 = expectedLine1,
                County = expectedCounty,
                CountryId = expectedCountry,
                Postcode = expectedPostcode
            };

            licenceRepository.GetById(expectedId).Returns(licence);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            pdh.UpdateAddress(expectedId, l => l, model);

            repository.Received(0).Create<Address>();
            repository.Received(1).Upsert(Arg.Is<Licence>(l =>
                l.Address.AddressLine1.Equals(expectedLine1) && l.Address.County.Equals(expectedCounty) &&
                l.Address.CountryId == expectedCountry && l.Address.Postcode.Equals(expectedPostcode)));
        }

        [TestMethod]
        public void it_should_create_a_new_address_entity_if_one_is_not_present_and_update_the_address_from_the_address_view_model()
        {
            const int expectedId = 1;
            const string expectedLine1 = "Line 1";
            const string expectedCounty = "Somerset";
            const int expectedCountry = 1;
            const string expectedPostcode = "BA2 3DQ";
            var licence = new Licence { Id = expectedId, Address = null };
            var model = new AddressViewModel
            {
                AddressLine1 = expectedLine1,
                County = expectedCounty,
                CountryId = expectedCountry,
                Postcode = expectedPostcode
            };

            licenceRepository.GetById(expectedId).Returns(licence);
            repository.Create<Address>().Returns(new Address());

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            pdh.UpdateAddress(expectedId, l => l, model);

            repository.Received(1).Create<Address>();
            repository.Received(1).Upsert(Arg.Is<Licence>(l =>
                l.Address.AddressLine1.Equals(expectedLine1) && l.Address.County.Equals(expectedCounty) &&
                l.Address.CountryId == expectedCountry && l.Address.Postcode.Equals(expectedPostcode)));
        }

        [TestMethod]
        public void it_should_update_an_entity_in_a_collection_by_its_id()
        {
            const int expectedId = 1;
            const string expectedName = "Name";
            var licence = new Licence
            {
                Id = expectedId,
                NamedIndividuals = new List<NamedIndividual>
                {
                    new NamedIndividual
                    {
                        Id = expectedId,
                        FullName = string.Empty
                    }
                }
            };
            var model = new FullNameViewModel
            {
                FullName = expectedName
            };

            licenceRepository.GetById(expectedId).Returns(licence);
            repository.Upsert(Arg.Any<NamedIndividual>()).Returns(expectedId);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            var result = pdh.Update(expectedId, l => l.NamedIndividuals, model, expectedId);

            Assert.AreEqual(expectedId, result);
            repository.Received(1).Upsert(Arg.Is<NamedIndividual>(a => a.Id == expectedId && a.FullName.Equals(expectedName)));
        }

        [TestMethod]
        public void it_should_create_and_update_a_new_entity_if_one_does_not_exist_with_the_specified_id()
        {
            const int expectedId = 1;
            const string expectedName = "Name";
            var licence = new Licence
            {
                Id = expectedId,
                NamedIndividuals = new List<NamedIndividual>()
            };
            var model = new FullNameViewModel
            {
                FullName = expectedName
            };

            licenceRepository.GetById(expectedId).Returns(licence);
            repository.Create<NamedIndividual>().Returns(new NamedIndividual { Id = expectedId});
            repository.Upsert(Arg.Any<NamedIndividual>()).Returns(expectedId);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            var result = pdh.Update(expectedId, l => l.NamedIndividuals, model, expectedId);

            Assert.AreEqual(expectedId, result);
            repository.Received(1).Upsert(Arg.Is<NamedIndividual>(a => a.Id == expectedId && a.FullName.Equals(expectedName)));
        }

        [TestMethod]
        public void it_should_create_and_update_a_new_entity_with_an_address_if_one_does_not_exist_with_the_specified_id_and_it_is_addressable()
        {
            const int expectedId = 1;
            const string expectedName = "Name";
            var licence = new Licence
            {
                Id = expectedId,
                DirectorOrPartners = new List<DirectorOrPartner>()
            };
            var model = new FullNameViewModel
            {
                FullName = expectedName
            };

            licenceRepository.GetById(expectedId).Returns(licence);
            repository.Create<DirectorOrPartner>().Returns(new DirectorOrPartner { Id = expectedId });
            repository.Create<Address>().Returns(new Address { Id = expectedId });
            repository.Upsert(Arg.Any<DirectorOrPartner>()).Returns(expectedId);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            var result = pdh.Update(expectedId, l => l.DirectorOrPartners, model, expectedId);

            Assert.AreEqual(expectedId, result);
            repository.Received(1).Create<Address>();
            repository.Received(1).Upsert(Arg.Is<DirectorOrPartner>(a => a.Id == expectedId && a.FullName.Equals(expectedName) && a.Address.Id == expectedId));
        }

        [TestMethod]
        public void it_should_add_checked_items_to_the_entity_from_the_model()
        {
            const int expectedId = 1;
            const string expectedName = "Name";
            var licence = new Licence
            {
                Id = expectedId,
                OperatingIndustries = new List<LicenceIndustry>()
            };
            var model = new OperatingIndustriesViewModel
            {
                OperatingIndustries = new List<CheckboxListItem>
                {
                    new CheckboxListItem
                    {
                        Id = expectedId,
                        Name = expectedName,
                        Checked = true
                    }
                }
            };

            licenceRepository.GetById(expectedId).Returns(licence);
            repository.GetById<Industry>(expectedId).Returns(new Industry {Id = expectedId, Name = expectedName});

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            pdh.Update(expectedId, l => l.OperatingIndustries, model.OperatingIndustries);

            repository.Received(1).Upsert(Arg.Is<Licence>(l =>
                l.OperatingIndustries.Single().Id == expectedId &&
                l.OperatingIndustries.Single().IndustryId == expectedId &&
                l.OperatingIndustries.Single().LicenceId == expectedId));
        }

        [TestMethod]
        public void it_should_not_add_a_checked_item_to_the_entity_from_the_model_if_it_is_already_present()
        {
            const int expectedId = 1;
            const string expectedName = "Name";
            var licence = new Licence
            {
                Id = expectedId,
                OperatingIndustries = new List<LicenceIndustry>
                {
                    new LicenceIndustry
                    {
                        LicenceId = expectedId,
                        IndustryId = expectedId,
                        Industry = new Industry
                        {
                            Id = expectedId,
                            Name = expectedName
                        }
                    }
                }
            };
            var model = new OperatingIndustriesViewModel
            {
                OperatingIndustries = new List<CheckboxListItem>
                {
                    new CheckboxListItem {Id = expectedId, Name = expectedName, Checked = true}
                }
            };

            licenceRepository.GetById(expectedId).Returns(licence);
            repository.GetById<Industry>(expectedId).Returns(new Industry { Id = expectedId, Name = expectedName });

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            pdh.Update(expectedId, l => l.OperatingIndustries, model.OperatingIndustries);

            repository.Received(1).Upsert(Arg.Is<Licence>(l =>
                l.OperatingIndustries.Single().Id == expectedId &&
                l.OperatingIndustries.Single().IndustryId == expectedId &&
                l.OperatingIndustries.Single().Industry.Name.Equals(expectedName)));
        }
    }
}
