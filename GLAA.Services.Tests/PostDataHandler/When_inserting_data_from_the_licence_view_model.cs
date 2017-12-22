using System;
using AutoMapper;
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
    public class When_inserting_data_from_the_licence_view_model
    {
        private IMapper mapper;
        private IEntityFrameworkRepository repository;
        private ILicenceRepository licenceRepository;
        private IStatusRepository statusRepository;
        private IDateTimeProvider dateTimeProvider;

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
        public void it_should_create_new_licence_and_address_entities_and_return_the_licence_id()
        {
            const int expectedId = 1;

            var licence = new Licence {Id = 1};

            repository.Create<Licence>().Returns(licence);
            repository.Create<Address>().Returns(new Address());
            repository.Upsert(Arg.Any<Licence>()).ReturnsForAnyArgs(1);
            repository.GetById<Licence>(expectedId).Returns(licence);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository,
                dateTimeProvider);
            var result = pdh.Insert(new LicenceApplicationViewModel());

            Assert.AreEqual(expectedId, result);
            repository.Received().Create<Licence>();
            repository.Received().Create<Address>();
        }

        [TestMethod]
        public void it_should_set_a_draft_URN_for_the_new_licence()
        {
            const int expectedId = 1;
            const string expectedUrn = "DRAFT-0001";

            var licence = new Licence { Id = 1 };

            repository.Create<Licence>().Returns(licence);
            repository.Create<Address>().Returns(new Address());
            repository.Upsert(Arg.Any<Licence>()).ReturnsForAnyArgs(1);
            repository.GetById<Licence>(expectedId).Returns(licence);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository,
                dateTimeProvider);
            var result = pdh.Insert(new LicenceApplicationViewModel());

            Assert.AreEqual(expectedId, result);
            repository.Received().Upsert(Arg.Is<Licence>(l => l.ApplicationId == expectedUrn));
        }

        [TestMethod]
        public void it_should_not_create_a_new_status_if_one_is_not_present_in_the_model()
        {
            const int expectedId = 1;

            var licence = new Licence { Id = 1 };

            repository.Create<Licence>().Returns(licence);
            repository.Create<Address>().Returns(new Address());
            repository.Upsert(Arg.Any<Licence>()).ReturnsForAnyArgs(1);
            repository.GetById<Licence>(expectedId).Returns(licence);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository,
                dateTimeProvider);
            var result = pdh.Insert(new LicenceApplicationViewModel());

            Assert.AreEqual(expectedId, result);
            repository.DidNotReceive().Create<LicenceStatusChange>();
            repository.DidNotReceive().Upsert(Arg.Any<LicenceStatusChange>());
        }

        [TestMethod]
        public void it_should_create_a_new_status_if_one_is_present_in_the_model()
        {
            var consts = new ConstantService();
            var model = new LicenceApplicationViewModel
            {
                NewLicenceStatus = new LicenceStatusViewModel
                {
                    Id = consts.NewApplicationStatusId
                }
            };

            const int expectedId = 1;

            var licence = new Licence { Id = 1 };
            var status = new LicenceStatus
            {
                Id = 100
            };

            repository.Create<Licence>().Returns(licence);
            repository.Create<Address>().Returns(new Address());
            repository.Create<LicenceStatusChange>().Returns(new LicenceStatusChange());
            repository.Upsert(Arg.Any<Licence>()).ReturnsForAnyArgs(1);
            repository.GetById<Licence>(expectedId).Returns(licence);
            repository.GetById<LicenceStatus>(100).Returns(status);

            dateTimeProvider.Now().Returns(DateTime.MinValue);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository,
                dateTimeProvider);
            var result = pdh.Insert(model);

            Assert.AreEqual(expectedId, result);
            repository.Received(1).Upsert(Arg.Is<LicenceStatusChange>(c => c.Status.Id == consts.NewApplicationStatusId && c.DateCreated == DateTime.MinValue));
        }
    }
}
