using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
                .ForMember(x => x.BusinessCredentialsViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.CommunicationPreference, opt => opt.MapFrom(y => y))
                .ForMember(x => x.Turnover, opt => opt.MapFrom(y => y))
                //.ForMember(x => x.PAYEStatus, opt => opt.MapFrom(y => y))
                //.ForMember(x => x.VATStatus, opt => opt.MapFrom(y => y))
                //.ForMember(x => x.TaxReference, opt => opt.MapFrom(y => y))
                .ForMember(x => x.BusinessName, opt => opt.ResolveUsing(BusinessNameResolver))
                .ForMember(x => x.IsValid, opt => opt.Ignore());

            CreateMap<Licence, BusinessEmailAddressViewModel>()
                .ForMember(x => x.BusinessEmailAddress, opt => opt.MapFrom(y => y.BusinessEmailAddress))
                .ForMember(x => x.BusinessEmailAddressConfirmation,
                    opt => opt.MapFrom(y => y.BusinessEmailAddressConfirmation));

            CreateMap<Licence, BusinessCredentialsViewModel>()
                .ForMember(x => x.CompaniesHouseRegistrationViewModel, opt => opt.ResolveUsing(CompaniesHouseRegistrationResolver))
                .ForMember(x => x.LegalStatus, opt => opt.MapFrom(y => (LegalStatusEnum)y.LegalStatus))
                .ForMember(x => x.PAYEStatusViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.VATStatusViewModel, opt => opt.MapFrom(y => y))
                .ForMember(x => x.TaxReferenceViewModel, opt => opt.MapFrom(y => y))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<Licence, PAYEStatusViewModel>()
                .ForMember(x => x.HasPAYENumber, opt => opt.MapFrom(y => y.HasPAYENumber))
                .ForMember(x => x.PAYENumbers, opt => opt.ResolveUsing(PAYENumberResolver))
                .ForMember(x => x.YesNo, opt => opt.UseValue(ProfileHelpers.YesNoList))
                .ForMember(x => x.IsValid, opt => opt.Ignore());

            CreateMap<Licence, VATStatusViewModel>()
                .ForMember(x => x.HasVATNumber, opt => opt.MapFrom(y => y.HasVATNumber))
                .ForMember(x => x.VATNumber, opt => opt.MapFrom(y => y.VATNumber))
                .ForMember(x => x.VATRegistrationDate, opt => opt.MapFrom(y => y.VATRegistrationDate))
                .ForMember(x => x.YesNo, opt => opt.UseValue(ProfileHelpers.YesNoList))
                .ForMember(x => x.IsValid, opt => opt.Ignore());

            CreateMap<Licence, CommunicationPreferenceViewModel>()
                .ForMember(x => x.CommunicationPreference, opt => opt.MapFrom(y => y.CommunicationPreference))
                .ForMember(x => x.AvailableCommunicationPreferences, opt => opt.Ignore());

            CreateMap<Licence, TaxReferenceViewModel>()
                .ForMember(x => x.TaxReferenceNumber, opt => opt.MapFrom(y => y.TaxReferenceNumber))
                //.ForMember(x => x.HasTaxReferenceNumber, opt => opt.MapFrom(y => y.HasTaxReferenceNumber))
                //.ForMember(x => x.LegalStatus, opt => opt.MapFrom(y => y.LegalStatus))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Licence, TurnoverViewModel>()
                .ForMember(x => x.TurnoverBand, opt => opt.MapFrom(y => y.TurnoverBand))
                .ForMember(x => x.AvailableTurnoverBands, opt => opt.Ignore());

            CreateMap<Licence, LegalStatusViewModel>()
                .ForMember(x => x.LegalStatus, opt => opt.MapFrom(y => y.LegalStatus))
                .ForMember(x => x.Other, opt => opt.MapFrom(y => y.OtherLegalStatus))
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
                //.ForMember(x => x.HasPAYENumber, opt => opt.MapFrom(y => y.PAYEStatus.HasPAYENumber))
                .ForMember(x => x.PAYENumbers, opt => opt.Ignore())
                //.ForMember(x => x.HasVATNumber, opt => opt.MapFrom(y => y.VATStatus.HasVATNumber))
                //.ForMember(x => x.VATNumber, opt => opt.MapFrom(y => y.VATStatus.VATNumber))
                //.ForMember(x => x.VATRegistrationDate, opt => opt.MapFrom(y => y.VATStatus.VATRegistrationDate))
                //.ForMember(x => x.TaxReferenceNumber, opt => opt.MapFrom(y => y.TaxReference.TaxReferenceNumber))
                //.ForMember(x => x.HasTaxReferenceNumber, opt => opt.MapFrom(y => y.TaxReference.HasTaxReferenceNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BusinessCredentialsViewModel, Licence>()
                .ForMember(x => x.CompaniesHouseNumber, opt => opt.MapFrom(y => y.CompaniesHouseRegistrationViewModel.CompaniesHouseNumber))
                .ForMember(x => x.CompanyRegistrationDate, opt => opt.MapFrom(y => y.CompaniesHouseRegistrationViewModel.CompanyRegistrationDate))
                .ForMember(x => x.HasVATNumber, opt => opt.MapFrom(y => y.VATStatusViewModel.HasVATNumber))
                .ForMember(x => x.VATNumber, opt => opt.MapFrom(y => y.VATStatusViewModel.VATNumber))
                .ForMember(x => x.VATRegistrationDate, opt => opt.MapFrom(y => y.VATStatusViewModel.VATRegistrationDate))
                .ForMember(x => x.HasPAYENumber, opt => opt.MapFrom(y => y.PAYEStatusViewModel.HasPAYENumber))
                //.ForMember(x => x.HasTaxReferenceNumber, opt => opt.MapFrom(y => y.TaxReferenceViewModel.HasTaxReferenceNumber))
                .ForMember(x => x.TaxReferenceNumber, opt => opt.MapFrom(y => y.TaxReferenceViewModel.TaxReferenceNumber))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BusinessNameViewModel, Licence>()
                .ForMember(x => x.BusinessName, opt => opt.MapFrom(y => y.BusinessName))
                .ForMember(x => x.HasTradingName, opt => opt.MapFrom(y => y.HasTradingName))
                .ForMember(x => x.TradingName, opt => opt.MapFrom(y => y.TradingName))
                .ForMember(x => x.HasPreviousTradingName, opt => opt.MapFrom(y => y.HasPreviousTradingName))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<LegalStatusViewModel, Licence>()
                .ForMember(x => x.LegalStatus, opt => opt.MapFrom(y => y.LegalStatus))
                .ForMember(x => x.OtherLegalStatus, opt => opt.MapFrom(y => y.Other))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<PAYENumber, PAYENumberRow>()
                .ForMember(x => x.PAYENumber, opt => opt.MapFrom(y => y.Number))
                .ForMember(x => x.PAYERegistrationDate, opt => opt.MapFrom(y => y.RegistrationDate))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<PAYENumberRow, PAYENumber>()
                .ForMember(x => x.Number, opt => opt.MapFrom(y => y.PAYENumber))
                .ForMember(x => x.RegistrationDate, opt => opt.MapFrom(y => y.PAYERegistrationDate))
                .ForAllOtherMembers(opt => opt.Ignore());

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

            CreateMap<PAYEStatusViewModel, Licence>()
                .ForMember(x => x.HasPAYENumber, opt => opt.MapFrom(y => y.HasPAYENumber))
                .ForMember(x => x.PAYENumbers, opt => opt.MapFrom(y => y.PAYENumbers))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<VATStatusViewModel, Licence>()
                .ForMember(x => x.HasVATNumber, opt => opt.MapFrom(y => y.HasVATNumber))
                .ForMember(x => x.VATNumber, opt => opt.MapFrom(y => y.VATNumber))
                .ForMember(x => x.VATRegistrationDate, opt => opt.MapFrom(y => y.VATRegistrationDate))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<TaxReferenceViewModel, Licence>()
                .ForMember(x => x.TaxReferenceNumber, opt => opt.MapFrom(y => y.TaxReferenceNumber))
                //.ForMember(x => x.HasTaxReferenceNumber, opt => opt.MapFrom(y => y.HasTaxReferenceNumber))                
                .ForAllOtherMembers(opt => opt.Ignore());

        }

        private IEnumerable<PAYENumberRow> PAYENumberResolver(Licence licence)
        {
            return licence.PAYENumbers.Select(x => new PAYENumberRow
            {
                Id = x.Id,
                PAYENumber = x.Number,
                PAYERegistrationDate = new DateViewModel
                {
                    Date = x.RegistrationDate
                }
            });
        }

        private CompaniesHouseRegistrationViewModel CompaniesHouseRegistrationResolver(Licence licence)
        {
            return new CompaniesHouseRegistrationViewModel
            {
                CompaniesHouseNumber = licence.CompaniesHouseNumber,
                CompanyRegistrationDate = new DateViewModel
                {
                    Date = licence.CompanyRegistrationDate
                }
            };
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
                Other = licence.OtherLegalStatus
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
