using System.Collections.Generic;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class SecurityQuestionsProfile : Profile
    {
        public SecurityQuestionsProfile()
        {
            CreateMap<RestraintOrder, RestraintOrderViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.Description, opt => opt.MapFrom(y => y.Description))
                .ForMember(x => x.Date, opt => opt.MapFrom(y => y.Date))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<RestraintOrderViewModel, RestraintOrder>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Description, opt => opt.MapFrom(y => y.Description))
                .ForMember(x => x.Date, opt => opt.MapFrom(y => y.Date.Date))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Conviction, UnspentConvictionViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.Description, opt => opt.MapFrom(y => y.Description))
                .ForMember(x => x.Date, opt => opt.MapFrom(y => y.Date))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<UnspentConvictionViewModel, Conviction>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Description, opt => opt.MapFrom(y => y.Description))
                .ForMember(x => x.Date, opt => opt.MapFrom(y => y.Date.Date))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<OffenceAwaitingTrial, OffenceAwaitingTrialViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.Description, opt => opt.MapFrom(y => y.Description))
                .ForMember(x => x.Date, opt => opt.MapFrom(y => y.Date))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<OffenceAwaitingTrialViewModel, OffenceAwaitingTrial>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Description, opt => opt.MapFrom(y => y.Description))
                .ForMember(x => x.Date, opt => opt.MapFrom(y => y.Date.Date))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PreviousTradingName, PreviousTradingNameViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.BusinessName, opt => opt.MapFrom(y => y.BusinessName))
                .ForMember(x => x.Town, opt => opt.MapFrom(y => y.Town))
                .ForMember(x => x.Country, opt => opt.MapFrom(y => y.Country))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PreviousTradingNameViewModel, PreviousTradingName>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.BusinessName, opt => opt.MapFrom(y => y.BusinessName))
                .ForMember(x => x.Town, opt => opt.MapFrom(y => y.Town))
                .ForMember(x => x.Country, opt => opt.MapFrom(y => y.Country))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
