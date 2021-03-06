﻿using AutoMapper;
using GLAA.Common;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.Services.Automapper;
using GLAA.Services.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace GLAA.Services.Tests.PostDataHandler
{
    [TestClass]
    public class When_deleting_an_entity
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
    }
}
