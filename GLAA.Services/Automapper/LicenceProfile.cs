using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class LicenceProfile : Profile
    {
        public LicenceProfile()
        {
            CreateMap<Licence, LicenceApplicationViewModel>()
                .ForMember(x => x.LastSection, opt => opt.Ignore())
                .ForMember(x => x.LastPage, opt => opt.Ignore())
                .ForMember(x => x.IsApplication, opt => opt.Ignore())
                .ForMember(x => x.Declaration, opt => opt.Ignore())
                .ForMember(x => x.SignUp, opt => opt.Ignore())
                .ForMember(x => x.OrganisationDetails, opt => opt.MapFrom(y => y))
                .ForMember(x => x.PrincipalAuthority, opt => opt.Ignore())
                .ForMember(x => x.AlternativeBusinessRepresentatives, opt => opt.Ignore())
                .ForMember(x => x.DirectorOrPartner, opt => opt.Ignore())
                .ForMember(x => x.NamedIndividuals, opt => opt.Ignore())
                .ForMember(x => x.Organisation, opt => opt.MapFrom(y => y))
                .ForMember(x => x.NewLicenceStatus, opt => opt.Ignore())
                .ForMember(x => x.YesNo, opt => opt.Ignore())
                .ForMember(x => x.IsValid, opt => opt.Ignore());

            CreateMappings();
        }

        private void CreateMappings()
        {

            // IPerson
            CreateMap<string, TownOfBirthViewModel>()
                .ForMember(x => x.TownOfBirth, opt => opt.MapFrom(y => y));

            CreateMap<string, CountryOfBirthViewModel>()
                .ForMember(x => x.CountryOfBirth, opt => opt.MapFrom(y => y));

            CreateMap<string, JobTitleViewModel>()
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(y => y));

            CreateMap<string, BusinessPhoneNumberViewModel>()
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.MapFrom(y => y));

            CreateMap<string, BusinessMobileNumberViewModel>()
                .ForMember(x => x.BusinessMobileNumber, opt => opt.MapFrom(y => y));

            CreateMap<string, BusinessExtensionViewModel>()
                .ForMember(x => x.BusinessExtension, opt => opt.MapFrom(y => y));

            CreateMap<string, PersonalEmailAddressViewModel>()
                .ForMember(x => x.PersonalEmailAddress, opt => opt.MapFrom(y => y));

            CreateMap<string, PersonalMobileNumberViewModel>()
                .ForMember(x => x.PersonalMobileNumber, opt => opt.MapFrom(y => y));

            CreateMap<string, NationalityViewModel>()
                .ForMember(x => x.Nationality, opt => opt.MapFrom(y => y));

            CreateMap<CommunicationPreferenceViewModel, CommunicationPreference?>()
                .ConvertUsing(MapNullableCommunicationPreference);

            CreateMap<CommunicationPreference?, CommunicationPreferenceViewModel>()
                .ForMember(x => x.CommunicationPreference, opt => opt.MapFrom(y => y))
                .ForMember(x => x.AvailableCommunicationPreferences, opt => opt.Ignore());

            CreateMap<LegalStatusViewModel, LegalStatusEnum?>()
                .ConvertUsing(MapNullableLegalStatus);

            CreateMap<IndustryViewModel, Industry>()
                .ForMember(x => x.Licences, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CountryViewModel, Country>()
                .ForMember(x => x.Licences, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<SectorViewModel, Sector>()
                .ForMember(x => x.Licences, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<MultipleViewModel, Multiple>()
                .ForMember(x => x.Licences, opt => opt.Ignore())
                .ReverseMap();

            // organisation details
            CreateMap<string, BusinessWebsiteViewModel>()
                .ForMember(x => x.BusinessWebsite, opt => opt.MapFrom(y => y));

            // ABR
            CreateMap<AlternativeBusinessRepresentativeCollectionViewModel, Licence>()
                .ForMember(x => x.HasAlternativeBusinessRepresentatives,
                    opt => opt.MapFrom(y => y.HasAlternativeBusinessRepresentatives))
                .ForAllOtherMembers(opt => opt.Ignore());

            // DOP
            CreateMap<DirectorOrPartnerCollectionViewModel, Licence>()
                .ForMember(x => x.NumberOfDirectorsOrPartners, opt => opt.MapFrom(y => y.NumberOfDirectorsOrPartners))
                .ForAllOtherMembers(opt => opt.Ignore());

            // NI
            CreateMap<NamedIndividualCollectionViewModel, Licence>()
                .ForMember(x => x.NamedIndividualType, opt => opt.MapFrom(y => y.NamedIndividualType))
                .ForAllOtherMembers(opt => opt.Ignore());

            // status history
            CreateMap<Licence, LicenceStatusHistoryViewModel>()
                .ForAllMembers(opt => opt.Ignore());

            CreateMap<LicenceStatusHistoryViewModel, Licence>()
                .ForAllMembers(opt => opt.Ignore());

        }

        private static CommunicationPreference? MapNullableCommunicationPreference(CommunicationPreferenceViewModel model)
        {
            return model.CommunicationPreference;
        }

        private static LegalStatusEnum? MapNullableLegalStatus(LegalStatusViewModel model)
        {
            return model.LegalStatus;
        }
    }
}