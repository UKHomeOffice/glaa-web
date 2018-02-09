using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class EligibilityProfile : Profile
    {
        public EligibilityProfile()
        {
            CreateMap<GLAAUser, SignUpViewModel>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(y => y))
                .ForMember(x => x.EmailAddress, opt => opt.MapFrom(y => y))
                .ForMember(x => x.Address, opt => opt.MapFrom(y => y.Address))
                .ForMember(x => x.CommunicationPreference, opt => opt.MapFrom(y => y))
                .ForMember(x => x.Password, opt => opt.MapFrom(y => y))
                .ForMember(x => x.Countries, opt => opt.Ignore())
                .ForMember(x => x.Counties, opt => opt.Ignore())
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<SignUpViewModel, GLAAUser>()
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

            CreateMap<GLAAUser, PasswordViewModel>()
                .ForMember(x => x.HasPassword, opt => opt.MapFrom(y => !string.IsNullOrEmpty(y.PasswordHash)))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthorityEmailAddressViewModel, GLAAUser>()
                .ForMember(x => x.Email, opt => opt.MapFrom(y => y.EmailAddress))
                .ForMember(x => x.UserName, opt => opt.MapFrom(y => y.EmailAddress))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthorityFullNameViewModel, GLAAUser>()
                .ForMember(x => x.Title, opt => opt.MapFrom(y => y.Title))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(y => y.FirstName))
                .ForMember(x => x.MiddleName, opt => opt.MapFrom(y => y.MiddleName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(y => y.LastName))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<CommunicationPreferenceViewModel, GLAAUser>()
                .ForMember(x => x.CommunicationPreference, opt => opt.MapFrom(y => y.CommunicationPreference))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
