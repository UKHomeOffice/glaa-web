using System.Collections.Generic;
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
    public class When_linking_or_unlinking_an_item
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
        public void it_updates_a_principal_authority_that_is_linked_to_a_director()
        {
            const int licenceId = 1;
            const int paId = 2;
            const int dopId = 3;
            const string expectedName = "Name";
            var licence = new Licence
            {
                Id = licenceId,
                PrincipalAuthorities = new List<PrincipalAuthority>
                {
                    new PrincipalAuthority
                    {
                        Id = paId,
                        DirectorOrPartner = new DirectorOrPartner
                        {
                            Id = dopId
                        }
                    }
                }
            };
            var model = new FullNameViewModel {FullName = expectedName};

            licenceRepository.GetById(licenceId).Returns(licence);
            repository.Upsert(Arg.Any<PrincipalAuthority>()).Returns(paId);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            var result = pdh.UpsertPrincipalAuthorityAndLinkToDirectorOrPartner(licenceId, dopId, paId, model);

            Assert.AreEqual(paId, result);
            repository.Received(1).Upsert(Arg.Is<PrincipalAuthority>(p => p.FullName.Equals(expectedName) && p.DirectorOrPartner.Id == dopId));
            repository.Received(0).Create<PrincipalAuthority>();
            repository.Received(0).GetById<DirectorOrPartner>(Arg.Any<int>());
        }

        [TestMethod]
        public void it_updates_a_principal_authority_and_creates_a_link_to_the_director()
        {
            const int licenceId = 1;
            const int paId = 2;
            const int dopId = 3;
            const string expectedName = "Name";
            var licence = new Licence
            {
                Id = licenceId,
                PrincipalAuthorities = new List<PrincipalAuthority>
                {
                    new PrincipalAuthority
                    {
                        Id = paId,
                        DirectorOrPartner = null
                    }
                }
            };
            var model = new FullNameViewModel { FullName = expectedName };

            licenceRepository.GetById(licenceId).Returns(licence);
            repository.GetById<DirectorOrPartner>(dopId).Returns(new DirectorOrPartner {Id = dopId});
            repository.Upsert(Arg.Any<PrincipalAuthority>()).Returns(paId);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            var result = pdh.UpsertPrincipalAuthorityAndLinkToDirectorOrPartner(licenceId, dopId, paId, model);

            Assert.AreEqual(paId, result);
            repository.Received(1).Upsert(Arg.Is<PrincipalAuthority>(p => p.FullName.Equals(expectedName) && p.DirectorOrPartner.Id == dopId));
            repository.Received(0).Create<PrincipalAuthority>();
            repository.Received(1).GetById<DirectorOrPartner>(dopId);
        }

        [TestMethod]
        public void it_creates_and_updates_a_new_principal_authority_if_one_does_not_exist()
        {
            const int licenceId = 1;
            const int paId = 2;
            const int dopId = 3;
            const string expectedName = "Name";
            var licence = new Licence
            {
                Id = licenceId,
                PrincipalAuthorities = new List<PrincipalAuthority>()
            };
            var model = new FullNameViewModel { FullName = expectedName };

            licenceRepository.GetById(licenceId).Returns(licence);
            repository.GetById<DirectorOrPartner>(dopId).Returns(new DirectorOrPartner { Id = dopId });
            repository.Create<PrincipalAuthority>().Returns(new PrincipalAuthority {Id = paId});
            repository.Upsert(Arg.Any<PrincipalAuthority>()).Returns(paId);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            var result = pdh.UpsertPrincipalAuthorityAndLinkToDirectorOrPartner(licenceId, dopId, paId, model);

            Assert.AreEqual(paId, result);
            repository.Received(1).Upsert(Arg.Is<PrincipalAuthority>(p => p.FullName.Equals(expectedName) && p.DirectorOrPartner.Id == dopId && p.Licence.Id == licenceId));
            repository.Received(1).Create<PrincipalAuthority>();
            repository.Received(1).GetById<DirectorOrPartner>(dopId);
        }

        [TestMethod]
        public void it_updates_a_director_that_is_linked_to_a_principal_authority()
        {
            const int licenceId = 1;
            const int paId = 2;
            const int dopId = 3;
            const string expectedName = "Name";
            var licence = new Licence
            {
                Id = licenceId,
                DirectorOrPartners = new List<DirectorOrPartner>
                {
                    new DirectorOrPartner
                    {
                        Id = dopId,
                        PrincipalAuthority = new PrincipalAuthority
                        {
                            Id = paId
                        }
                    }
                }
            };
            var model = new FullNameViewModel { FullName = expectedName };

            licenceRepository.GetById(licenceId).Returns(licence);
            repository.Upsert(Arg.Any<DirectorOrPartner>()).Returns(dopId);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            var result = pdh.UpsertDirectorOrPartnerAndLinkToPrincipalAuthority(licenceId, paId, dopId, model);

            Assert.AreEqual(dopId, result);
            repository.Received(1).Upsert(Arg.Is<DirectorOrPartner>(p => p.FullName.Equals(expectedName) && p.PrincipalAuthority.Id == paId));
            repository.Received(0).Create<DirectorOrPartner>();
            repository.Received(0).GetById<PrincipalAuthority>(Arg.Any<int>());
        }

        [TestMethod]
        public void it_updates_a_director_and_creates_a_link_to_the_principal_authority()
        {
            const int licenceId = 1;
            const int paId = 2;
            const int dopId = 3;
            const string expectedName = "Name";
            var licence = new Licence
            {
                Id = licenceId,
                DirectorOrPartners = new List<DirectorOrPartner>
                {
                    new DirectorOrPartner
                    {
                        Id = dopId,
                        PrincipalAuthority = null
                    }
                }
            };
            var model = new FullNameViewModel { FullName = expectedName };

            licenceRepository.GetById(licenceId).Returns(licence);
            repository.GetById<PrincipalAuthority>(paId).Returns(new PrincipalAuthority { Id = paId });
            repository.Upsert(Arg.Any<DirectorOrPartner>()).Returns(dopId);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            var result = pdh.UpsertDirectorOrPartnerAndLinkToPrincipalAuthority(licenceId, paId, dopId, model);

            Assert.AreEqual(dopId, result);
            repository.Received(1).Upsert(Arg.Is<DirectorOrPartner>(p => p.FullName.Equals(expectedName) && p.PrincipalAuthority.Id == paId));
            repository.Received(0).Create<DirectorOrPartner>();
            repository.Received(1).GetById<PrincipalAuthority>(paId);
        }

        [TestMethod]
        public void it_creates_and_updates_a_new_director_if_one_does_not_exist()
        {
            const int licenceId = 1;
            const int paId = 2;
            const int dopId = 3;
            const string expectedName = "Name";
            var licence = new Licence
            {
                Id = licenceId,
                DirectorOrPartners = new List<DirectorOrPartner>()
            };
            var model = new FullNameViewModel { FullName = expectedName };

            licenceRepository.GetById(licenceId).Returns(licence);
            repository.GetById<PrincipalAuthority>(paId).Returns(new PrincipalAuthority { Id = paId });
            repository.Create<DirectorOrPartner>().Returns(new DirectorOrPartner { Id = dopId });
            repository.Upsert(Arg.Any<DirectorOrPartner>()).Returns(dopId);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            var result = pdh.UpsertDirectorOrPartnerAndLinkToPrincipalAuthority(licenceId, paId, dopId, model);

            Assert.AreEqual(dopId, result);
            repository.Received(1).Upsert(Arg.Is<DirectorOrPartner>(p => p.FullName.Equals(expectedName) && p.PrincipalAuthority.Id == paId && p.Licence.Id == licenceId));
            repository.Received(1).Create<DirectorOrPartner>();
            repository.Received(1).GetById<PrincipalAuthority>(paId);
        }

        [TestMethod]
        public void it_unlinks_the_principal_authority_from_a_director()
        {
            const int dopId = 1;
            var dop = new DirectorOrPartner {Id = dopId, PrincipalAuthority = new PrincipalAuthority()};

            repository.GetById<DirectorOrPartner>(dopId).Returns(dop);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            pdh.UnlinkPrincipalAuthorityFromDirectorOrPartner(dopId);

            repository.Received(1).Upsert(Arg.Is<DirectorOrPartner>(d => d.PrincipalAuthority == null));
        }

        [TestMethod]
        public void it_unlinks_a_director_from_the_principal_authority()
        {
            const int paId = 1;
            var pa = new PrincipalAuthority { Id = paId, DirectorOrPartner = new DirectorOrPartner() };

            repository.GetById<PrincipalAuthority>(paId).Returns(pa);

            var pdh = new LicenceApplicationPostDataHandler(mapper, repository, licenceRepository, statusRepository, dateTimeProvider);
            pdh.UnlinkDirectorOrPartnerFromPrincipalAuthority(paId);

            repository.Received(1).Upsert(Arg.Is<PrincipalAuthority>(d => d.DirectorOrPartner == null));
        }
    }
}
