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
                            new FormPageDefinition(nameof(SignUpViewModel.EmailAddress), "EmailAddress"),
                            new FormPageDefinition(nameof(SignUpViewModel.FullName), "FullName"),
                            new FormPageDefinition(nameof(SignUpViewModel.Address), "Address"),
                            new FormPageDefinition(nameof(SignUpViewModel.CommunicationPreference), "CommunicationPreference"),
                            new FormPageDefinition(nameof(SignUpViewModel.Password), "Password"),
                            new FormPageDefinition("Summary")
                        }
                    },
                    {
                        FormSection.OrganisationDetails,
                        new[]
                        {
                            new FormPageDefinition("Introduction"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessName), "BusinessName"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.Address), "Address"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessPhoneNumber), "BusinessPhoneNumber"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessMobileNumber), "BusinessMobileNumber"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessEmailAddress), "BusinessEmailAddress"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.BusinessWebsite), "BusinessWebsite"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.CommunicationPreference), "CommunicationPreference"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.LegalStatus), "LegalStatus"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel), "BusinessCredentials", true),
                            new FormPageDefinition(nameof(OperatingIndustriesViewModel.OperatingIndustries), "OperatingIndustries"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.Turnover), "Turnover"),
                            new FormPageDefinition(nameof(OrganisationDetailsViewModel.OperatingCountries), "OperatingCountries"),
                            new FormPageDefinition("Summary")
                        }
                    },
                    {
                        FormSection.PrincipalAuthority,                        
                        new[]
                        {
                            new FormPageDefinition("Introduction"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.IsDirector), "IsDirector"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PrincipalAuthorityConfirmation), "PrincipalAuthorityConfirmation"),
                        }
                        .Concat(BasicPersonFields)
                        .Concat(new [] {
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.Nationality), "Nationality"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PassportViewModel), "Passport"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PrincipalAuthorityRightToWorkViewModel), "PrincipalAuthorityRightToWork"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.UndischargedBankruptViewModel), "UndischargedBankrupt"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.DisqualifiedDirectorViewModel), "DisqualifiedDirector"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.RestraintOrdersViewModel), "RestraintOrders", true),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.RestraintOrdersViewModel), "RestraintOrders"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.UnspentConvictionsViewModel), "UnspentConvictions", true),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.UnspentConvictionsViewModel), "UnspentConvictions"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.OffencesAwaitingTrialViewModel), "OffencesAwaitingTrial", true),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.OffencesAwaitingTrialViewModel), "OffencesAwaitingTrial"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PreviousLicenceViewModel), "PreviousLicence"),
                            new FormPageDefinition(nameof(PrincipalAuthorityViewModel.PreviousExperience), "PreviousExperience"),
                            new FormPageDefinition("Summary")
                        }).ToArray()
                    },
                    {
                        FormSection.AlternativeBusinessRepresentatives,
                        new []
                        {
                            new FormPageDefinition("Introduction"),
                            new FormPageDefinition("AlternativeBusinessRepresentatives"),
                            new FormPageDefinition("Summary")
                        }
                    },
                    {
                        FormSection.AlternativeBusinessRepresentative,
                        BasicPersonFields
                        .Concat(PersonSecurityFields)
                        .Concat(new[] {
                            new FormPageDefinition("Summary")
                        }).ToArray()
                    },
                    {
                        FormSection.DirectorOrPartner,
                        new[]
                        {
                            new FormPageDefinition(nameof(IsPreviousPrincipalAuthorityViewModel.IsPreviousPrincipalAuthority)),
                        }
                        .Concat(BasicPersonFields)
                        .Concat(PersonSecurityFields)
                        .Concat(new [] {
                            new FormPageDefinition("Summary")
                        }).ToArray()                        
                    },
                    {
                        FormSection.DirectorsOrPartners,
                        new []
                        {
                            new FormPageDefinition("Introduction"),
                            new FormPageDefinition("DirectorsOrPartners"),
                            new FormPageDefinition("Summary")
                        }
                    },
                    {
                        FormSection.NamedIndividual,
                        new[]
                        {
                            new FormPageDefinition(nameof(NamedIndividualViewModel.FullName), "FullName"),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.DateOfBirth), "DateOfBirth"),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.BusinessPhoneNumber), "BusinessPhoneNumber"),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.BusinessExtension), "BusinessExtension"),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.RightToWorkViewModel), "RightToWork"),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.UndischargedBankruptViewModel), "UndischargedBankrupt"),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.DisqualifiedDirectorViewModel), "DisqualifiedDirector"),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.RestraintOrdersViewModel), "RestraintOrders", true),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.RestraintOrdersViewModel), "RestraintOrders"),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.UnspentConvictionsViewModel), "UnspentConvictions", true),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.UnspentConvictionsViewModel), "UnspentConvictions"),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.OffencesAwaitingTrialViewModel), "OffencesAwaitingTrial", true),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.OffencesAwaitingTrialViewModel), "OffencesAwaitingTrial"),
                            new FormPageDefinition(nameof(NamedIndividualViewModel.PreviousLicenceViewModel), "PreviousLicence"),
                            new FormPageDefinition("Summary")
                        }
                    },
                    {
                        FormSection.JobTitle,
                        new[]
                        {
                            new FormPageDefinition("JobTitles"),
                            new FormPageDefinition("Summary")
                        }
                    },
                    {
                        FormSection.NamedIndividuals,
                        new []
                        {
                            new FormPageDefinition("Introduction"),
                            new FormPageDefinition("NamedIndividuals"),
                            new FormPageDefinition("Summary")
                        }
                    },
                    {
                        FormSection.Organisation,
                        new[]
                        {
                            new FormPageDefinition("Introduction"),
                            new FormPageDefinition(nameof(OrganisationViewModel.OutsideSectorsViewModel), "OutsideSectors"),
                            new FormPageDefinition(nameof(OrganisationViewModel.WrittenAgreementViewModel), "WrittenAgreement"),
                            new FormPageDefinition(nameof(OrganisationViewModel.PscControlledViewModel), "PSCControlled"),
                            new FormPageDefinition(nameof(OrganisationViewModel.MultipleBranchViewModel), "MultipleBranch"),
                            new FormPageDefinition(nameof(OrganisationViewModel.TransportingWorkersViewModel), "TransportingWorkers"),
                            new FormPageDefinition(nameof(OrganisationViewModel.AccommodatingWorkersViewModel), "AccommodatingWorkers"),
                            new FormPageDefinition(nameof(OrganisationViewModel.SourcingWorkersViewModel), "SourcingWorkers"),
                            new FormPageDefinition(nameof(OrganisationViewModel.WorkerSupplyMethodViewModel), "WorkerSupplyMethod"),
                            new FormPageDefinition(nameof(OrganisationViewModel.WorkerContractViewModel), "WorkerContract"),
                            new FormPageDefinition(nameof(OrganisationViewModel.BannedFromTradingViewModel), "BannedFromTrading"),
                            new FormPageDefinition(nameof(OrganisationViewModel.SubcontractorViewModel), "Subcontractor"),
                            new FormPageDefinition(nameof(OrganisationViewModel.ShellfishWorkerNumberViewModel), "ShellfishWorkerNumber"),
                            new FormPageDefinition(nameof(OrganisationViewModel.ShellfishWorkerNationalityViewModel), "ShellfishWorkerNationality"),
                            new FormPageDefinition(nameof(OrganisationViewModel.PreviouslyWorkedInShellfishViewModel), "PreviouslyWorkedInShellfish"),
                            new FormPageDefinition("Summary")
                        }
                    }
                };
        }

        private static readonly FormPageDefinition[] BasicPersonFields =
        {
            new FormPageDefinition(nameof(PersonViewModel.FullName), "FullName"),
            new FormPageDefinition(nameof(PersonViewModel.AlternativeName), "AlternativeName"),
            new FormPageDefinition(nameof(PersonViewModel.DateOfBirth), "DateOfBirth"),
            new FormPageDefinition(nameof(PersonViewModel.BirthDetailsViewModel), "BirthDetails"),
            new FormPageDefinition(nameof(PersonViewModel.JobTitle), "JobTitle"),
            new FormPageDefinition(nameof(PersonViewModel.Address), "Address"),
            new FormPageDefinition(nameof(PersonViewModel.BusinessPhoneNumber), "BusinessPhoneNumber"),
            new FormPageDefinition(nameof(PersonViewModel.BusinessExtension), "BusinessExtension"),
            new FormPageDefinition(nameof(PersonViewModel.PersonalMobileNumber), "PersonalMobileNumber"),
            new FormPageDefinition(nameof(PersonViewModel.PersonalEmailAddress), "PersonalEmailAddress"),
        };

        private static readonly FormPageDefinition[] PersonSecurityFields =
        {
            new FormPageDefinition(nameof(PersonViewModel.Nationality), "Nationality"),
            new FormPageDefinition(nameof(PersonViewModel.PassportViewModel), "Passport"),
            new FormPageDefinition(nameof(RightToWorkViewModel), "RightToWork"),
            new FormPageDefinition(nameof(PersonViewModel.UndischargedBankruptViewModel), "UndischargedBankrupt"),
            new FormPageDefinition(nameof(PersonViewModel.DisqualifiedDirectorViewModel), "DisqualifiedDirector"),
            new FormPageDefinition(nameof(PersonViewModel.RestraintOrdersViewModel), "RestraintOrders", true),
            new FormPageDefinition(nameof(PersonViewModel.RestraintOrdersViewModel), "RestraintOrders"),
            new FormPageDefinition(nameof(PersonViewModel.UnspentConvictionsViewModel), "UnspentConvictions", true),
            new FormPageDefinition(nameof(PersonViewModel.UnspentConvictionsViewModel), "UnspentConvictions"),
            new FormPageDefinition(nameof(PersonViewModel.OffencesAwaitingTrialViewModel), "OffencesAwaitingTrial", true),
            new FormPageDefinition(nameof(PersonViewModel.OffencesAwaitingTrialViewModel), "OffencesAwaitingTrial"),
            new FormPageDefinition(nameof(PersonViewModel.PreviousLicenceViewModel), "PreviousLicence"),
        };

        public IDictionary<FormSection, FormPageDefinition[]> Fields { get; set; }
    }
}