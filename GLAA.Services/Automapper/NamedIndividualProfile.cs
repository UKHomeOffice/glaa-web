using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class NamedIndividualProfile : Profile
    {
        public NamedIndividualProfile()
        {
            CreateMap<Licence, NamedIndividualCollectionViewModel>()
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.IsShellfish, opt => opt.MapFrom(y => y.IsShellfish))
                .ForMember(x => x.TurnoverBand, opt => opt.MapFrom(y => y.TurnoverBand))
                .ForMember(x => x.NamedIndividuals, opt => opt.MapFrom(y => y.NamedIndividuals))
                .ForMember(x => x.NamedJobTitles, opt => opt.MapFrom(y => y.NamedJobTitles))
                .ForMember(x => x.NamedIndividualType, opt => opt.MapFrom(y => y.NamedIndividualType))
                .ForMember(x => x.IntroText, opt => opt.Ignore())                
                .ForMember(x => x.AvailableIndividualTypes, opt => opt.Ignore())
                .ForMember(x => x.IsSubmitted, opt => opt.ResolveUsing(ProfileHelpers.GetIsSubmitted));

            CreateMap<NamedIndividual, NamedIndividualViewModel>()
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.FullName, opt => opt.ResolveUsing(ProfileHelpers.FullNameResolver))
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.ResolveUsing(ProfileHelpers.BusinessPhoneNumberResolver))
                .ForMember(x => x.BusinessExtension, opt => opt.ResolveUsing(ProfileHelpers.BusinessExtensionResolver))
                .ForMember(x => x.RightToWorkViewModel, opt => opt.ResolveUsing(ProfileHelpers.RightToWorkResolver))
                .ForMember(x => x.UndischargedBankruptViewModel, opt => opt.ResolveUsing(ProfileHelpers.UndischargedBankruptResolver))
                .ForMember(x => x.DisqualifiedDirectorViewModel, opt => opt.ResolveUsing(ProfileHelpers.DisqualifiedDirectorResolver))
                .ForMember(x => x.RestraintOrdersViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.UnspentConvictionsViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.OffencesAwaitingTrialViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.PreviousLicenceViewModel, opt => opt.ResolveUsing(ProfileHelpers.PreviousLicenceResolver))
                .ForMember(x => x.IsSubmitted, opt => opt.ResolveUsing(x => ProfileHelpers.GetIsSubmitted(x.Licence)));

            CreateMap<NamedIndividualViewModel, NamedIndividual>()
                .ForMember(x => x.Licence, opt => opt.Ignore())
                .ForMember(x => x.LicenceId, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())                
                .ForMember(x => x.FullName, opt => opt.MapFrom(y => y.FullName.FullName))
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(y => y.DateOfBirth.DateOfBirth))
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.MapFrom(y => y.BusinessPhoneNumber.BusinessPhoneNumber))
                .ForMember(x => x.BusinessExtension, opt => opt.MapFrom(y => y.BusinessExtension.BusinessExtension))
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
                .ForMember(x => x.PreviousLicenceDescription, opt => opt.MapFrom(y => y.PreviousLicenceViewModel.PreviousLicenceDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NamedJobTitleViewModel, NamedJobTitle>()
                .ForMember(x => x.Licence, opt => opt.Ignore())
                .ForMember(x => x.LicenceId, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(y => y.JobTitle))
                .ForMember(x => x.JobTitleNumber, opt => opt.MapFrom(y => y.JobTitleNumber));

            CreateMap<NamedJobTitle, NamedJobTitleViewModel>()
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.IsSubmitted, opt => opt.ResolveUsing(x => ProfileHelpers.GetIsSubmitted(x.Licence)));

            CreateMap<ICollection<NamedIndividual>, NamedIndividualCollectionViewModel>()
                .ForMember(x => x.IsShellfish, opt => opt.Ignore())
                .ForMember(x => x.TurnoverBand, opt => opt.Ignore())
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.NamedIndividuals, opt => opt.MapFrom(y => y))
                .ForMember(x => x.NamedJobTitles, opt => opt.Ignore())
                .ForMember(x => x.IntroText, opt => opt.Ignore())
                .ForMember(x => x.NamedIndividualType, opt => opt.Ignore())
                .ForMember(x => x.NamedIndividualType, opt => opt.MapFrom(y => y.FirstOrDefault().Licence.NamedIndividualType))
                .ForMember(x => x.AvailableIndividualTypes, opt => opt.Ignore())
                .ForMember(x => x.IsSubmitted, opt => opt.Ignore());

            CreateMap<ICollection<NamedJobTitle>, NamedIndividualCollectionViewModel>()
                .ForMember(x => x.IsShellfish, opt => opt.Ignore())
                .ForMember(x => x.TurnoverBand, opt => opt.Ignore())
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.NamedIndividuals, opt => opt.Ignore())
                .ForMember(x => x.NamedJobTitles, opt => opt.MapFrom(y => y))
                .ForMember(x => x.IntroText, opt => opt.Ignore())
                .ForMember(x => x.NamedIndividualType, opt => opt.Ignore())
                .ForMember(x => x.AvailableIndividualTypes, opt => opt.Ignore())
                .ForMember(x => x.IsSubmitted, opt => opt.Ignore());

            CreateMap<FullNameViewModel, NamedIndividual>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(y => y.FullName))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DateOfBirthViewModel, NamedIndividual>()
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(y => y.DateOfBirth.Date))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BusinessPhoneNumberViewModel, NamedIndividual>()
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.MapFrom(y => y.BusinessPhoneNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BusinessExtensionViewModel, NamedIndividual>()
                .ForMember(x => x.BusinessExtension, opt => opt.MapFrom(y => y.BusinessExtension))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<RightToWorkViewModel, NamedIndividual>()
                .ForMember(x => x.RequiresVisa, opt => opt.MapFrom(y => y.RequiresVisa))
                .ForMember(x => x.VisaDescription, opt => opt.MapFrom(y => y.VisaDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NamedIndividual, RightToWorkViewModel>()
                .ForMember(x => x.RequiresVisa, opt => opt.MapFrom(y => y.RequiresVisa))
                .ForMember(x => x.VisaDescription, opt => opt.MapFrom(y => y.VisaDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<UndischargedBankruptViewModel, NamedIndividual>()
                .ForMember(x => x.IsUndischargedBankrupt, opt => opt.MapFrom(y => y.IsUndischargedBankrupt))
                .ForMember(x => x.BankruptcyDate, opt => opt.MapFrom(y => y.BankruptcyDate))
                .ForMember(x => x.BankruptcyNumber, opt => opt.MapFrom(y => y.BankruptcyNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NamedIndividual, UndischargedBankruptViewModel>()
                .ForMember(x => x.IsUndischargedBankrupt, opt => opt.MapFrom(y => y.IsUndischargedBankrupt))
                .ForMember(x => x.BankruptcyDate, opt => opt.MapFrom(y => y.BankruptcyDate))
                .ForMember(x => x.BankruptcyNumber, opt => opt.MapFrom(y => y.BankruptcyNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DisqualifiedDirectorViewModel, NamedIndividual>()
                .ForMember(x => x.IsDisqualifiedDirector, opt => opt.MapFrom(y => y.IsDisqualifiedDirector))
                .ForMember(x => x.DisqualificationDetails, opt => opt.MapFrom(y => y.DisqualificationDetails))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NamedIndividual, DisqualifiedDirectorViewModel>()
                .ForMember(x => x.IsDisqualifiedDirector, opt => opt.MapFrom(y => y.IsDisqualifiedDirector))
                .ForMember(x => x.DisqualificationDetails, opt => opt.MapFrom(y => y.DisqualificationDetails))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<RestraintOrdersViewModel, NamedIndividual>()
                .ForMember(x => x.HasRestraintOrders, opt => opt.MapFrom(y => y.HasRestraintOrders))
                .ForMember(x => x.RestraintOrders, opt => opt.ResolveUsing(ProfileHelpers.RestraintOrdersResolver))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NamedIndividual, RestraintOrdersViewModel>()
                .ForMember(x => x.HasRestraintOrders, opt => opt.MapFrom(y => y.HasRestraintOrders))
                .ForMember(x => x.RestraintOrders, opt => opt.MapFrom(y => y.RestraintOrders))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<UnspentConvictionsViewModel, NamedIndividual>()
                .ForMember(x => x.HasUnspentConvictions, opt => opt.MapFrom(y => y.HasUnspentConvictions))
                .ForMember(x => x.UnspentConvictions, opt => opt.ResolveUsing(ProfileHelpers.UnspentConvictionsResolver))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NamedIndividual, UnspentConvictionsViewModel>()
                .ForMember(x => x.HasUnspentConvictions, opt => opt.MapFrom(y => y.HasUnspentConvictions))
                .ForMember(x => x.UnspentConvictions, opt => opt.MapFrom(y => y.UnspentConvictions))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<OffencesAwaitingTrialViewModel, NamedIndividual>()
                .ForMember(x => x.HasOffencesAwaitingTrial, opt => opt.MapFrom(y => y.HasOffencesAwaitingTrial))
                .ForMember(x => x.OffencesAwaitingTrial, opt => opt.ResolveUsing(ProfileHelpers.OffencesAwaitingTrialResolver))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NamedIndividual, OffencesAwaitingTrialViewModel>()
                .ForMember(x => x.HasOffencesAwaitingTrial, opt => opt.MapFrom(y => y.HasOffencesAwaitingTrial))
                .ForMember(x => x.OffencesAwaitingTrial, opt => opt.MapFrom(y => y.OffencesAwaitingTrial))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PreviousLicenceViewModel, NamedIndividual>()
                .ForMember(x => x.HasPreviouslyHeldLicence, opt => opt.MapFrom(y => y.HasPreviouslyHeldLicence))
                .ForMember(x => x.PreviousLicenceDescription, opt => opt.MapFrom(y => y.PreviousLicenceDescription))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<NamedIndividual, PreviousLicenceViewModel>()
                .ForMember(x => x.HasPreviouslyHeldLicence, opt => opt.MapFrom(y => y.HasPreviouslyHeldLicence))
                .ForMember(x => x.PreviousLicenceDescription, opt => opt.MapFrom(y => y.PreviousLicenceDescription))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
