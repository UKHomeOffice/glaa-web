using System.Collections.Generic;
using GLAA.ViewModels;
using GLAA.Web.FormLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLAA.Web.Tests.FormLogic
{
    [TestClass]
    public class When_using_the_form_definition
    {
        private const string ValidValue = "valid";

        public class ExampleViewModel
        {
            public ExampleViewModel()
            {
                Populated = ValidValue;
                SubModel = new SubModel();
            }

            public string Populated { get; set; }
            public string Unpopulated { get; set; }
            public bool CanView { get; set; }

            public SubModel SubModel { get; set; }
        }

        public class SubModel : ICanView<ExampleViewModel>
        {
            public bool CanView(ExampleViewModel parent)
            {
                return parent.CanView;
            }
        }

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
                        new FormPageDefinition()
                    }
                }
            };

            var result = form.GetSectionLength(section);

            Assert.AreEqual(config.Fields.Count, result);
        }

        [TestMethod]
        public void it_returns_null_if_a_non_present_view_model_is_requested()
        {
            const FormSection emptySection = FormSection.OrganisationDetails;
            const string invalidActionName = "not present";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    emptySection,
                    new[]
                    {
                        new FormPageDefinition()
                    }
                }
            };

            var result = form.GetViewModel(emptySection, invalidActionName, new ExampleViewModel());

            Assert.IsNull(result);
        }

        [TestMethod]
        public void it_returns_null_if_the_parent_is_null()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string validActionName = "validName";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), validActionName)
                    }
                }
            };

            var result = form.GetViewModel<ExampleViewModel>(validSection, validActionName, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void it_returns_the_correct_view_model_if_a_valid_view_model_is_requested()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string validActionName = "validName";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), validActionName)
                    }
                }
            };

            var result = form.GetViewModel(validSection, validActionName, new ExampleViewModel());

            Assert.AreEqual(ValidValue, result);
        }

        [TestMethod]
        public void a_page_with_an_overridden_view_condition_is_always_viewable()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string validActionName = "validName";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), validActionName, true)
                    }
                }
            };

            var result = form.CanViewPage(validSection, validActionName, new ExampleViewModel());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void a_model_that_is_not_i_can_view_is_always_viewable()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string validActionName = "validName";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), validActionName)
                    }
                }
            };

            var result = form.CanViewPage(validSection, validActionName, new ExampleViewModel());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void a_parent_that_meets_the_child_view_condition_is_viewable()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string validActionName = "validName";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.SubModel), validActionName)
                    }
                }
            };

            var parent = new ExampleViewModel {CanView = true};

            var result = form.CanViewPage(validSection, validActionName, parent);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void a_parent_that_does_not_meet_the_child_view_condition_is_not_viewable()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string validActionName = "validName";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.SubModel), validActionName)
                    }
                }
            };

            var parent = new ExampleViewModel { CanView = false };

            var result = form.CanViewPage(validSection, validActionName, parent);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void the_last_page_is_returned_when_requested()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string validActionName = "validName";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), validActionName)
                    }
                }
            };

            var result = form.GetLastPage(validSection);

            Assert.IsTrue(result.MatchesName(validActionName));
        }

        [TestMethod]
        public void it_should_identify_if_the_page_following_the_requested_page_is_the_last_page()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string validActionName = "validName";
            const string lastActionName = "last";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), validActionName),
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), lastActionName)
                    }
                }
            };

            var result = form.IsNextPageLastPage(validSection, validActionName);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void it_should_identify_if_the_page_following_the_requested_page_is_not_the_last_page()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string validActionName = "validName";
            const string lastActionName = "last";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), validActionName),
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), lastActionName)
                    }
                }
            };

            var result = form.IsNextPageLastPage(validSection, "no match");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void it_should_return_the_next_page_if_found()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string validActionName = "validName";
            const string lastActionName = "last";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), validActionName),
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), lastActionName)
                    }
                }
            };

            var result = form.GetNextPage(validSection, validActionName);

            Assert.IsTrue(result.MatchesName(lastActionName));
        }

        [TestMethod]
        public void it_should_return_the_last_page_if_no_match_for_next_page_is_found()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string validActionName = "validName";
            const string lastActionName = "last";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), validActionName),
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), lastActionName)
                    }
                }
            };

            var result = form.GetNextPage(validSection, "no match");

            Assert.IsTrue(result.MatchesName(lastActionName));
        }

        [TestMethod]
        public void it_should_return_the_previous_page_if_found()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string validActionName = "validName";
            const string lastActionName = "last";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), validActionName),
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), lastActionName)
                    }
                }
            };

            var result = form.GetPreviousPage(validSection, lastActionName);

            Assert.IsTrue(result.MatchesName(validActionName));
        }

        [TestMethod]
        public void it_should_return_the_last_page_if_no_match_for_previous_page_is_found()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string validActionName = "validName";
            const string lastActionName = "last";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), validActionName),
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), lastActionName)
                    }
                }
            };

            var result = form.GetPreviousPage(validSection, "no match");

            Assert.IsTrue(result.MatchesName(lastActionName));
        }

        [TestMethod]
        public void it_should_keep_searching_forwards_until_a_viewable_match_is_found()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string startActionName = "startName";
            const string noViewActionName = "no";
            const string lastActionName = "last";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), startActionName),
                        new FormPageDefinition(nameof(ExampleViewModel.SubModel), noViewActionName),
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), lastActionName)
                    }
                }
            };

            var parent = new ExampleViewModel {CanView = false};

            var result = form.GetNextPossiblePage(validSection, startActionName, parent);

            Assert.IsTrue(result.MatchesName(lastActionName));
        }

        [TestMethod]
        public void it_should_keep_searching_backwards_until_a_viewable_match_is_found()
        {
            const FormSection validSection = FormSection.OrganisationDetails;
            const string startActionName = "startName";
            const string noViewActionName = "no";
            const string lastActionName = "last";

            config.Fields = new Dictionary<FormSection, FormPageDefinition[]>
            {
                {
                    validSection,
                    new[]
                    {
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), startActionName),
                        new FormPageDefinition(nameof(ExampleViewModel.SubModel), noViewActionName),
                        new FormPageDefinition(nameof(ExampleViewModel.Populated), lastActionName)
                    }
                }
            };

            var parent = new ExampleViewModel {CanView = false};

            var result = form.GetPreviousPossiblePage(validSection, lastActionName, parent);

            Assert.IsTrue(result.MatchesName(startActionName));
        }
    }
}
