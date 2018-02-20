using System.Collections.Generic;
using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GLAA.Web.Controllers;
using NSubstitute;

namespace GLAA.Web.Tests.Controllers
{
    [TestClass]
    public class When_accessing_the_licence_controller
    {
        private LicenceController controller;

        [TestInitialize]
        public void Setup()
        {
            var licenceViewModelBuilder = Substitute.For<ILicenceApplicationViewModelBuilder>();
            var licenceStatusViewModelBuilder = Substitute.For<ILicenceStatusViewModelBuilder>();

            licenceStatusViewModelBuilder.BuildRandomStatus().Returns(x => new LicenceStatusViewModel());

            controller = new LicenceController(null, licenceViewModelBuilder, null, licenceStatusViewModelBuilder, null, new ConstantService(), null, null);
        }
    }
}
