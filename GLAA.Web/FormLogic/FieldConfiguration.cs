using System.Collections.Generic;
using System.Linq;
using GLAA.ViewModels;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Web.FormLogic
{
    public class FieldConfiguration : IFieldConfiguration
    {
        public FieldConfiguration()
        {
            Fields =
                new Dictionary<FormSection, FormPageDefinition[]>
                {
                    {
                        FormSection.Declaration,
                        new []
                        {
                            new FormPageDefinition(nameof(DeclarationViewModel)), 
                        }
                    },
                    {
                        FormSection.SignUp,
                        new []
                        {
                            new FormPageDefinition(nameof(SignUpViewModel.EmailAddress)),
                            new FormPageDefinition(nameof(SignUpViewModel.FullName)),
                            new FormPageDefinition(nameof(SignUpViewModel.Address)),
                            new FormPageDefinition(nameof(SignUpViewModel.CommunicationPreference)),
                            new FormPageDefinition(nameof(SignUpViewModel.Password)),
                            new FormPageDefinition()
                        }
                    },
                    {
                        FormSection.OrganisationDetails,
                        new[]
                        {
                            new FormPageDefinition(string.Empty, "Introduction"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessName), "BusinessName"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.Address), "Address"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessPhoneNumber), "BusinessPhoneNumber"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessMobileNumber), "BusinessMobileNumber"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessEmailAddress), "BusinessEmailAddress"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessWebsite), "BusinessWebsite"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.CommunicationPreference), "CommunicationPreference"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.LegalStatus), "LegalStatus"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessCredentials), "BusinessCredentials", true),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.OperatingIndustries), "OperatingIndustries"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.Turnover), "Turnover"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.OperatingCountries), "OperatingCountries"),
                            new FormPageDefinition(string.Empty, "Summary")
                        }
                    },
                    {
                        FormSection.PrincipalAuthority,                        
                        new[]
                        {
                            new FormPageDefinition(string.Empty, "Introduction"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.IsDirector), "IsDirector"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PrincipalAuthorityConfirmation), "PrincipalAuthorityConfirmation"),
                        }
                        .Concat(BasicPersonFields)
                        .Concat(new [] {
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.Nationality), "Nationality"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.Passport), "Passport"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PrincipalAuthorityRightToWork), "PrincipalAuthorityRightToWork"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.UndischargedBankrupt), "UndischargedBankrupt"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.DisqualifiedDirector), "DisqualifiedDirector"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.RestraintOrders), "RestraintOrders", true),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.RestraintOrders), "ReviewRestraintOrders"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.UnspentConvictions), "UnspentConvictions", true),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.UnspentConvictions), "ReviewUnspentConvictions"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.OffencesAwaitingTrial), "OffencesAwaitingTrial", true),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.OffencesAwaitingTrial), "ReviewOffencesAwaitingTrial"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PreviousLicence), "PreviousLicence"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PreviousExperience), "PreviousExperience"),
                            new FormPageDefinition(string.Empty, "Summary")
                        }).ToArray()
                    },
                    {
                        FormSection.AlternativeBusinessRepresentatives,
                        new []
                        {
                            new FormPageDefinition(string.Empty, "Introduction"),
                            new FormPageDefinition(nameof(AlternativeBusinessRepresentativeCollectionViewModel.HasAlternativeBusinessRepresentatives), "HasAlternativeBusinessRepresentatives"),
                            new FormPageDefinition(string.Empty, "Summary")
                        }
                    },
                    {
                        FormSection.AlternativeBusinessRepresentative,
                        BasicPersonFields
                        .Concat(PersonSecurityFields)
                        .Concat(new[] {
                            new FormPageDefinition()
                        }).ToArray()
                    },
                    {
                        FormSection.DirectorOrPartner,
                        new[]
                        {
                            new FormPageDefinition(nameof(DirectorOrPartnerViewModel.IsPreviousPrincipalAuthority)),
                        }
                        .Concat(BasicPersonFields)
                        .Concat(PersonSecurityFields)
                        .Concat(new [] {
                            new FormPageDefinition()
                        }).ToArray()                        
                    },
                    {
                        FormSection.DirectorsOrPartners,
                        new []
                        {
                            new FormPageDefinition(),
                            new FormPageDefinition(),
                            new FormPageDefinition()
                        }
                    },
                    {
                        FormSection.NamedIndividual,
                        new[]
                        {
                            new FormPageDefinition(nameof(NamedIndividualViewModel.FullName)),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.DateOfBirth)),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.BusinessPhoneNumber)),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.BusinessExtension)),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.RightToWorkViewModel)),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.UndischargedBankruptViewModel)),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.DisqualifiedDirectorViewModel)),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.RestraintOrdersViewModel), true),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.RestraintOrdersViewModel)),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.UnspentConvictionsViewModel), true),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.UnspentConvictionsViewModel)),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.OffencesAwaitingTrialViewModel), true),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.OffencesAwaitingTrialViewModel)),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.PreviousLicenceViewModel)),
                            new FormPageDefinition()
                        }
                    },
                    {
                        FormSection.JobTitle,
                        new[]
                        {
                            new FormPageDefinition(),
                            new FormPageDefinition()
                        }
                    },
                    {
                        FormSection.NamedIndividuals,
                        new []
                        {
                            new FormPageDefinition(),
                            new FormPageDefinition(),
                            new FormPageDefinition()
                        }
                    },
                    {
                        FormSection.Organisation,
                        new[]
                        {
                            new FormPageDefinition(),
                            new FormPageDefinition(nameof(OrganisationViewModel.OutsideSectorsViewModel)),
                            new FormPageDefinition(nameof(OrganisationViewModel.WrittenAgreementViewModel)),
                            new FormPageDefinition(nameof(OrganisationViewModel.PscControlledViewModel)),
                            new FormPageDefinition(nameof(OrganisationViewModel.MultipleBranchViewModel)),
                            new FormPageDefinition(nameof(OrganisationViewModel.TransportingWorkersViewModel)),
                            new FormPageDefinition(nameof(OrganisationViewModel.AccommodatingWorkersViewModel)),
                            new FormPageDefinition(nameof(OrganisationViewModel.SourcingWorkersViewModel)),
                            new FormPageDefinition(nameof(OrganisationViewModel.WorkerSupplyMethodViewModel)),
                            new FormPageDefinition(nameof(OrganisationViewModel.WorkerContractViewModel)),
                            new FormPageDefinition(nameof(OrganisationViewModel.BannedFromTradingViewModel)),
                            new FormPageDefinition(nameof(OrganisationViewModel.SubcontractorViewModel)),
                            new FormPageDefinition(nameof(OrganisationViewModel.ShellfishWorkerNumberViewModel)),
                            new FormPageDefinition(nameof(OrganisationViewModel.ShellfishWorkerNationalityViewModel)),
                            new FormPageDefinition(nameof(OrganisationViewModel.PreviouslyWorkedInShellfishViewModel)),
                            new FormPageDefinition()
                        }
                    }
                };
        }

        private static readonly FormPageDefinition[] BasicPersonFields =
        {
            new FormPageDefinition(nameof(PersonViewModel.FullName), "FullName"),
            new FormPageDefinition(nameof(PersonViewModel.AlternativeName), "AlternativeName"),
            new FormPageDefinition(nameof(PersonViewModel.DateOfBirth), "DateOfBirth"),
            new FormPageDefinition(nameof(PersonViewModel.BirthDetails), "BirthDetails"),
            new FormPageDefinition(nameof(PersonViewModel.JobTitle), "JobTitle"),
            new FormPageDefinition(nameof(PersonViewModel.Address), "Address"),
            new FormPageDefinition(nameof(PersonViewModel.BusinessPhoneNumber), "BusinessPhoneNumber"),
            new FormPageDefinition(nameof(PersonViewModel.BusinessExtension), "BusinessExtension"),
            new FormPageDefinition(nameof(PersonViewModel.PersonalMobileNumber), "PersonalMobileNumber"),
            new FormPageDefinition(nameof(PersonViewModel.PersonalEmailAddress), "PersonalEmailAddress"),
        };

        private static readonly FormPageDefinition[] PersonSecurityFields =
        {
            new FormPageDefinition(nameof(PersonViewModel.Nationality)),
            new FormPageDefinition(nameof(PersonViewModel.Passport)),
            new FormPageDefinition(nameof(RightToWorkViewModel)),
            new FormPageDefinition(nameof(PersonViewModel.UndischargedBankrupt)),
            new FormPageDefinition(nameof(PersonViewModel.DisqualifiedDirector)),
            new FormPageDefinition(nameof(PersonViewModel.RestraintOrders), true),
            new FormPageDefinition(nameof(PersonViewModel.RestraintOrders)),
            new FormPageDefinition(nameof(PersonViewModel.UnspentConvictions), true),
            new FormPageDefinition(nameof(PersonViewModel.UnspentConvictions)),
            new FormPageDefinition(nameof(PersonViewModel.OffencesAwaitingTrial), true),
            new FormPageDefinition(nameof(PersonViewModel.OffencesAwaitingTrial)),
            new FormPageDefinition(nameof(PersonViewModel.PreviousLicence)),
        };

        public IDictionary<FormSection, FormPageDefinition[]> Fields { get; set; }

        public FormPageDefinition[] this[FormSection section] => Fields[section];
    }
}