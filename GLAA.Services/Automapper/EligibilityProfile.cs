using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class EligibilityProfile : Profile
    {
        public EligibilityProfile()
        {
            CreateMap<GLAAUser, EligibilityViewModel>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(y => y))
                .ForMember(x => x.EmailAddress, opt => opt.MapFrom(y => y))
                .ForMember(x => x.Address, opt => opt.MapFrom(y => y.Address))
                .ForMember(x => x.CommunicationPreference, opt => opt.MapFrom(y => y))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<EligibilityViewModel, GLAAUser>()
                .ForMember(x => x.Title, opt => opt.MapFrom(y => y.FullName.Title))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(y => y.FullName.FirstName))
                .ForMember(x => x.MiddleName, opt => opt.MapFrom(y => y.FullName.MiddleName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(y => y.FullName.LastName))
                .ForMember(x => x.Address, opt => opt.MapFrom(y => y.Address))
                .ForMember(x => x.CommunicationPreference, opt => opt.MapFrom(y => y.CommunicationPreference.CommunicationPreference))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<GLAAUser, PrincipalAuthorityFullNameViewModel>()
                .ForMember(x => x.Title, opt => opt.MapFrom(y => y.Title))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(y => y.FirstName))
                .ForMember(x => x.MiddleName, opt => opt.MapFrom(y => y.MiddleName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(y => y.LastName))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<GLAAUser, PrincipalAuthorityEmailAddressViewModel>()
                .ForMember(x => x.EmailAddress, opt => opt.MapFrom(y => y.Email))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<GLAAUser, CommunicationPreferenceViewModel>()
                .ForMember(x => x.CommunicationPreference, opt => opt.MapFrom(y => y.CommunicationPreference))
                .ForAllOtherMembers(opt => opt.Ignore());
        }

        private int GetApplicationFee(TurnoverBand? turnover)
        {
            switch (turnover)
            {
                case TurnoverBand.UnderOneMillion:
                    return 400;
                case TurnoverBand.OneToFiveMillion:
                    return 1200;
                case TurnoverBand.FiveToTenMillion:
                    return 2000;
                case TurnoverBand.OverTenMillion:
                    return 2600;
                default:
                    return 0;
            }
        }

        private int GetInspectionFee(TurnoverBand? turnover)
        {
            switch (turnover)
            {
                case TurnoverBand.UnderOneMillion:
                    return 1850;
                case TurnoverBand.OneToFiveMillion:
                    return 2150;
                case TurnoverBand.FiveToTenMillion:
                    return 2400;
                case TurnoverBand.OverTenMillion:
                    return 2900;
                default:
                    return 0;
            }
        }
    }
}
