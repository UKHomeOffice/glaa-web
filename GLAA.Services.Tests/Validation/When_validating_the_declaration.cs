using System;
using System.Collections.Generic;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Services.Tests.Validation
{
    [TestClass]
    public class When_validating_the_declaration
    {
        private DeclarationViewModel validModel;

        [TestInitialize]
        public void Setup()
        {
            validModel = new DeclarationViewModel
            {
                SignatoryName = "The name of the signatory",
                SignatureDate = new DateViewModel
                {
                    Date = new DateTime(2000, 01, 01)
                }
            };
        }

        [TestMethod]
        public void a_blank_model_is_invalid()
        {
            var vm = new DeclarationViewModel();

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
            validModel.SignatoryName = string.Empty;

            validModel.Validate();

            Assert.AreEqual(false, validModel.IsValid);
        }
    }
}
