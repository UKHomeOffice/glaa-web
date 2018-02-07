using GLAA.Domain.Models;
using GLAA.ViewModels;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Services.Tests.Validation
{
    [TestClass]
    public class When_validating_eligibility_criteria
    {
        private SignUpViewModel validModel;

        [TestInitialize]
        public void Setup()
        {
            // valid model
            validModel = new SignUpViewModel
            {
                FullName = new PrincipalAuthorityFullNameViewModel
                {
                    FirstName = "First",
                    LastName = "Last"
                },
                EmailAddress = new PrincipalAuthorityEmailAddressViewModel
                {
                    EmailAddress  = "a@example.org"
                },
                Address = new AddressViewModel
                {
                    AddressLine1 = "Line 1",
                    AddressLine2 = "Line 2",
                    AddressLine3 = "Line 3",
                    Country = "UK",
                    County = "Notts",
                    NonUK = false,
                    Postcode = "W1A 1AA",
                    Town = "Nottingham"
                },
                CommunicationPreference = new CommunicationPreferenceViewModel
                {
                    CommunicationPreference = CommunicationPreference.Email
                },
                Password = new PasswordViewModel
                {
                    Password = "hunter2",
                    ConfirmPassword = "hunter2",
                    HasPassword = true
                }
            };
        }

        [TestMethod]
        public void a_blank_model_is_invalid()
        {
            var vm = new SignUpViewModel();

            vm.Validate();

            Assert.AreEqual(false, vm.IsValid);
        }

        [TestMethod]
        public void a_complete_model_is_valid()
        {
            validModel.Validate();

            Assert.AreEqual(true, validModel.IsValid);
        }

        [TestMethod]
        public void a_model_with_a_missing_required_property_is_invalid()
        {
            validModel.FullName.LastName = null;

            validModel.Validate();

            Assert.AreEqual(false, validModel.IsValid);
        }
    }
}
