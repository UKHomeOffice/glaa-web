using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class AlternativeBusinessRepresentativeProfile : Profile
    {
        public AlternativeBusinessRepresentativeProfile()
        {
            CreateMap<Licence, AlternativeBusinessRepresentativeCollectionViewModel>()
                .ForMember(x => x.AlternativeBusinessRepresentatives, opt => opt.MapFrom(y => y.AlternativeBusinessRepresentatives))
                .ForMember(x => x.HasAlternativeBusinessRepresentatives, opt => opt.MapFrom(y => y.HasAlternativeBusinessRepresentatives))
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.YesNo, opt => opt.Ignore());

            CreateMap<ICollection<AlternativeBusinessRepresentative>, AlternativeBusinessRepresentativeCollectionViewModel>()
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.HasAlternativeBusinessRepresentatives, opt => opt.MapFrom(y => y.FirstOrDefault().Licence.HasAlternativeBusinessRepresentatives))
                .ForMember(x => x.AlternativeBusinessRepresentatives, opt => opt.MapFrom(y => y))
                .ForMember(x => x.YesNo, opt => opt.Ignore());
            
            CreateMap<AlternativeBusinessRepresentative, AlternativeBusinessRepresentativeViewModel>()
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.NationalInsuranceNumber, opt => opt.ResolveUsing(ProfileHelpers.NationalInsuranceNumberResolver))
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
                .ForMember(x => x.PassportViewModel, opt => opt.ResolveUsing(ProfileHelpers.PassportViewModel))
                .ForMember(x => x.RightToWorkViewModel, opt => opt.ResolveUsing(ProfileHelpers.RightToWorkResolver))
                .ForMember(x => x.UndischargedBankruptViewModel, opt => opt.ResolveUsing(ProfileHelpers.UndischargedBankruptResolver))
                .ForMember(x => x.DisqualifiedDirectorViewModel, opt => opt.ResolveUsing(ProfileHelpers.DisqualifiedDirectorResolver))
                .ForMember(x => x.RestraintOrdersViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.UnspentConvictionsViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.OffencesAwaitingTrialViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.PreviousLicenceViewModel, opt => opt.ResolveUsing(ProfileHelpers.PreviousLicenceResolver))
                .ForMember(x => x.Countries, opt => opt.Ignore())
                .ForMember(x => x.Counties, opt => opt.Ignore());

            CreateMap<AlternativeBusinessRepresentative, AlternativeFullNameViewModel>()
                .ConvertUsing(ProfileHelpers.AlternativeFullNameResolver);

            CreateMap<AlternativeBusinessRepresentative, NationalInsuranceNumberViewModel>()
                .ConvertUsing(ProfileHelpers.NationalInsuranceNumberResolver);

            CreateMap<AlternativeBusinessRepresentativeViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.Licence, opt => opt.Ignore())
                .ForMember(x => x.LicenceId, opt => opt.Ignore())
                .ForMember(x => x.CountyOfBirth, opt => opt.Ignore())
                .ForMember(x => x.FullName, opt => opt.MapFrom(y => y.FullName.FullName))
                .ForMember(x => x.HasAlternativeName, opt => opt.MapFrom(y => y.AlternativeName.HasAlternativeName))
                .ForMember(x => x.AlternativeName, opt => opt.MapFrom(y => y.AlternativeName.AlternativeName))
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(y => y.DateOfBirth.DateOfBirth.Date))
                .ForMember(x => x.TownOfBirth, opt => opt.MapFrom(y => y.TownOfBirth.TownOfBirth))
                .ForMember(x => x.CountryOfBirthId, opt => opt.MapFrom(y => y.CountryOfBirth.CountryOfBirthId))
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(y => y.JobTitle.JobTitle))
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.MapFrom(y => y.BusinessPhoneNumber.BusinessPhoneNumber))
                .ForMember(x => x.BusinessExtension, opt => opt.MapFrom(y => y.BusinessExtension.BusinessExtension))
                .ForMember(x => x.PersonalEmailAddress, opt => opt.MapFrom(y => y.PersonalEmailAddress.PersonalEmailAddress))
                .ForMember(x => x.PersonalMobileNumber, opt => opt.MapFrom(y => y.PersonalMobileNumber.PersonalMobileNumber))                
                .ForMember(x => x.NationalInsuranceNumber, opt => opt.MapFrom(y => y.NationalInsuranceNumber.NationalInsuranceNumber))
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

            CreateMap<FullNameViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(y => y.FullName))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<AlternativeFullNameViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.AlternativeName, opt => opt.MapFrom(y => y.AlternativeName))
                .ForMember(x => x.HasAlternativeName, opt => opt.MapFrom(y => y.HasAlternativeName))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DateOfBirthViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(y => y.DateOfBirth.Date))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<TownOfBirthViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.TownOfBirth, opt => opt.MapFrom(y => y.TownOfBirth))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<CountryOfBirthViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.CountryOfBirthId, opt => opt.MapFrom(y => y.CountryOfBirthId))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<JobTitleViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(y => y.JobTitle))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BusinessPhoneNumberViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.MapFrom(y => y.BusinessPhoneNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PersonalMobileNumberViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.PersonalMobileNumber, opt => opt.MapFrom(y => y.PersonalMobileNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PersonalEmailAddressViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.PersonalEmailAddress, opt => opt.MapFrom(y => y.PersonalEmailAddress))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BusinessExtensionViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.BusinessExtension, opt => opt.MapFrom(y => y.BusinessExtension))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NationalInsuranceNumberViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.NationalInsuranceNumber, opt => opt.MapFrom(y => y.NationalInsuranceNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NationalityViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.Nationality, opt => opt.MapFrom(y => y.Nationality))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<AlternativeBusinessRepresentative, NationalityViewModel>()
                .ForMember(x => x.Nationality, opt => opt.MapFrom(y => y.Nationality))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PassportViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.HasPassport, opt => opt.MapFrom(y => y.HasPassport))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<AlternativeBusinessRepresentative, PassportViewModel>()
                .ForMember(x => x.HasPassport, opt => opt.MapFrom(y => y.HasPassport))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<RightToWorkViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.RequiresVisa, opt => opt.MapFrom(y => y.RequiresVisa))
                .ForMember(x => x.VisaDescription, opt => opt.MapFrom(y => y.VisaDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<AlternativeBusinessRepresentative, RightToWorkViewModel>()
                .ForMember(x => x.RequiresVisa, opt => opt.MapFrom(y => y.RequiresVisa))
                .ForMember(x => x.VisaDescription, opt => opt.MapFrom(y => y.VisaDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<UndischargedBankruptViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.IsUndischargedBankrupt, opt => opt.MapFrom(y => y.IsUndischargedBankrupt))
                .ForMember(x => x.BankruptcyDate, opt => opt.MapFrom(y => y.BankruptcyDate))
                .ForMember(x => x.BankruptcyNumber, opt => opt.MapFrom(y => y.BankruptcyNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<AlternativeBusinessRepresentative, UndischargedBankruptViewModel>()
                .ForMember(x => x.IsUndischargedBankrupt, opt => opt.MapFrom(y => y.IsUndischargedBankrupt))
                .ForMember(x => x.BankruptcyDate, opt => opt.MapFrom(y => y.BankruptcyDate))
                .ForMember(x => x.BankruptcyNumber, opt => opt.MapFrom(y => y.BankruptcyNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DisqualifiedDirectorViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.IsDisqualifiedDirector, opt => opt.MapFrom(y => y.IsDisqualifiedDirector))
                .ForMember(x => x.DisqualificationDetails, opt => opt.MapFrom(y => y.DisqualificationDetails))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<AlternativeBusinessRepresentative, DisqualifiedDirectorViewModel>()
                .ForMember(x => x.IsDisqualifiedDirector, opt => opt.MapFrom(y => y.IsDisqualifiedDirector))
                .ForMember(x => x.DisqualificationDetails, opt => opt.MapFrom(y => y.DisqualificationDetails))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<RestraintOrdersViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.HasRestraintOrders, opt => opt.MapFrom(y => y.HasRestraintOrders))
                .ForMember(x => x.RestraintOrders, opt => opt.ResolveUsing(ProfileHelpers.RestraintOrdersResolver))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<AlternativeBusinessRepresentative, RestraintOrdersViewModel>()
                .ForMember(x => x.HasRestraintOrders, opt => opt.MapFrom(y => y.HasRestraintOrders))
                .ForMember(x => x.RestraintOrders, opt => opt.MapFrom(y => y.RestraintOrders))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<UnspentConvictionsViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.HasUnspentConvictions, opt => opt.MapFrom(y => y.HasUnspentConvictions))
                .ForMember(x => x.UnspentConvictions, opt => opt.ResolveUsing(ProfileHelpers.UnspentConvictionsResolver))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<AlternativeBusinessRepresentative, UnspentConvictionsViewModel>()
                .ForMember(x => x.HasUnspentConvictions, opt => opt.MapFrom(y => y.HasUnspentConvictions))
                .ForMember(x => x.UnspentConvictions, opt => opt.MapFrom(y => y.UnspentConvictions))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<OffencesAwaitingTrialViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.HasOffencesAwaitingTrial, opt => opt.MapFrom(y => y.HasOffencesAwaitingTrial))
                .ForMember(x => x.OffencesAwaitingTrial, opt => opt.ResolveUsing(ProfileHelpers.OffencesAwaitingTrialResolver))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<AlternativeBusinessRepresentative, OffencesAwaitingTrialViewModel>()
                .ForMember(x => x.HasOffencesAwaitingTrial, opt => opt.MapFrom(y => y.HasOffencesAwaitingTrial))
                .ForMember(x => x.OffencesAwaitingTrial, opt => opt.MapFrom(y => y.OffencesAwaitingTrial))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PreviousLicenceViewModel, AlternativeBusinessRepresentative>()
                .ForMember(x => x.HasPreviouslyHeldLicence, opt => opt.MapFrom(y => y.HasPreviouslyHeldLicence))
                .ForMember(x => x.PreviousLicenceDescription, opt => opt.MapFrom(y => y.PreviousLicenceDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<AlternativeBusinessRepresentative, PreviousLicenceViewModel>()
                .ForMember(x => x.HasPreviouslyHeldLicence, opt => opt.MapFrom(y => y.HasPreviouslyHeldLicence))
                .ForMember(x => x.PreviousLicenceDescription, opt => opt.MapFrom(y => y.PreviousLicenceDescription))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
