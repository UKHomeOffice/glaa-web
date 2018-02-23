using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels.PublicRegister;

namespace GLAA.Services.Automapper
{
    public class PublicRegisterProfile : Profile
    {
        public PublicRegisterProfile()
        {
            CreateMap<Licence, PublicRegisterLicenceSummaryViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.ApplicationId, opt => opt.MapFrom(y => y.ApplicationId))
                .ForMember(x => x.MostRecentStatus, opt => opt.MapFrom(y => StatusProfile.MapLicenceStatusViewModel(LicenceRepository.GetLatestStatus(y))))
                .ForMember(x => x.BusinessName, opt => opt.MapFrom(y => y.BusinessName))
                .ForMember(x => x.TradingName, opt => opt.MapFrom(y => y.TradingName))
                .ForMember(x => x.CountryId, opt => opt.MapFrom(y => y.Address.CountryId))
                .ForMember(x => x.CountyId, opt => opt.MapFrom(y => y.Address.CountyId))
                .ForMember(x => x.IsApplication, opt => opt.MapFrom(y => LicenceRepository.GetLatestStatus(y).Status.IsApplication))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Licence, PublicRegisterLicenceDetailViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.ApplicationId, opt => opt.MapFrom(y => y.ApplicationId))
                .ForMember(x => x.MostRecentStatus, opt => opt.MapFrom(y => StatusProfile.MapLicenceStatusViewModel(LicenceRepository.GetLatestStatus(y))))
                .ForMember(x => x.MostRecentLicenceIssuedStatus, opt => opt.MapFrom(y => StatusProfile.MapLicenceStatusViewModel(LicenceRepository.GetLatestLicenceIssueStatus(y))))
                .ForMember(x => x.MostRecentLicenceSubmittedStatus, opt => opt.MapFrom(y => StatusProfile.MapLicenceStatusViewModel(LicenceRepository.GetLatestLicenceSubmissionStatus(y))))
                .ForMember(x => x.BusinessName, opt => opt.MapFrom(y => y.BusinessName))
                .ForMember(x => x.TradingName, opt => opt.MapFrom(y => y.TradingName))
                .ForMember(x => x.IsApplication, opt => opt.MapFrom(y => LicenceRepository.GetLatestStatus(y).Status.IsApplication))
                .ForMember(x => x.Address, opt => opt.MapFrom(y => y.Address))
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.MapFrom(y => y.BusinessPhoneNumber))
                .ForMember(x => x.BusinessType, opt => opt.MapFrom(y => y.LegalStatus))
                .ForMember(x => x.OperatingCountries, opt => opt.MapFrom(y => y.OperatingCountries))
                .ForMember(x => x.PrincipalAuthorities, opt => opt.MapFrom(y => y.PrincipalAuthorities))
                .ForMember(x => x.OperatingIndustries, opt => opt.MapFrom(y => y.OperatingIndustries))
                .ForMember(x => x.NamedIndividuals, opt => opt.MapFrom(y => y.NamedIndividuals))
                .ForMember(x => x.NamedPosts, opt => opt.MapFrom(y => y.NamedJobTitles))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
