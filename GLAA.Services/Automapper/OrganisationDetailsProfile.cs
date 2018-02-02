using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class OrganisationDetailsProfile : Profile
    {
        public OrganisationDetailsProfile()
        {
            CreateMap<Licence, OrganisationDetailsViewModel>()
                .ForMember(x => x.BusinessEmailAddress, opt => opt.ResolveUsing(EmailResolver))
                .ForMember(x => x.OperatingIndustries, opt => opt.ResolveUsing(ProfileHelpers.OperatingIndustriesResolver))
                .ForMember(x => x.OperatingCountries, opt => opt.ResolveUsing(OperatingCountriesResolver))
                .ForMember(x => x.LegalStatus, opt => opt.ResolveUsing(LegalStatusResolver))
                .ForMember(x => x.CommunicationPreference, opt => opt.MapFrom(y => y))
                .ForMember(x => x.Turnover, opt => opt.MapFrom(y => y))
                .ForMember(x => x.PAYEERNStatus, opt => opt.MapFrom(y => y))
                .ForMember(x => x.VATStatus, opt => opt.MapFrom(y => y))
                .ForMember(x => x.TaxReference, opt => opt.MapFrom(y => y))
                .ForMember(x => x.BusinessName, opt => opt.ResolveUsing(BusinessNameResolver))
                .ForMember(x => x.IsValid, opt => opt.Ignore());

            CreateMap<Licence, BusinessEmailAddressViewModel>()
                .ForMember(x => x.BusinessEmailAddress, opt => opt.MapFrom(y => y.BusinessEmailAddress))
                .ForMember(x => x.BusinessEmailAddressConfirmation,
                    opt => opt.MapFrom(y => y.BusinessEmailAddressConfirmation));

            CreateMap<Licence, PAYEERNStatusViewModel>()
                .ForMember(x => x.HasPAYEERNNumber, opt => opt.MapFrom(y => y.HasPAYEERNNumber))
                .ForMember(x => x.PAYEERNNumber, opt => opt.MapFrom(y => y.PAYEERNNumber))
                .ForMember(x => x.PAYEERNRegistrationDate, opt => opt.MapFrom(y => y.PAYEERNRegistrationDate))
                .ForMember(x => x.YesNo, opt => opt.UseValue(ProfileHelpers.YesNoList));

            CreateMap<Licence, VATStatusViewModel>()
                .ForMember(x => x.HasVATNumber, opt => opt.MapFrom(y => y.HasVATNumber))
                .ForMember(x => x.VATNumber, opt => opt.MapFrom(y => y.VATNumber))
                .ForMember(x => x.VATRegistrationDate, opt => opt.MapFrom(y => y.VATRegistrationDate))
                .ForMember(x => x.YesNo, opt => opt.UseValue(ProfileHelpers.YesNoList));

            CreateMap<Licence, CommunicationPreferenceViewModel>()
                .ForMember(x => x.CommunicationPreference, opt => opt.MapFrom(y => y.CommunicationPreference))
                .ForMember(x => x.AvailableCommunicationPreferences, opt => opt.Ignore());

            CreateMap<Licence, TaxReferenceViewModel>()
                .ForMember(x => x.TaxReferenceNumber, opt => opt.MapFrom(y => y.TaxReferenceNumber));

            CreateMap<Licence, TurnoverViewModel>()
                .ForMember(x => x.TurnoverBand, opt => opt.MapFrom(y => y.TurnoverBand))
                .ForMember(x => x.AvailableTurnoverBands, opt => opt.Ignore());

            CreateMap<Licence, LegalStatusViewModel>()
                .ForMember(x => x.LegalStatus, opt => opt.MapFrom(y => y.LegalStatus))
                .ForMember(x => x.CompanyRegistrationDate, opt => opt.MapFrom(y => y.CompanyRegistrationDate))
                .ForMember(x => x.CompaniesHouseNumber, opt => opt.MapFrom(y => y.CompaniesHouseNumber))
                .ForMember(x => x.AvailableLegalStatuses, opt => opt.Ignore());

            CreateMap<Industry, IndustryViewModel>()
                .ForMember(x => x.Checked, opt => opt.Ignore());

            CreateMap<Country, CountryViewModel>()
                .ForMember(x => x.Checked, opt => opt.Ignore());

            CreateMap<OrganisationDetailsViewModel, Licence>()
                .ForMember(x => x.BusinessName, opt => opt.MapFrom(y => y.BusinessName.BusinessName))
                .ForMember(x => x.HasTradingName, opt => opt.MapFrom(y => y.BusinessName.HasTradingName))
                .ForMember(x => x.TradingName, opt => opt.MapFrom(y => y.BusinessName.TradingName))
                .ForMember(x => x.HasPreviousTradingName, opt => opt.MapFrom(y => y.BusinessName.HasPreviousTradingName))
                .ForMember(x => x.PreviousTradingNames, opt => opt.Ignore())
                .ForMember(x => x.OperatingIndustries, opt => opt.Ignore())
                .ForMember(x => x.IsShellfish, opt => opt.ResolveUsing(ShellfishResolver))
                .ForMember(x => x.OperatingCountries, opt => opt.Ignore())
                .ForMember(x => x.TurnoverBand, opt => opt.MapFrom(y => y.Turnover.TurnoverBand))
                .ForMember(x => x.CommunicationPreference, opt => opt.MapFrom(y => y.CommunicationPreference.CommunicationPreference))
                .ForMember(x => x.Address, opt => opt.MapFrom(y => y.Address))
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.MapFrom(y => y.BusinessPhoneNumber.BusinessPhoneNumber))
                .ForMember(x => x.BusinessMobileNumber, opt => opt.MapFrom(y => y.BusinessMobileNumber.BusinessMobileNumber))
                .ForMember(x => x.BusinessEmailAddress, opt => opt.MapFrom(y => y.BusinessEmailAddress.BusinessEmailAddress))
                .ForMember(x => x.BusinessEmailAddressConfirmation, opt => opt.MapFrom(y => y.BusinessEmailAddress.BusinessEmailAddressConfirmation))
                .ForMember(x => x.BusinessWebsite, opt => opt.MapFrom(y => y.BusinessWebsite.BusinessWebsite))
                .ForMember(x => x.LegalStatus, opt => opt.MapFrom(y => y.LegalStatus.LegalStatus))
                .ForMember(x => x.CompaniesHouseNumber, opt => opt.MapFrom(y => y.LegalStatus.CompaniesHouseNumber))
                .ForMember(x => x.CompanyRegistrationDate, opt => opt.MapFrom(y => y.LegalStatus.CompanyRegistrationDate))
                .ForMember(x => x.HasPAYEERNNumber, opt => opt.MapFrom(y => y.PAYEERNStatus.HasPAYEERNNumber))
                .ForMember(x => x.PAYEERNNumber, opt => opt.MapFrom(y => y.PAYEERNStatus.PAYEERNNumber))
                .ForMember(x => x.PAYEERNRegistrationDate, opt => opt.MapFrom(y => y.PAYEERNStatus.PAYEERNRegistrationDate))
                .ForMember(x => x.HasVATNumber, opt => opt.MapFrom(y => y.VATStatus.HasVATNumber))
                .ForMember(x => x.VATNumber, opt => opt.MapFrom(y => y.VATStatus.VATNumber))
                .ForMember(x => x.VATRegistrationDate, opt => opt.MapFrom(y => y.VATStatus.VATRegistrationDate))
                .ForMember(x => x.TaxReferenceNumber, opt => opt.MapFrom(y => y.TaxReference.TaxReferenceNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BusinessNameViewModel, Licence>()
                .ForMember(x => x.BusinessName, opt => opt.MapFrom(y => y.BusinessName))
                .ForMember(x => x.HasTradingName, opt => opt.MapFrom(y => y.HasTradingName))
                .ForMember(x => x.TradingName, opt => opt.MapFrom(y => y.TradingName))
                .ForMember(x => x.HasPreviousTradingName, opt => opt.MapFrom(y => y.HasPreviousTradingName))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<LegalStatusViewModel, Licence>()
                .ForMember(x => x.LegalStatus, opt => opt.MapFrom(y => y.LegalStatus))
                .ForMember(x => x.CompaniesHouseNumber, opt => opt.MapFrom(y => y.CompaniesHouseNumber))
                .ForMember(x => x.CompanyRegistrationDate, opt => opt.MapFrom(y => y.CompanyRegistrationDate))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<TurnoverViewModel, Licence>()
                .ForMember(x => x.TurnoverBand, opt => opt.MapFrom(y => y.TurnoverBand))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<CommunicationPreferenceViewModel, Licence>()
                .ForMember(x => x.CommunicationPreference, opt => opt.MapFrom(y => y.CommunicationPreference))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<BusinessPhoneNumberViewModel, Licence>()
                .ForMember(x => x.BusinessPhoneNumber, opt => opt.MapFrom(y => y.BusinessPhoneNumber))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<BusinessMobileNumberViewModel, Licence>()
                .ForMember(x => x.BusinessMobileNumber, opt => opt.MapFrom(y => y.BusinessMobileNumber))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<BusinessEmailAddressViewModel, Licence>()
                .ForMember(x => x.BusinessEmailAddress, opt => opt.MapFrom(y => y.BusinessEmailAddress))
                .ForMember(x => x.BusinessEmailAddressConfirmation, opt => opt.MapFrom(y => y.BusinessEmailAddressConfirmation))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<BusinessWebsiteViewModel, Licence>()
                .ForMember(x => x.BusinessWebsite, opt => opt.MapFrom(y => y.BusinessWebsite))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<PAYEERNStatusViewModel, Licence>()
                .ForMember(x => x.HasPAYEERNNumber, opt => opt.MapFrom(y => y.HasPAYEERNNumber))
                .ForMember(x => x.PAYEERNNumber, opt => opt.MapFrom(y => y.PAYEERNNumber))
                .ForMember(x => x.PAYEERNRegistrationDate, opt => opt.MapFrom(y => y.PAYEERNRegistrationDate))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<VATStatusViewModel, Licence>()
                .ForMember(x => x.HasVATNumber, opt => opt.MapFrom(y => y.HasVATNumber))
                .ForMember(x => x.VATNumber, opt => opt.MapFrom(y => y.VATNumber))
                .ForMember(x => x.VATRegistrationDate, opt => opt.MapFrom(y => y.VATRegistrationDate))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<TaxReferenceViewModel, Licence>()
                .ForMember(x => x.TaxReferenceNumber, opt => opt.MapFrom(y => y.TaxReferenceNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

        }

        private bool ShellfishResolver(OrganisationDetailsViewModel model)
        {
            return model.OperatingIndustries.OperatingIndustries.Any(x => x.Name == "Shellfish" && x.Checked);
        }

        private BusinessEmailAddressViewModel EmailResolver(Licence licence)
        {
            return new BusinessEmailAddressViewModel
            {
                BusinessEmailAddress = licence.BusinessEmailAddress,
                BusinessEmailAddressConfirmation = licence.BusinessEmailAddressConfirmation,
            };
        }

        private OperatingCountriesViewModel OperatingCountriesResolver(Licence licence)
        {
            var vm = new OperatingCountriesViewModel();

            if (licence.OperatingCountries != null)
            {
                foreach (var country in licence.OperatingCountries)
                {
                    var match = vm.OperatingCountries.Single(x => x.Id == country.CountryId);
                    match.Checked = true;
                }
            }

            return vm;
        }
        
        private LegalStatusViewModel LegalStatusResolver(Licence licence)
        {
            return new LegalStatusViewModel
            {
                LegalStatus = licence.LegalStatus,
                CompaniesHouseNumber = licence.CompaniesHouseNumber,
                CompanyRegistrationDate = new DateViewModel
                {
                    Date = licence.CompanyRegistrationDate
                }                
            };
        }

        private BusinessNameViewModel BusinessNameResolver(Licence licence)
        {
            return new BusinessNameViewModel
            {
                BusinessName = licence.BusinessName,
                HasTradingName = licence.HasTradingName,
                TradingName = licence.TradingName,
                HasPreviousTradingName = licence.HasPreviousTradingName,
                PreviousTradingNames = licence.PreviousTradingNames?.Select(p => new PreviousTradingNameViewModel
                {
                    Id = p.Id,
                    BusinessName = p.BusinessName,
                    Country = p.Country,
                    Town = p.Town
                }).ToList()
            };
        }
    }
}
