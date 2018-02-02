using System.Collections.Generic;
using System.Linq;
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
                            new FormPageDefinition("Index", null)
                        }
                    },
                    {
                        FormSection.Eligibility,
                        new []
                        {
                            new FormPageDefinition("Eligibility.1", typeof(SuppliesWorkersViewModel)), 
                            new FormPageDefinition("Eligibility.2", typeof(OperatingIndustriesViewModel)), 
                            new FormPageDefinition("Eligibility.3", typeof(TurnoverViewModel)),
                            new FormPageDefinition("Eligibility.4", typeof(EligibilitySummaryViewModel)),
                            new FormPageDefinition("Eligibility.5", null)
                        }
                    },
                    {
                        FormSection.OrganisationDetails,
                        new[]
                        {
                            new FormPageDefinition("OrganisationDetailsIntroduction", null),
                            new FormPageDefinition(nameof(BusinessNameViewModel.BusinessName), typeof(BusinessNameViewModel)),
                            new FormPageDefinition("Address", typeof(AddressViewModel)),
                            new FormPageDefinition(nameof(BusinessPhoneNumberViewModel.BusinessPhoneNumber), typeof(BusinessPhoneNumberViewModel)),
                            new FormPageDefinition(nameof(BusinessMobileNumberViewModel.BusinessMobileNumber), typeof(BusinessMobileNumberViewModel)),
                            new FormPageDefinition(nameof(BusinessEmailAddressViewModel.BusinessEmailAddress), typeof(BusinessEmailAddressViewModel)),
                            new FormPageDefinition(nameof(BusinessWebsiteViewModel.BusinessWebsite), typeof(BusinessWebsiteViewModel)),
                            new FormPageDefinition(nameof(CommunicationPreferenceViewModel.CommunicationPreference), typeof(CommunicationPreferenceViewModel)),
                            new FormPageDefinition(nameof(LegalStatusViewModel.LegalStatus), typeof(LegalStatusViewModel)),
                            new FormPageDefinition("PAYEERNStatus", typeof(PAYEERNStatusViewModel)),
                            new FormPageDefinition("VATStatus", typeof(VATStatusViewModel)),
                            new FormPageDefinition("TaxReference", typeof(TaxReferenceViewModel)),
                            new FormPageDefinition(nameof(OperatingIndustriesViewModel.OperatingIndustries), typeof(OperatingIndustriesViewModel)),
                            new FormPageDefinition("Turnover", typeof(TurnoverViewModel)),
                            new FormPageDefinition(nameof(OperatingCountriesViewModel.OperatingCountries), typeof(OperatingCountriesViewModel)),
                            new FormPageDefinition("OrganisationDetailsSummary", null)
                        }
                    },
                    {
                        FormSection.PrincipalAuthority,
                        new[]
                        {
                            new FormPageDefinition("PricipalAuthority.1", null),
                            new FormPageDefinition("PricipalAuthority.2", typeof(IsDirectorViewModel)),
                            new FormPageDefinition("PricipalAuthority.3", typeof(PrincipalAuthorityConfirmationViewModel)),
                        }
                        .Concat(BasicPersonFields)
                        .Concat(new [] {
                            new FormPageDefinition("PricipalAuthority.16", typeof(NationalityViewModel)),
                            new FormPageDefinition("PricipalAuthority.17", typeof(PassportViewModel)),
                            new FormPageDefinition("PricipalAuthority.18", typeof(PrincipalAuthorityRightToWorkViewModel)),
                            new FormPageDefinition("PricipalAuthority.19", typeof(UndischargedBankruptViewModel)),
                            new FormPageDefinition("PricipalAuthority.20", typeof(DisqualifiedDirectorViewModel)),
                            new FormPageDefinition("PricipalAuthority.21", typeof(RestraintOrdersViewModel), true),
                            new FormPageDefinition("PricipalAuthority.22", typeof(RestraintOrdersViewModel)),
                            new FormPageDefinition("PricipalAuthority.23", typeof(UnspentConvictionsViewModel), true),
                            new FormPageDefinition("PricipalAuthority.24", typeof(UnspentConvictionsViewModel)),
                            new FormPageDefinition("PricipalAuthority.25", typeof(OffencesAwaitingTrialViewModel), true),
                            new FormPageDefinition("PricipalAuthority.26", typeof(OffencesAwaitingTrialViewModel)),
                            new FormPageDefinition("PricipalAuthority.27", typeof(PreviousLicenceViewModel)),
                            new FormPageDefinition("PricipalAuthority.28", typeof(PreviousExperienceViewModel)),
                            new FormPageDefinition("PricipalAuthority.29", null)
                        }).ToArray()
                    },
                    {
                        FormSection.AlternativeBusinessRepresentatives,
                        new []
                        {
                            new FormPageDefinition("AlternativeBusinessRepresentatives.1", null),
                            new FormPageDefinition("AlternativeBusinessRepresentatives.2", null),
                            new FormPageDefinition("AlternativeBusinessRepresentatives.3", null)
                        }
                    },
                    {
                        FormSection.AlternativeBusinessRepresentative,
                        BasicPersonFields
                        .Concat(PersonSecurityFields)
                        .Concat(new[] {
                            new FormPageDefinition("AlternativeBusinessRepresentative.25", null)
                        }).ToArray()
                    },
                    {
                        FormSection.DirectorOrPartner,
                        new[]
                        {
                            new FormPageDefinition("DirectorOrPartner.1", typeof(IsPreviousPrincipalAuthorityViewModel)),
                        }
                        .Concat(BasicPersonFields)
                        .Concat(PersonSecurityFields)
                        .Concat(new [] {
                            new FormPageDefinition("DirectorOrPartner.26", null)
                        }).ToArray()                        
                    },
                    {
                        FormSection.DirectorsOrPartners,
                        new []
                        {
                            new FormPageDefinition("DirectorsOrPartners.1", null),
                            new FormPageDefinition("DirectorsOrPartners.2", null),
                            new FormPageDefinition("DirectorsOrPartners.3", null)
                        }
                    },
                    {
                        FormSection.NamedIndividual,
                        new[]
                        {
                            new FormPageDefinition("NamedIndividual.1", typeof(FullNameViewModel)),
                            new FormPageDefinition("NamedIndividual.2", typeof(DateOfBirthViewModel)),
                            new FormPageDefinition("NamedIndividual.3", typeof(BusinessPhoneNumberViewModel)),
                            new FormPageDefinition("NamedIndividual.4", typeof(BusinessExtensionViewModel)),
                            new FormPageDefinition("NamedIndividual.5", typeof(RightToWorkViewModel)),
                            new FormPageDefinition("NamedIndividual.6", typeof(UndischargedBankruptViewModel)),
                            new FormPageDefinition("NamedIndividual.7", typeof(DisqualifiedDirectorViewModel)),
                            new FormPageDefinition("NamedIndividual.8", typeof(RestraintOrdersViewModel), true),
                            new FormPageDefinition("NamedIndividual.9", typeof(RestraintOrdersViewModel)),
                            new FormPageDefinition("NamedIndividual.10", typeof(UnspentConvictionsViewModel), true),
                            new FormPageDefinition("NamedIndividual.11", typeof(UnspentConvictionsViewModel)),
                            new FormPageDefinition("NamedIndividual.12", typeof(OffencesAwaitingTrialViewModel), true),
                            new FormPageDefinition("NamedIndividual.13", typeof(OffencesAwaitingTrialViewModel)),
                            new FormPageDefinition("NamedIndividual.14", typeof(PreviousLicenceViewModel)),
                            new FormPageDefinition("NamedIndividual.15", null)
                        }
                    },
                    {
                        FormSection.JobTitle,
                        new[]
                        {
                            new FormPageDefinition("JobTitle.1", null),
                            new FormPageDefinition("JobTitle.2", null)
                        }
                    },
                    {
                        FormSection.NamedIndividuals,
                        new []
                        {
                            new FormPageDefinition("NamedIndividuals.1", null),
                            new FormPageDefinition("NamedIndividuals.2", null),
                            new FormPageDefinition("NamedIndividuals.3", null)
                        }
                    },
                    {
                        FormSection.Organisation,
                        new[]
                        {
                            new FormPageDefinition("Organisation.1", null),
                            new FormPageDefinition("Organisation.2", typeof(OutsideSectorsViewModel)),
                            new FormPageDefinition("Organisation.3", typeof(WrittenAgreementViewModel)),
                            new FormPageDefinition("Organisation.4", typeof(PSCControlledViewModel)),
                            new FormPageDefinition("Organisation.5", typeof(MultipleBranchViewModel)),
                            new FormPageDefinition("Organisation.6", typeof(TransportingWorkersViewModel)),
                            new FormPageDefinition("Organisation.7", typeof(AccommodatingWorkersViewModel)),
                            new FormPageDefinition("Organisation.8", typeof(SourcingWorkersViewModel)),
                            new FormPageDefinition("Organisation.9", typeof(WorkerSupplyMethodViewModel)),
                            new FormPageDefinition("Organisation.10", typeof(WorkerContractViewModel)),
                            new FormPageDefinition("Organisation.11", typeof(BannedFromTradingViewModel)),
                            new FormPageDefinition("Organisation.12", typeof(SubcontractorViewModel)),
                            new FormPageDefinition("Organisation.13", typeof(ShellfishWorkerNumberViewModel)),
                            new FormPageDefinition("Organisation.14", typeof(ShellfishWorkerNationalityViewModel)),
                            new FormPageDefinition("Organisation.15", typeof(PreviouslyWorkedInShellfishViewModel)),
                            new FormPageDefinition("Organisation.16", null)
                        }
                    }
                };
        }

        private static readonly FormPageDefinition[] BasicPersonFields =
        {
            new FormPageDefinition("", typeof(FullNameViewModel)),
            new FormPageDefinition("", typeof(AlternativeFullNameViewModel)),
            new FormPageDefinition("", typeof(DateOfBirthViewModel)),
            new FormPageDefinition("", typeof(TownOfBirthViewModel)),
            new FormPageDefinition("", typeof(CountryOfBirthViewModel)),
            new FormPageDefinition("", typeof(JobTitleViewModel)),
            new FormPageDefinition("", typeof(AddressViewModel)),
            new FormPageDefinition("", typeof(BusinessPhoneNumberViewModel)),
            new FormPageDefinition("", typeof(BusinessExtensionViewModel)),
            new FormPageDefinition("", typeof(PersonalMobileNumberViewModel)),
            new FormPageDefinition("", typeof(PersonalEmailAddressViewModel)),
            new FormPageDefinition("", typeof(NationalInsuranceNumberViewModel)),
        };

        private static readonly FormPageDefinition[] PersonSecurityFields =
        {
            new FormPageDefinition("", typeof(NationalityViewModel)),
            new FormPageDefinition("", typeof(PassportViewModel)),
            new FormPageDefinition("", typeof(RightToWorkViewModel)),
            new FormPageDefinition("", typeof(UndischargedBankruptViewModel)),
            new FormPageDefinition("", typeof(DisqualifiedDirectorViewModel)),
            new FormPageDefinition("", typeof(RestraintOrdersViewModel), true),
            new FormPageDefinition("", typeof(RestraintOrdersViewModel)),
            new FormPageDefinition("", typeof(UnspentConvictionsViewModel), true),
            new FormPageDefinition("", typeof(UnspentConvictionsViewModel)),
            new FormPageDefinition("", typeof(OffencesAwaitingTrialViewModel), true),
            new FormPageDefinition("", typeof(OffencesAwaitingTrialViewModel)),
            new FormPageDefinition("", typeof(PreviousLicenceViewModel)),
        };

        public IDictionary<FormSection, FormPageDefinition[]> Fields { get; set; }
    }
}