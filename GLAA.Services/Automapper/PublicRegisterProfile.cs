using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Services.PublicRegister;
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
                .ForMember(x => x.MostRecentStatus, opt => opt.MapFrom(y => StatusProfile.MapLicenceStatusViewModel(PublicRegisterViewModelBuilder.GetLatestStatus(y))))
                .ForMember(x => x.OrganisationName, opt => opt.MapFrom(y => y.OrganisationName))
                .ForMember(x => x.TradingName, opt => opt.MapFrom(y => y.TradingName))
                .ForMember(x => x.Country, opt => opt.MapFrom(y => y.Address.Country))
                .ForMember(x => x.County, opt => opt.MapFrom(y => y.Address.County))
                .ForMember(x => x.IsApplication, opt => opt.MapFrom(y => PublicRegisterViewModelBuilder.GetLatestStatus(y).Status.IsApplication))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
