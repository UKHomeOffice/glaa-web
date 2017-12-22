using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Core.Models;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class EligibilityProfile : Profile
    {
        public EligibilityProfile()
        {
            CreateMap<Licence, EligibilityViewModel>()
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.SuppliesWorkers, opt => opt.ResolveUsing(SuppliesWorkersResolver))
                .ForMember(x => x.Turnover, opt => opt.MapFrom(y => y))
                .ForMember(x => x.EligibilitySummary, opt => opt.ResolveUsing(RegistrationResolver))
                .ForMember(x => x.OperatingIndustries, opt => opt.ResolveUsing(ProfileHelpers.OperatingIndustriesResolver));

            CreateMap<EligibilityViewModel, Licence>()
                .ForMember(x => x.SuppliesWorkers, opt => opt.MapFrom(y => y.SuppliesWorkers.SuppliesWorkers))
                .ForMember(x => x.TurnoverBand, opt => opt.MapFrom(y => y.Turnover.TurnoverBand))
                .ForMember(x => x.OtherOperatingIndustry, opt => opt.MapFrom(y => y.OperatingIndustries.OtherIndustry))
                .ForMember(x => x.ContinueApplication, opt => opt.MapFrom(y => y.EligibilitySummary.ContinueApplication))
                .ForMember(x => x.OperatingIndustries, opt => opt.Ignore())
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<EligibilitySummaryViewModel, Licence>()
                .ForMember(x => x.ContinueApplication, opt => opt.MapFrom(y => y.ContinueApplication))
                .ForMember(x => x.EmailAlreadyRegistered, opt => opt.MapFrom(y => y.EmailAlreadyRegistered))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<SuppliesWorkersViewModel, Licence>()
                .ForMember(x => x.SuppliesWorkers, opt => opt.MapFrom(y => y.SuppliesWorkers))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<ICollection<Industry>, OperatingIndustriesViewModel>()
                .ForMember(x => x.OtherIndustry, opt => opt.Ignore())
                .ForMember(x => x.OperatingIndustries, opt => opt.MapFrom(y => y));
        }

        private EligibilitySummaryViewModel RegistrationResolver(Licence licence)
        {
            return new EligibilitySummaryViewModel
            {
                ContinueApplication = licence.ContinueApplication,
                EmailAlreadyRegistered = licence.EmailAlreadyRegistered,
                Email = licence.User?.Email,
                ApplicationFee = GetApplicationFee(licence.TurnoverBand),
                InspectionFee = GetInspectionFee(licence.TurnoverBand),
                SuppliesWorkersText = GetSuppliesWorkersText(licence.SuppliesWorkers),
                IndustriesText = GetIndustriesText(licence.OperatingIndustries, licence.OtherOperatingIndustry)
            };
        }

        private string GetIndustriesText(IEnumerable<LicenceIndustry> industries, string otherOperatingIndustry)
        {
            return string.IsNullOrEmpty(otherOperatingIndustry) || industries.Any() 
                ? "You operate in a regulated sector" 
                : "You do not operate in one of the regulated sectors";
        }

        private string GetSuppliesWorkersText(bool? suppliesWorkers)
        {
            return suppliesWorkers.HasValue && suppliesWorkers.Value
                ? "You supply workers"
                : "You do not supply workers";
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

        private SuppliesWorkersViewModel SuppliesWorkersResolver(Licence licence)
        {
            return new SuppliesWorkersViewModel
            {
                SuppliesWorkers = licence.SuppliesWorkers
            };
        }
    }
}
