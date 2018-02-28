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
                            new FormPageDefinition(),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessName)),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.Address)),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessPhoneNumber)),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessMobileNumber)),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessEmailAddress)),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessWebsite)),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.CommunicationPreference)),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.LegalStatus)),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessCredentialsViewModel), true),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.OperatingIndustries)),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.Turnover)),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.OperatingCountries)),
                            new FormPageDefinition()
                        }
                    },
                    {
                        FormSection.PrincipalAuthority,                        
                        new[]
                        {
                            new FormPageDefinition(),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.IsDirector)),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PrincipalAuthorityConfirmation)),
                        }
                        .Concat(BasicPersonFields)
                        .Concat(new [] {
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.Nationality)),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PassportViewModel)),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PrincipalAuthorityRightToWorkViewModel)),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.UndischargedBankruptViewModel)),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.DisqualifiedDirectorViewModel)),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.RestraintOrdersViewModel), true),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.RestraintOrdersViewModel)),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.UnspentConvictionsViewModel), true),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.UnspentConvictionsViewModel)),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.OffencesAwaitingTrialViewModel), true),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.OffencesAwaitingTrialViewModel)),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PreviousLicenceViewModel)),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PreviousExperience)),
                            new FormPageDefinition()
                        }).ToArray()
                    },
                    {
                        FormSection.AlternativeBusinessRepresentatives,
                        new []
                        {
                            new FormPageDefinition(),
                            new FormPageDefinition(),
                            new FormPageDefinition()
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
            new FormPageDefinition(nameof(PersonViewModel.FullName)),
            new FormPageDefinition(nameof(PersonViewModel.AlternativeName)),
            new FormPageDefinition(nameof(PersonViewModel.DateOfBirth)),
            new FormPageDefinition(nameof(PersonViewModel)),
            new FormPageDefinition(nameof(PersonViewModel.JobTitle)),
            new FormPageDefinition(nameof(PersonViewModel.Address)),
            new FormPageDefinition(nameof(PersonViewModel.BusinessPhoneNumber)),
            new FormPageDefinition(nameof(PersonViewModel.BusinessExtension)),
            new FormPageDefinition(nameof(PersonViewModel.PersonalMobileNumber)),
            new FormPageDefinition(nameof(PersonViewModel.PersonalEmailAddress)),
        };

        private static readonly FormPageDefinition[] PersonSecurityFields =
        {
            new FormPageDefinition(nameof(PersonViewModel.Nationality)),
            new FormPageDefinition(nameof(PersonViewModel.PassportViewModel)),
            new FormPageDefinition(nameof(RightToWorkViewModel)),
            new FormPageDefinition(nameof(PersonViewModel.UndischargedBankruptViewModel)),
            new FormPageDefinition(nameof(PersonViewModel.DisqualifiedDirectorViewModel)),
            new FormPageDefinition(nameof(PersonViewModel.RestraintOrdersViewModel), true),
            new FormPageDefinition(nameof(PersonViewModel.RestraintOrdersViewModel)),
            new FormPageDefinition(nameof(PersonViewModel.UnspentConvictionsViewModel), true),
            new FormPageDefinition(nameof(PersonViewModel.UnspentConvictionsViewModel)),
            new FormPageDefinition(nameof(PersonViewModel.OffencesAwaitingTrialViewModel), true),
            new FormPageDefinition(nameof(PersonViewModel.OffencesAwaitingTrialViewModel)),
            new FormPageDefinition(nameof(PersonViewModel.PreviousLicenceViewModel)),
        };

        public IDictionary<FormSection, FormPageDefinition[]> Fields { get; set; }
    }
}