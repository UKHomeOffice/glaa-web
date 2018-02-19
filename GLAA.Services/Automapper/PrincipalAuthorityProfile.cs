using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class PrincipalAuthorityProfile : Profile
    {
        private IsDirectorViewModel DirectorResolver(PrincipalAuthority pa)
        {
            return new IsDirectorViewModel
            {
                IsDirector = pa.IsDirector,
                YesNo = ProfileHelpers.YesNoList
            };
        }

        private PrincipalAuthorityConfirmationViewModel ConfirmationResolver(PrincipalAuthority pa)
        {
            return new PrincipalAuthorityConfirmationViewModel
            {
                WillProvideConfirmation = pa.WillProvideConfirmation ?? false,
                IsDirector = pa.IsDirector
            };
        }

        public static PrincipalAuthorityRightToWorkViewModel PrincipalAuthorityRightToWorkResolver(PrincipalAuthority pa)
        {
            return new PrincipalAuthorityRightToWorkViewModel
            {
                RightToWorkInUk = pa.PermissionToWorkStatus,
                VisaNumber = pa.VisaNumber,
                ImmigrationStatus = pa.ImmigrationStatus,
                LeaveToRemainTo = new DateViewModel { Date = pa.LeaveToRemainTo },
                LengthOfUKWork = new TimeSpanViewModel
                {
                    Months = pa.LengthOfUKWorkMonths,
                    Years = pa.LengthOfUKWorkYears
                }
            };
        }

        public static TimeSpanViewModel LengthOfUKWorkResolver(PrincipalAuthority pa)
        {
            return new TimeSpanViewModel
            {
                Months = pa.LengthOfUKWorkMonths,
                Years = pa.LengthOfUKWorkYears
            };
        }

        public static PreviousExperienceViewModel PreviousExperienceResolver(PrincipalAuthority pa)
        {
            return new PreviousExperienceViewModel
            {
                PreviousExperience = pa.PreviousExperience
            };
        }

        public PrincipalAuthorityProfile()
        {
            CreateMap<PrincipalAuthority, PrincipalAuthorityViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.IsDirector, opt => opt.ResolveUsing(DirectorResolver))
                .ForMember(x => x.DirectorOrPartnerId, opt => opt.MapFrom(y => y.DirectorOrPartnerId))
                .ForMember(x => x.PrincipalAuthorityConfirmation, opt => opt.ResolveUsing(ConfirmationResolver))
                .ForMember(x => x.NationalInsuranceNumber, opt => opt.ResolveUsing(ProfileHelpers.NationalInsuranceNumberResolver))
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.FullName, opt => opt.ResolveUsing(ProfileHelpers.FullNameResolver))
                .ForMember(x => x.AlternativeName, opt => opt.ResolveUsing(ProfileHelpers.AlternativeFullNameResolver))
                .ForMember(x => x.TownOfBirth, opt => opt.ResolveUsing(ProfileHelpers.TownOfBirthResolver))
                .ForMember(x => x.CountryOfBirth, opt => opt.MapFrom(y => y.CountryOfBirthId))
                .ForMember(x => x.JobTitle, opt => opt.ResolveUsing(ProfileHelpers.JobTitleResolver))
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.ResolveUsing(ProfileHelpers.BusinessPhoneNumberResolver))
                .ForMember(x => x.BusinessExtension, opt => opt.ResolveUsing(ProfileHelpers.BusinessExtensionResolver))
                .ForMember(x => x.PersonalMobileNumber, opt => opt.ResolveUsing(ProfileHelpers.PersonalMobileNumberResolver))
                .ForMember(x => x.PersonalEmailAddress, opt => opt.ResolveUsing(ProfileHelpers.PersonalEmailAddressResolver))
                .ForMember(x => x.Nationality, opt => opt.ResolveUsing(ProfileHelpers.NationalityResolver))
                .ForMember(x => x.LegalStatus, opt => opt.MapFrom(y => y.Licence.LegalStatus))
                .ForMember(x => x.PassportViewModel, opt => opt.ResolveUsing(ProfileHelpers.PassportViewModel))
                .ForMember(x => x.PrincipalAuthorityRightToWorkViewModel, opt => opt.ResolveUsing(PrincipalAuthorityRightToWorkResolver))
                .ForMember(x => x.UndischargedBankruptViewModel, opt => opt.ResolveUsing(ProfileHelpers.UndischargedBankruptResolver))
                .ForMember(x => x.DisqualifiedDirectorViewModel, opt => opt.ResolveUsing(ProfileHelpers.DisqualifiedDirectorResolver))
                .ForMember(x => x.RestraintOrdersViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.UnspentConvictionsViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.OffencesAwaitingTrialViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.PreviousLicenceViewModel, opt => opt.ResolveUsing(ProfileHelpers.PreviousLicenceResolver))
                .ForMember(x => x.PreviousExperience, opt => opt.ResolveUsing(PreviousExperienceResolver))
                .ForMember(x => x.IsUk, opt => opt.MapFrom(y => y.CountryOfBirth != null && y.CountryOfBirth.IsUk))
                .ForMember(x => x.Counties, opt => opt.Ignore())
                .ForMember(x => x.Countries, opt => opt.Ignore());

            CreateMap<PrincipalAuthority, IsDirectorViewModel>()
                .ForMember(x => x.IsDirector, opt => opt.MapFrom(y => y.IsDirector))
                .ForMember(x => x.YesNo, opt => opt.Ignore());

            CreateMap<PrincipalAuthority, PrincipalAuthorityConfirmationViewModel>()
                .ConvertUsing(ConfirmationResolver);

            CreateMap<PrincipalAuthority, NationalInsuranceNumberViewModel>()
                .ConvertUsing(ProfileHelpers.NationalInsuranceNumberResolver);

            CreateMap<PrincipalAuthority, AlternativeFullNameViewModel>()
                .ConvertUsing(ProfileHelpers.AlternativeFullNameResolver);

            CreateMap<AlternativeFullNameViewModel, PrincipalAuthority>()                
                .ForMember(x => x.AlternativeName, opt => opt.MapFrom(y => y.AlternativeName))
                .ForMember(x => x.HasAlternativeName, opt => opt.MapFrom(y => y.HasAlternativeName))
                .ForAllOtherMembers(opt => opt.Ignore());

            // TODO: make IsCurrent work properly
            CreateMap<IsPreviousPrincipalAuthorityViewModel, PrincipalAuthority>()
                .ForMember(x => x.IsDirector, opt => opt.MapFrom(y => y.IsPreviousPrincipalAuthority))
                .ForMember(x => x.IsCurrent, opt => opt.UseValue(true))
                .ForAllOtherMembers(opt => opt.Ignore());

            // TODO: make IsCurrent work properly
            CreateMap<IsDirectorViewModel, PrincipalAuthority>()
                .ForMember(x => x.IsDirector, opt => opt.MapFrom(y => y.IsDirector))
                .ForMember(x => x.IsCurrent, opt => opt.UseValue(true))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthorityConfirmationViewModel, PrincipalAuthority>()
                .ForMember(x => x.WillProvideConfirmation, opt => opt.MapFrom(y => y.WillProvideConfirmation))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PreviousExperienceViewModel, PrincipalAuthority>()
                .ForMember(x => x.PreviousExperience, opt => opt.MapFrom(y => y.PreviousExperience))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<FullNameViewModel, PrincipalAuthority>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(y => y.FullName))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DateOfBirthViewModel, PrincipalAuthority>()
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(y => y.DateOfBirth.Date))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<TownOfBirthViewModel, PrincipalAuthority>()
                .ForMember(x => x.TownOfBirth, opt => opt.MapFrom(y => y.TownOfBirth))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<CountryOfBirthViewModel, PrincipalAuthority>()
                .ForMember(x => x.CountryOfBirthId, opt => opt.MapFrom(y => y.CountryOfBirthId))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<JobTitleViewModel, PrincipalAuthority>()
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(y => y.JobTitle))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BusinessPhoneNumberViewModel, PrincipalAuthority>()
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.MapFrom(y => y.BusinessPhoneNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PersonalMobileNumberViewModel, PrincipalAuthority>()
                .ForMember(x => x.PersonalMobileNumber, opt => opt.MapFrom(y => y.PersonalMobileNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PersonalEmailAddressViewModel, PrincipalAuthority>()
                .ForMember(x => x.PersonalEmailAddress, opt => opt.MapFrom(y => y.PersonalEmailAddress))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BusinessExtensionViewModel, PrincipalAuthority>()
                .ForMember(x => x.BusinessExtension, opt => opt.MapFrom(y => y.BusinessExtension))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NationalInsuranceNumberViewModel, PrincipalAuthority>()
                .ForMember(x => x.NationalInsuranceNumber, opt => opt.MapFrom(y => y.NationalInsuranceNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NationalityViewModel, PrincipalAuthority>()
                .ForMember(x => x.Nationality, opt => opt.MapFrom(y => y.Nationality))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthority, NationalityViewModel>()
                .ForMember(x => x.Nationality, opt => opt.MapFrom(y => y.Nationality))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PassportViewModel, PrincipalAuthority>()
                .ForMember(x => x.HasPassport, opt => opt.MapFrom(y => y.HasPassport))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthority, PassportViewModel>()
                .ForMember(x => x.HasPassport, opt => opt.MapFrom(y => y.HasPassport))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthorityRightToWorkViewModel, PrincipalAuthority>()
                .ForMember(x => x.PermissionToWorkStatus, opt => opt.MapFrom(y => y.RightToWorkInUk))
                .ForMember(x => x.VisaNumber, opt => opt.MapFrom(y => y.VisaNumber))
                .ForMember(x => x.ImmigrationStatus, opt => opt.MapFrom(y => y.ImmigrationStatus))
                .ForMember(x => x.LeaveToRemainTo, opt => opt.MapFrom(y => y.LeaveToRemainTo.Date))
                .ForMember(x => x.LengthOfUKWorkYears, opt => opt.MapFrom(y => y.LengthOfUKWork.Years))
                .ForMember(x => x.LengthOfUKWorkMonths, opt => opt.MapFrom(y => y.LengthOfUKWork.Months))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthority, PrincipalAuthorityRightToWorkViewModel>()
                .ForMember(x => x.RightToWorkInUk, opt => opt.MapFrom(y => y.PermissionToWorkStatus))
                .ForMember(x => x.VisaNumber, opt => opt.MapFrom(y => y.VisaNumber))
                .ForMember(x => x.ImmigrationStatus, opt => opt.MapFrom(y => y.ImmigrationStatus))
                .ForMember(x => x.LeaveToRemainTo, opt => opt.MapFrom(y => y.LeaveToRemainTo))
                .ForMember(x => x.LengthOfUKWork, opt => opt.ResolveUsing(LengthOfUKWorkResolver))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<UndischargedBankruptViewModel, PrincipalAuthority>()
                .ForMember(x => x.IsUndischargedBankrupt, opt => opt.MapFrom(y => y.IsUndischargedBankrupt))
                .ForMember(x => x.BankruptcyDate, opt => opt.MapFrom(y => y.BankruptcyDate))
                .ForMember(x => x.BankruptcyNumber, opt => opt.MapFrom(y => y.BankruptcyNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthority, UndischargedBankruptViewModel>()
                .ForMember(x => x.IsUndischargedBankrupt, opt => opt.MapFrom(y => y.IsUndischargedBankrupt))
                .ForMember(x => x.BankruptcyDate, opt => opt.MapFrom(y => y.BankruptcyDate))
                .ForMember(x => x.BankruptcyNumber, opt => opt.MapFrom(y => y.BankruptcyNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DisqualifiedDirectorViewModel, PrincipalAuthority>()
                .ForMember(x => x.IsDisqualifiedDirector, opt => opt.MapFrom(y => y.IsDisqualifiedDirector))
                .ForMember(x => x.DisqualificationDetails, opt => opt.MapFrom(y => y.DisqualificationDetails))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthority, DisqualifiedDirectorViewModel>()
                .ForMember(x => x.IsDisqualifiedDirector, opt => opt.MapFrom(y => y.IsDisqualifiedDirector))
                .ForMember(x => x.DisqualificationDetails, opt => opt.MapFrom(y => y.DisqualificationDetails))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<RestraintOrdersViewModel, PrincipalAuthority>()
                .ForMember(x => x.HasRestraintOrders, opt => opt.MapFrom(y => y.HasRestraintOrders))
                .ForMember(x => x.RestraintOrders, opt => opt.ResolveUsing(ProfileHelpers.RestraintOrdersResolver))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthority, RestraintOrdersViewModel>()
                .ForMember(x => x.HasRestraintOrders, opt => opt.MapFrom(y => y.HasRestraintOrders))
                .ForMember(x => x.RestraintOrders, opt => opt.MapFrom(y => y.RestraintOrders.AsEnumerable()))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<UnspentConvictionsViewModel, PrincipalAuthority>()
                .ForMember(x => x.HasUnspentConvictions, opt => opt.MapFrom(y => y.HasUnspentConvictions))
                .ForMember(x => x.UnspentConvictions, opt => opt.ResolveUsing(ProfileHelpers.UnspentConvictionsResolver))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthority, UnspentConvictionsViewModel>()
                .ForMember(x => x.HasUnspentConvictions, opt => opt.MapFrom(y => y.HasUnspentConvictions))
                .ForMember(x => x.UnspentConvictions, opt => opt.MapFrom(y => y.UnspentConvictions))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<OffencesAwaitingTrialViewModel, PrincipalAuthority>()
                .ForMember(x => x.HasOffencesAwaitingTrial, opt => opt.MapFrom(y => y.HasOffencesAwaitingTrial))
                .ForMember(x => x.OffencesAwaitingTrial, opt => opt.ResolveUsing(ProfileHelpers.OffencesAwaitingTrialResolver))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthority, OffencesAwaitingTrialViewModel>()
                .ForMember(x => x.HasOffencesAwaitingTrial, opt => opt.MapFrom(y => y.HasOffencesAwaitingTrial))
                .ForMember(x => x.OffencesAwaitingTrial, opt => opt.MapFrom(y => y.OffencesAwaitingTrial))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PreviousLicenceViewModel, PrincipalAuthority>()
                .ForMember(x => x.HasPreviouslyHeldLicence, opt => opt.MapFrom(y => y.HasPreviouslyHeldLicence))
                .ForMember(x => x.PreviousLicenceDescription, opt => opt.MapFrom(y => y.PreviousLicenceDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthority, PreviousLicenceViewModel>()
                .ForMember(x => x.HasPreviouslyHeldLicence, opt => opt.MapFrom(y => y.HasPreviouslyHeldLicence))
                .ForMember(x => x.PreviousLicenceDescription, opt => opt.MapFrom(y => y.PreviousLicenceDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<RightToWorkViewModel, PrincipalAuthority>()
                .ForAllMembers(opt => opt.Ignore());
            
            CreateMap<PrincipalAuthorityViewModel, PrincipalAuthority>()
                .ForMember(x => x.IsCurrent, opt => opt.Ignore())
                .ForMember(x => x.Licence, opt => opt.Ignore())
                .ForMember(x => x.LicenceId, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.DirectorOrPartner, opt => opt.Ignore())
                .ForMember(x => x.DirectorOrPartnerId, opt => opt.MapFrom(y => y.DirectorOrPartnerId))
                .ForMember(x => x.CountyOfBirth, opt => opt.Ignore())
                .ForMember(x => x.FullName, opt => opt.MapFrom(y => y.FullName.FullName))
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(y => y.DateOfBirth.DateOfBirth.Date))
                .ForMember(x => x.HasAlternativeName, opt => opt.MapFrom(y => y.AlternativeName.HasAlternativeName))
                .ForMember(x => x.AlternativeName, opt => opt.MapFrom(y => y.AlternativeName.AlternativeName))
                .ForMember(x => x.TownOfBirth, opt => opt.MapFrom(y => y.TownOfBirth.TownOfBirth))
                .ForMember(x => x.CountryOfBirthId, opt => opt.MapFrom(y => y.CountryOfBirth.CountryOfBirthId))
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(y => y.JobTitle.JobTitle))                
                .ForMember(x => x.Address, opt => opt.MapFrom(y => y.Address))
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.MapFrom(y => y.BusinessPhoneNumber.BusinessPhoneNumber))
                .ForMember(x => x.BusinessExtension, opt => opt.MapFrom(y => y.BusinessExtension.BusinessExtension))
                .ForMember(x => x.PersonalEmailAddress, opt => opt.MapFrom(y => y.PersonalEmailAddress.PersonalEmailAddress))
                .ForMember(x => x.PersonalMobileNumber, opt => opt.MapFrom(y => y.PersonalMobileNumber.PersonalMobileNumber))
                .ForMember(x => x.NationalInsuranceNumber, opt => opt.MapFrom(y => y.NationalInsuranceNumber.NationalInsuranceNumber))
                .ForMember(x => x.IsDirector, opt => opt.MapFrom(y => y.IsDirector.IsDirector))
                .ForMember(x => x.WillProvideConfirmation, opt => opt.MapFrom(y => y.PrincipalAuthorityConfirmation.WillProvideConfirmation))
                .ForMember(x => x.PreviousExperience, opt => opt.MapFrom(y => y.PreviousExperience.PreviousExperience))
                .ForMember(x => x.Nationality, opt => opt.MapFrom(y => y.Nationality.Nationality))
                .ForMember(x => x.HasPassport, opt => opt.MapFrom(y => y.PassportViewModel.HasPassport))
                .ForMember(x => x.PermissionToWorkStatus, opt => opt.MapFrom(y => y.PrincipalAuthorityRightToWorkViewModel.RightToWorkInUk))
                .ForMember(x => x.VisaNumber, opt => opt.MapFrom(y => y.PrincipalAuthorityRightToWorkViewModel.VisaNumber))
                .ForMember(x => x.LengthOfUKWorkYears, opt => opt.MapFrom(y => y.PrincipalAuthorityRightToWorkViewModel.LengthOfUKWork.Years))
                .ForMember(x => x.LengthOfUKWorkMonths, opt => opt.MapFrom(y => y.PrincipalAuthorityRightToWorkViewModel.LengthOfUKWork.Months))
                .ForMember(x => x.ImmigrationStatus, opt => opt.MapFrom(y => y.PrincipalAuthorityRightToWorkViewModel.ImmigrationStatus))
                .ForMember(x => x.LeaveToRemainTo, opt => opt.MapFrom(y => y.PrincipalAuthorityRightToWorkViewModel.LeaveToRemainTo.Date))
                .ForMember(x => x.IsUndischargedBankrupt, opt => opt.MapFrom(y => y.UndischargedBankruptViewModel.IsUndischargedBankrupt))
                .ForMember(x => x.BankruptcyDate, opt => opt.MapFrom(y => y.UndischargedBankruptViewModel.BankruptcyDate.Date))
                .ForMember(x => x.BankruptcyNumber, opt => opt.MapFrom(y => y.UndischargedBankruptViewModel.BankruptcyNumber))
                .ForMember(x => x.IsDisqualifiedDirector, opt => opt.MapFrom(y => y.DisqualifiedDirectorViewModel.IsDisqualifiedDirector))
                .ForMember(x => x.DisqualificationDetails, opt => opt.MapFrom(y => y.DisqualifiedDirectorViewModel.DisqualificationDetails))
                .ForMember(x => x.HasRestraintOrders, opt => opt.MapFrom(y => y.RestraintOrdersViewModel.HasRestraintOrders))
                .ForMember(x => x.RestraintOrders, opt => opt.Ignore())
                .ForMember(x => x.HasUnspentConvictions, opt => opt.MapFrom(y => y.UnspentConvictionsViewModel.HasUnspentConvictions))
                .ForMember(x => x.UnspentConvictions, opt => opt.Ignore())
                .ForMember(x => x.HasOffencesAwaitingTrial, opt => opt.MapFrom(y => y.OffencesAwaitingTrialViewModel.HasOffencesAwaitingTrial))
                .ForMember(x => x.OffencesAwaitingTrial, opt => opt.Ignore())
                .ForMember(x => x.HasPreviouslyHeldLicence, opt => opt.MapFrom(y => y.PreviousLicenceViewModel.HasPreviouslyHeldLicence))
                .ForMember(x => x.PreviousLicenceDescription, opt => opt.MapFrom(y => y.PreviousLicenceViewModel.PreviousLicenceDescription))
                .ForMember(x => x.RequiresVisa, opt => opt.Ignore())
                .ForMember(x => x.VisaDescription, opt => opt.Ignore());
        }
    }
}
