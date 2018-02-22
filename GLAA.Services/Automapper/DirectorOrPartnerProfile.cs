using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class DirectorOrPartnerProfile : Profile
    {
        public DirectorOrPartnerProfile()
        {
            CreateMap<Licence, DirectorOrPartnerCollectionViewModel>()
                .ForMember(x => x.DirectorsOrPartners, opt => opt.MapFrom(y => y.DirectorOrPartners))
                .ForMember(x => x.NumberOfDirectorsOrPartners, opt => opt.MapFrom(y => y.NumberOfDirectorsOrPartners))
                .ForMember(x => x.DirectorsRequired, opt => opt.ResolveUsing(ProfileHelpers.DirectorsRequiredResolver))
                .ForMember(x => x.IsValid, opt => opt.Ignore());

            CreateMap<ICollection<DirectorOrPartner>, DirectorOrPartnerCollectionViewModel>()
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.NumberOfDirectorsOrPartners, opt => opt.Ignore())
                .ForMember(x => x.DirectorsRequired, opt => opt.Ignore())
                .ForMember(x => x.DirectorsOrPartners, opt => opt.MapFrom(y => y));

            CreateMap<DirectorOrPartner, DirectorOrPartnerViewModel>()
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.Address, opt => opt.MapFrom(y => y.Address))
                .ForMember(x => x.BirthDetailsViewModel, opt => opt.ResolveUsing(ProfileHelpers.BirthDetailsResolver<DirectorOrPartnerViewModel>))
                .ForMember(x => x.FullName, opt => opt.ResolveUsing(ProfileHelpers.FullNameResolver))
                .ForMember(x => x.AlternativeName, opt => opt.ResolveUsing(ProfileHelpers.AlternativeFullNameResolver))
                .ForMember(x => x.JobTitle, opt => opt.ResolveUsing(ProfileHelpers.JobTitleResolver))
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.ResolveUsing(ProfileHelpers.BusinessPhoneNumberResolver))
                .ForMember(x => x.BusinessExtension, opt => opt.ResolveUsing(ProfileHelpers.BusinessExtensionResolver))
                .ForMember(x => x.PersonalMobileNumber, opt => opt.ResolveUsing(ProfileHelpers.PersonalMobileNumberResolver))
                .ForMember(x => x.PersonalEmailAddress, opt => opt.ResolveUsing(ProfileHelpers.PersonalEmailAddressResolver))
                .ForMember(x => x.Nationality, opt => opt.ResolveUsing(ProfileHelpers.NationalityResolver))
                .ForMember(x => x.IsPreviousPrincipalAuthority, opt => opt.ResolveUsing(ProfileHelpers.IsPreviousPrincipalAuthorityResolver))
                .ForMember(x => x.PrincipalAuthorityId, opt => opt.MapFrom(y => y.PrincipalAuthority.Id))
                .ForMember(x => x.PassportViewModel, opt => opt.ResolveUsing(ProfileHelpers.PassportViewModel))
                .ForMember(x => x.RightToWorkViewModel, opt => opt.ResolveUsing(ProfileHelpers.RightToWorkResolver))
                .ForMember(x => x.UndischargedBankruptViewModel, opt => opt.ResolveUsing(ProfileHelpers.UndischargedBankruptResolver))
                .ForMember(x => x.DisqualifiedDirectorViewModel, opt => opt.ResolveUsing(ProfileHelpers.DisqualifiedDirectorResolver))
                .ForMember(x => x.RestraintOrdersViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.UnspentConvictionsViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.OffencesAwaitingTrialViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.PreviousLicenceViewModel, opt => opt.ResolveUsing(ProfileHelpers.PreviousLicenceResolver))
                .ForMember(x => x.IsUk, opt => opt.MapFrom(y => y.CountryOfBirth != null && y.CountryOfBirth.IsUk))
                .ForMember(x => x.Countries, opt => opt.Ignore())
                .ForMember(x => x.Counties, opt => opt.Ignore());

            CreateMap<DirectorOrPartner, AlternativeFullNameViewModel>()
                .ConvertUsing(ProfileHelpers.AlternativeFullNameResolver);

            CreateMap<DirectorOrPartner, BirthDetailsViewModel>()
                .ConvertUsing(ProfileHelpers.BirthDetailsResolver);

            CreateMap<AlternativeFullNameViewModel, DirectorOrPartner>()                
                .ForMember(x => x.AlternativeName, opt => opt.MapFrom(y => y.AlternativeName))
                .ForMember(x => x.HasAlternativeName, opt => opt.MapFrom(y => y.HasAlternativeName))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DirectorOrPartnerViewModel, DirectorOrPartner>()
                .ForMember(x => x.Licence, opt => opt.Ignore())
                .ForMember(x => x.LicenceId, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CountyOfBirth, opt => opt.Ignore())
                .ForMember(x => x.IsPreviousPrincipalAuthority, opt => opt.MapFrom(y => y.IsPreviousPrincipalAuthority.IsPreviousPrincipalAuthority))
                .ForMember(x => x.FullName, opt => opt.MapFrom(y => y.FullName.FullName))
                .ForMember(x => x.HasAlternativeName, opt => opt.MapFrom(y => y.AlternativeName.HasAlternativeName))
                .ForMember(x => x.AlternativeName, opt => opt.MapFrom(y => y.AlternativeName.AlternativeName))
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(y => y.DateOfBirth.DateOfBirth.Date))
                .ForMember(x => x.TownOfBirth, opt => opt.MapFrom(y => y.BirthDetailsViewModel.TownOfBirthViewModel.TownOfBirth))
                .ForMember(x => x.CountryOfBirthId, opt => opt.MapFrom(y => y.BirthDetailsViewModel.CountryOfBirthViewModel.CountryOfBirthId))
                .ForMember(x => x.CountryOfBirth, opt => opt.Ignore())
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(y => y.JobTitle.JobTitle))
                .ForMember(x => x.Address, opt => opt.MapFrom(y => y.Address))
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.MapFrom(y => y.BusinessPhoneNumber.BusinessPhoneNumber))
                .ForMember(x => x.BusinessExtension, opt => opt.MapFrom(y => y.BusinessExtension.BusinessExtension))
                .ForMember(x => x.PersonalEmailAddress, opt => opt.MapFrom(y => y.PersonalEmailAddress.PersonalEmailAddress))
                .ForMember(x => x.PersonalMobileNumber, opt => opt.MapFrom(y => y.PersonalMobileNumber.PersonalMobileNumber))
                .ForMember(x => x.NationalInsuranceNumber, opt => opt.MapFrom(y => y.BirthDetailsViewModel.NationalInsuranceNumberViewModel.NationalInsuranceNumber))
                .ForMember(x => x.SocialSecurityNumber, opt => opt.MapFrom(y => y.BirthDetailsViewModel.SocialSecurityNumberViewModel.SocialSecurityNumber))
                .ForMember(x => x.PrincipalAuthority, opt => opt.Ignore())
                .ForMember(x => x.Nationality, opt => opt.MapFrom(y => y.Nationality.Nationality))
                .ForMember(x => x.HasPassport, opt => opt.MapFrom(y => y.PassportViewModel.HasPassport))
                .ForMember(x => x.RequiresVisa, opt => opt.MapFrom(y => y.RightToWorkViewModel.RequiresVisa))
                .ForMember(x => x.VisaDescription, opt => opt.MapFrom(y => y.RightToWorkViewModel.VisaDescription))
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
                .ForMember(x => x.PreviousLicenceDescription, opt => opt.MapFrom(y => y.PreviousLicenceViewModel.PreviousLicenceDescription));

            CreateMap<IsPreviousPrincipalAuthorityViewModel, DirectorOrPartner>()
                .ForMember(x => x.IsPreviousPrincipalAuthority, opt => opt.MapFrom(y => y.IsPreviousPrincipalAuthority))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DirectorOrPartner, IsPreviousPrincipalAuthorityViewModel>()
                .ConvertUsing(ProfileHelpers.IsPreviousPrincipalAuthorityResolver);

            CreateMap<IsDirectorViewModel, DirectorOrPartner>()
                .ForMember(x => x.IsPreviousPrincipalAuthority, opt => opt.MapFrom(y => y.IsDirector))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DirectorOrPartner, IsDirectorViewModel>()
                .ForMember(x => x.IsDirector, opt => opt.MapFrom(y => y.IsPreviousPrincipalAuthority))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<FullNameViewModel, DirectorOrPartner>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(y => y.FullName))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DateOfBirthViewModel, DirectorOrPartner>()
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(y => y.DateOfBirth.Date))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BirthDetailsViewModel, DirectorOrPartner>()
                .ForMember(x => x.TownOfBirth, opt => opt.MapFrom(y => y.TownOfBirthViewModel.TownOfBirth))
                .ForMember(x => x.CountryOfBirthId, opt => opt.MapFrom(y => y.CountryOfBirthViewModel.CountryOfBirthId))
                .ForMember(x => x.NationalInsuranceNumber, opt => opt.MapFrom(y => y.NationalInsuranceNumberViewModel.NationalInsuranceNumber))
                .ForMember(x => x.SocialSecurityNumber, opt => opt.MapFrom(y => y.SocialSecurityNumberViewModel.SocialSecurityNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<JobTitleViewModel, DirectorOrPartner>()
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(y => y.JobTitle))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BusinessPhoneNumberViewModel, DirectorOrPartner>()
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.MapFrom(y => y.BusinessPhoneNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PersonalMobileNumberViewModel, DirectorOrPartner>()
                .ForMember(x => x.PersonalMobileNumber, opt => opt.MapFrom(y => y.PersonalMobileNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PersonalEmailAddressViewModel, DirectorOrPartner>()
                .ForMember(x => x.PersonalEmailAddress, opt => opt.MapFrom(y => y.PersonalEmailAddress))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BusinessExtensionViewModel, DirectorOrPartner>()
                .ForMember(x => x.BusinessExtension, opt => opt.MapFrom(y => y.BusinessExtension))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NationalityViewModel, DirectorOrPartner>()
                .ForMember(x => x.Nationality, opt => opt.MapFrom(y => y.Nationality))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DirectorOrPartner, NationalityViewModel>()
                .ForMember(x => x.Nationality, opt => opt.MapFrom(y => y.Nationality))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PassportViewModel, DirectorOrPartner>()
                .ForMember(x => x.HasPassport, opt => opt.MapFrom(y => y.HasPassport))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DirectorOrPartner, PassportViewModel>()
                .ForMember(x => x.HasPassport, opt => opt.MapFrom(y => y.HasPassport))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<RightToWorkViewModel, DirectorOrPartner>()
                .ForMember(x => x.RequiresVisa, opt => opt.MapFrom(y => y.RequiresVisa))
                .ForMember(x => x.VisaDescription, opt => opt.MapFrom(y => y.VisaDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DirectorOrPartner, RightToWorkViewModel>()
                .ForMember(x => x.RequiresVisa, opt => opt.MapFrom(y => y.RequiresVisa))
                .ForMember(x => x.VisaDescription, opt => opt.MapFrom(y => y.VisaDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<UndischargedBankruptViewModel, DirectorOrPartner>()
                .ForMember(x => x.IsUndischargedBankrupt, opt => opt.MapFrom(y => y.IsUndischargedBankrupt))
                .ForMember(x => x.BankruptcyDate, opt => opt.MapFrom(y => y.BankruptcyDate))
                .ForMember(x => x.BankruptcyNumber, opt => opt.MapFrom(y => y.BankruptcyNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DirectorOrPartner, UndischargedBankruptViewModel>()
                .ForMember(x => x.IsUndischargedBankrupt, opt => opt.MapFrom(y => y.IsUndischargedBankrupt))
                .ForMember(x => x.BankruptcyDate, opt => opt.MapFrom(y => y.BankruptcyDate))
                .ForMember(x => x.BankruptcyNumber, opt => opt.MapFrom(y => y.BankruptcyNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DisqualifiedDirectorViewModel, DirectorOrPartner>()
                .ForMember(x => x.IsDisqualifiedDirector, opt => opt.MapFrom(y => y.IsDisqualifiedDirector))
                .ForMember(x => x.DisqualificationDetails, opt => opt.MapFrom(y => y.DisqualificationDetails))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DirectorOrPartner, DisqualifiedDirectorViewModel>()
                .ForMember(x => x.IsDisqualifiedDirector, opt => opt.MapFrom(y => y.IsDisqualifiedDirector))
                .ForMember(x => x.DisqualificationDetails, opt => opt.MapFrom(y => y.DisqualificationDetails))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<RestraintOrdersViewModel, DirectorOrPartner>()
                .ForMember(x => x.HasRestraintOrders, opt => opt.MapFrom(y => y.HasRestraintOrders))
                .ForMember(x => x.RestraintOrders, opt => opt.ResolveUsing(ProfileHelpers.RestraintOrdersResolver))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DirectorOrPartner, RestraintOrdersViewModel>()
                .ForMember(x => x.HasRestraintOrders, opt => opt.MapFrom(y => y.HasRestraintOrders))
                .ForMember(x => x.RestraintOrders, opt => opt.MapFrom(y => y.RestraintOrders))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<UnspentConvictionsViewModel, DirectorOrPartner>()
                .ForMember(x => x.HasUnspentConvictions, opt => opt.MapFrom(y => y.HasUnspentConvictions))
                .ForMember(x => x.UnspentConvictions, opt => opt.ResolveUsing(ProfileHelpers.UnspentConvictionsResolver))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DirectorOrPartner, UnspentConvictionsViewModel>()
                .ForMember(x => x.HasUnspentConvictions, opt => opt.MapFrom(y => y.HasUnspentConvictions))
                .ForMember(x => x.UnspentConvictions, opt => opt.MapFrom(y => y.UnspentConvictions))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<OffencesAwaitingTrialViewModel, DirectorOrPartner>()
                .ForMember(x => x.HasOffencesAwaitingTrial, opt => opt.MapFrom(y => y.HasOffencesAwaitingTrial))
                .ForMember(x => x.OffencesAwaitingTrial, opt => opt.ResolveUsing(ProfileHelpers.OffencesAwaitingTrialResolver))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DirectorOrPartner, OffencesAwaitingTrialViewModel>()
                .ForMember(x => x.HasOffencesAwaitingTrial, opt => opt.MapFrom(y => y.HasOffencesAwaitingTrial))
                .ForMember(x => x.OffencesAwaitingTrial, opt => opt.MapFrom(y => y.OffencesAwaitingTrial))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PreviousLicenceViewModel, DirectorOrPartner>()
                .ForMember(x => x.HasPreviouslyHeldLicence, opt => opt.MapFrom(y => y.HasPreviouslyHeldLicence))
                .ForMember(x => x.PreviousLicenceDescription, opt => opt.MapFrom(y => y.PreviousLicenceDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DirectorOrPartner, PreviousLicenceViewModel>()
                .ForMember(x => x.HasPreviouslyHeldLicence, opt => opt.MapFrom(y => y.HasPreviouslyHeldLicence))
                .ForMember(x => x.PreviousLicenceDescription, opt => opt.MapFrom(y => y.PreviousLicenceDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PrincipalAuthorityRightToWorkViewModel, DirectorOrPartner>()
                .ForAllMembers(opt => opt.Ignore());
        }
    }
}

