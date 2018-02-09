using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class StatusProfile : Profile
    {
        public StatusProfile()
        {
            CreateMap<LicenceStatusChange, LicenceStatusViewModel>()
                .ConvertUsing(MapLicenceStatusViewModel);
            CreateMap<LicenceStatus, LicenceStatusViewModel>()
                .ForMember(x => x.DateCreated, opt => opt.Ignore())
                .ForMember(x => x.SelectedReason, opt => opt.Ignore())
                .ForMember(x => x.NonCompliantStandards, opt => opt.Ignore())
                .ForMember(x => x.NextStatuses, opt => opt.Ignore());
        }

        /// <summary>
        /// This needs to be reused in other places
        /// TODO: Is this the best way?
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        public static LicenceStatusViewModel MapLicenceStatusViewModel(LicenceStatusChange change)
        {
            return new LicenceStatusViewModel
            {
                Id = change.Status.Id,
                ExternalDescription = change.Status?.ExternalDescription,
                InternalStatus = change.Status?.InternalStatus,
                InternalDescription = change.Status?.InternalDescription,
                ActiveCheckDescription = change.Status?.ActiveCheckDescription,
                ShowInPublicRegister = change.Status.ShowInPublicRegister,
                RequireNonCompliantStandards = change.Status.RequireNonCompliantStandards,
                DateCreated = change.DateCreated,
                NonCompliantStandards = change.NonCompliantStandards?.Select(s => MapStandard(s.LicensingStandard)),
                NextStatuses = change.Status.NextStatuses?.Select(x => MapLicenceStatus(x.NextStatus)),
                SelectedReason = change.Reason?.Description,
                CssClassStem = change.Status.CssClassStem,
                AdminCategory = change.Status.AdminCategory,
                LicenceIssued = change.Status.LicenceIssued,
                LicenceSubmitted = change.Status.LicenceSubmitted
            };
        }

        public static NextStatusViewModel MapLicenceStatus(LicenceStatus status)
        {
            return new NextStatusViewModel
            {
                Id = status.Id,
                InternalStatus = status.InternalStatus ?? string.Empty,
                InternalDescription = status.InternalDescription ?? string.Empty,
                RequireNonCompliantStandards = status.RequireNonCompliantStandards,
                Reasons = status.StatusReasons?.Select(MapReason)
            };
        }

        public static ReasonViewModel MapReason(StatusReason reason)
        {
            return new ReasonViewModel
            {
                Id = reason.Id,
                Description = reason.Description
            };
        }

        public static LicensingStandardViewModel MapStandard(LicensingStandard standard)
        {
            return new LicensingStandardViewModel
            {
                Id = standard.Id,
                IsCritical = standard.IsCritical,
                Name = standard.Name
            };
        }
    }
}
