using System.Collections.Generic;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.FormLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Web.Tests.FormLogic
{
    [TestClass]
    public class When_using_the_form_definition
    {
        private IFormDefinition form;
        private IFieldConfiguration config;

        [TestInitialize]
        public void Setup()
        {
            config = new FieldConfiguration();
            form = new LicenceApplicationFormDefinition(config);
        }

        [TestMethod]
        public void the_section_length_is_returned_correctly()
        {
            var section = FormSection.OrganisationDetails;

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    section,
                    new[]
                    {
                        new FormPageDefinition(string.Empty, null)
                    }
                }
            };

            Assert.AreEqual(config.Fields.Count, form.GetSectionLength(section));
        }

        [TestMethod]
        public void it_returns_null_if_a_non_present_view_model_is_requested()
        {
            var emptySection = FormSection.OrganisationDetails;
            var invalidFormPageIndex = 2;

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    emptySection,
                    new[]
                    {
                        new FormPageDefinition(string.Empty, null)
                    }
                }
            };

            Assert.IsNull(form.GetViewModel(emptySection, invalidFormPageIndex, emptySection));
        }

        [TestMethod]
        public void it_returns_the_correct_view_model_if_a_valid_view_model_is_requested()
        {
            var validSection = FormSection.OrganisationDetails;
            var validFormPageIndex = 1;

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition("BusinessName", typeof(BusinessNameViewModel))
                    }
                }
            };

            Assert.IsNotNull(form.GetViewModel(validSection, validFormPageIndex, validSection));

        }
    }
}
