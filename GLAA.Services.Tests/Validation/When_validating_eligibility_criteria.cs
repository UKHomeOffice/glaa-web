using System;
using System.Collections.Generic;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Services.Tests.Validation
{
    [TestClass]
    public class When_validating_eligibility_criteria
    {
        private EligibilityViewModel validModel;

        [TestInitialize]
        public void Setup()
        {
            // valid model
            validModel = new EligibilityViewModel
            {
                SuppliesWorkers = new SuppliesWorkersViewModel
                {
                    SuppliesWorkers = true
                },
                Turnover = new TurnoverViewModel
                {
                    TurnoverBand  = TurnoverBand.FiveToTenMillion
                },                
                OperatingIndustries = new OperatingIndustriesViewModel
                {
                    OperatingIndustries = new List<CheckboxListItem>
                    {
                        new CheckboxListItem
                        {
                            Id = 1,
                            Name = "An industry",
                            Checked = true
                        }
                    }
                },
                EligibilitySummary = new EligibilitySummaryViewModel
                {
                    ContinueApplication = true
                }
            };
        }

        [TestMethod]
        public void a_blank_model_is_invalid()
        {
            var vm = new EligibilityViewModel();

            vm.Validate();

            Assert.AreEqual(false, vm.IsValid);
        }

        [TestMethod]
        public void a_complete_model_with_other_entered_is_valid()
        {
            validModel = new EligibilityViewModel
            {
                SuppliesWorkers = new SuppliesWorkersViewModel
                {
                    SuppliesWorkers = true
                },
                Turnover = new TurnoverViewModel
                {
                    TurnoverBand = TurnoverBand.FiveToTenMillion
                },
                OperatingIndustries = new OperatingIndustriesViewModel
                {
                    OperatingIndustries = new List<CheckboxListItem>
                    {
                        new CheckboxListItem
                        {
                            Id = 1,
                            Name = "An industry",
                            Checked = true
                        },
                        new CheckboxListItem
                        {
                            Id = 5,
                            Name = "Other",
                            Checked = true
                        }
                    },
                    OtherIndustry = "Some other industry"
                },
                EligibilitySummary = new EligibilitySummaryViewModel
                {
                    ContinueApplication = true
                }
            };
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
            validModel.SuppliesWorkers.SuppliesWorkers = null;

            validModel.Validate();

            Assert.AreEqual(false, validModel.IsValid);
        }
    }
}
