using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Repository;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace GLAA.Services.Tests.ViewModelBuilder
{
    [TestClass]
    public class When_retrieving_a_record
    {
        private ILicenceRepository licenceRepository;
        private IMapper mapper;
        private IReferenceDataProvider referenceDataProvider;

        private readonly IEnumerable<SelectListItem> countries = new[]
        {
            new SelectListItem
            {
                Text = "Country",
                Value = "1"
            }
        };

        private readonly IEnumerable<SelectListItem> counties = new[]
        {
            new SelectListItem
            {
                Text = "County",
                Value = "1"
            }
        };

        [TestInitialize]
        public void Init()
        {
            licenceRepository = Substitute.For<ILicenceRepository>();
            mapper = Substitute.For<IMapper>();
            referenceDataProvider = Substitute.For<IReferenceDataProvider>();
        }

        [TestMethod]
        public void it_should_populate_the_countries_list_if_required()
        {
            referenceDataProvider.GetCountries().Returns(countries);

            var vmb = new LicenceApplicationViewModelBuilder(licenceRepository, mapper, referenceDataProvider);

            var result = vmb.New<CountryNeeder>();

            Assert.AreEqual(countries.Single().Text, result.Countries.Single().Text);
            Assert.AreEqual(countries.Single().Value, result.Countries.Single().Value);
        }

        [TestMethod]
        public void it_should_populate_the_counties_list_if_required()
        {
            referenceDataProvider.GetCounties().Returns(counties);

            var vmb = new LicenceApplicationViewModelBuilder(licenceRepository, mapper, referenceDataProvider);

            var result = vmb.New<CountyNeeder>();

            Assert.AreEqual(counties.Single().Text, result.Counties.Single().Text);
            Assert.AreEqual(counties.Single().Value, result.Counties.Single().Value);
        }

        public void it_should_populate_both_lists_if_required()
        {
            referenceDataProvider.GetCounties().Returns(counties);
            referenceDataProvider.GetCountries().Returns(countries);

            var vmb = new LicenceApplicationViewModelBuilder(licenceRepository, mapper, referenceDataProvider);

            var result = vmb.New<BothNeeder>();

            Assert.AreEqual(countries.Single().Text, result.Countries.Single().Text);
            Assert.AreEqual(countries.Single().Value, result.Countries.Single().Value);
            Assert.AreEqual(counties.Single().Text, result.Counties.Single().Text);
            Assert.AreEqual(counties.Single().Value, result.Counties.Single().Value);
        }
    }

    internal class CountryNeeder : INeedCountries
    {
        public IEnumerable<SelectListItem> Countries { get; set; }
    }

    internal class CountyNeeder : INeedCounties
    {
        public IEnumerable<SelectListItem> Counties { get; set; }
    }

    internal class BothNeeder : INeedCountries, INeedCounties
    {
        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> Counties { get; set; }
    }
}
