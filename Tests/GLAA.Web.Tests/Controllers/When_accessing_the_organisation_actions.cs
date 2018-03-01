using System;
using System.Diagnostics;
using GLAA.Domain.Models;
using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GLAA.Web.Controllers;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace GLAA.Web.Tests.Controllers
{
    [TestClass]
    public class When_accessing_the_organisation_actions
    {
        private OrganisationDetailsController controller;
        private ILicenceApplicationPostDataHandler licencePostDataHandler;
        private ILicenceApplicationViewModelBuilder licenceViewModelBuilder;
        private ISessionHelper session;
        private IFormDefinition formDefinition;
        private IReferenceDataProvider referenceDataProvider;

        [TestInitialize]
        public void Setup()
        {
            licenceViewModelBuilder = Substitute.For<ILicenceApplicationViewModelBuilder>();
            var licenceStatusViewModelBuilder = Substitute.For<ILicenceStatusViewModelBuilder>();
            licencePostDataHandler = Substitute.For<ILicenceApplicationPostDataHandler>();
            session = Substitute.For<ISessionHelper>();
            formDefinition = new LicenceApplicationFormDefinition(new FieldConfiguration());
            referenceDataProvider = Substitute.For<IReferenceDataProvider>();

            licenceStatusViewModelBuilder.BuildRandomStatus().Returns(x => new LicenceStatusViewModel());

            controller = new OrganisationDetailsController(session, licenceViewModelBuilder, licencePostDataHandler, licenceStatusViewModelBuilder, formDefinition, new ConstantService(), referenceDataProvider);
        }

        [TestMethod]
        public void it_can_submit_the_fullname()
        {
            licenceViewModelBuilder.Build<OrganisationDetailsViewModel>(Arg.Any<int>())
                .Returns(new OrganisationDetailsViewModel());

            var result = controller.BusinessName(new BusinessNameViewModel {
                BusinessName = "Org Name",
                HasPreviousTradingName = false,
                HasTradingName = false
            }) as RedirectToActionResult;

            licencePostDataHandler.Received(1).Update(
                Arg.Any<int>(), 
                Arg.Any<Func<Licence, Licence>>(), 
                Arg.Any<BusinessNameViewModel>());

            Assert.IsNotNull(result);
            Assert.AreEqual("Part", result.ActionName);
            Assert.AreEqual(3, result.RouteValues["id"]);
        }
    }
}
