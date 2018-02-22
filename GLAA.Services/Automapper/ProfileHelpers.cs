using System.Collections.Generic;
using System.Linq;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.Services.Automapper
{
    public static class ProfileHelpers
    {
        public static FullNameViewModel FullNameResolver(Person person)
        {
            return new FullNameViewModel
            {
                FullName = person.FullName
            };
        }
        public static FullNameViewModel FullNameResolver(NamedIndividual person)
        {
            return new FullNameViewModel
            {
                FullName = person.FullName
            };
        }

        public static AlternativeFullNameViewModel AlternativeFullNameResolver(Person person)
        {
            return new AlternativeFullNameViewModel
            {
                AlternativeName = person.AlternativeName,
                HasAlternativeName = person.HasAlternativeName,
                YesNo = YesNoList
            };
        }

        public static TownOfBirthViewModel TownOfBirthResolver(Person person)
        {
            return new TownOfBirthViewModel
            {
                TownOfBirth = person.TownOfBirth
            };
        }

        public static JobTitleViewModel JobTitleResolver(Person person)
        {
            return new JobTitleViewModel
            {
                JobTitle = person.JobTitle
            };
        }

        public static BusinessPhoneNumberViewModel BusinessPhoneNumberResolver(Person person)
        {
            return new BusinessPhoneNumberViewModel
            {
                BusinessPhoneNumber = person.BusinessPhoneNumber
            };
        }

        public static BusinessPhoneNumberViewModel BusinessPhoneNumberResolver(NamedIndividual person)
        {
            return new BusinessPhoneNumberViewModel
            {
                BusinessPhoneNumber = person.BusinessPhoneNumber
            };
        }

        public static BusinessMobileNumberViewModel BusinessMobileNumberResolver(Person person)
        {
            return new BusinessMobileNumberViewModel
            {
                BusinessMobileNumber = person.BusinessPhoneNumber
            };
        }

        public static BusinessExtensionViewModel BusinessExtensionResolver(Person person)
        {
            return new BusinessExtensionViewModel
            {
                BusinessExtension = person.BusinessExtension
            };
        }

        public static BusinessExtensionViewModel BusinessExtensionResolver(NamedIndividual person)
        {
            return new BusinessExtensionViewModel
            {
                BusinessExtension = person.BusinessExtension
            };
        }

        public static PersonalEmailAddressViewModel PersonalEmailAddressResolver(Person person)
        {
            return new PersonalEmailAddressViewModel
            {
                PersonalEmailAddress = person.PersonalEmailAddress
            };
        }

        public static PersonalMobileNumberViewModel PersonalMobileNumberResolver(Person person)
        {
            return new PersonalMobileNumberViewModel
            {
                PersonalMobileNumber = person.PersonalMobileNumber
            };
        }

        public static NationalityViewModel NationalityResolver(Person person)
        {
            return new NationalityViewModel
            {
                Nationality = person.Nationality
            };
        }

        public static IsPreviousPrincipalAuthorityViewModel IsPreviousPrincipalAuthorityResolver(DirectorOrPartner dop)
        {
            return new IsPreviousPrincipalAuthorityViewModel
            {
                IsPreviousPrincipalAuthority = dop.IsPreviousPrincipalAuthority,
                YesNo = YesNoList
            };
        }

        public static BirthDetailsViewModel BirthDetailsResolver<T>(Person person, T viewModel) where T: PersonViewModel
        {
            return new BirthDetailsViewModel
            {
                NationalInsuranceNumberViewModel = NationalInsuranceNumberResolver(person),
                CountryOfBirthViewModel = CountryOfBirthResolver<T>(person, viewModel),
                TownOfBirthViewModel = TownOfBirthResolver(person),
                SocialSecurityNumberViewModel = SocialSecurityNumberResolver(person),
            };
        }

        public static BirthDetailsViewModel BirthDetailsResolver(Person person, BirthDetailsViewModel viewModel) 
        {
            return new BirthDetailsViewModel
            {
                NationalInsuranceNumberViewModel = NationalInsuranceNumberResolver(person),
                CountryOfBirthViewModel = CountryOfBirthResolver(person, viewModel),
                TownOfBirthViewModel = TownOfBirthResolver(person),
                SocialSecurityNumberViewModel = SocialSecurityNumberResolver(person),
            };
        }

        public static CountryOfBirthViewModel CountryOfBirthResolver<T>(Person person, T viewModel) where T : PersonViewModel
        {
            return new CountryOfBirthViewModel
            {
                CountryOfBirthId = person.CountryOfBirthId,
                Countries = viewModel.BirthDetailsViewModel.CountryOfBirthViewModel.Countries
            };
        }

        public static CountryOfBirthViewModel CountryOfBirthResolver(Person person, BirthDetailsViewModel viewModel)
        {
            return new CountryOfBirthViewModel
            {
                CountryOfBirthId = person.CountryOfBirthId,
                Countries = viewModel.CountryOfBirthViewModel.Countries
            };
        }

        public static NationalInsuranceNumberViewModel NationalInsuranceNumberResolver(Person person)
        {
            return new NationalInsuranceNumberViewModel
            {
                NationalInsuranceNumber = person.NationalInsuranceNumber,
                IsUk = person.Address?.Country?.IsUk != null && person.Address.Country.IsUk
            };
        }

        public static SocialSecurityNumberViewModel SocialSecurityNumberResolver(Person person)
        {
            return new SocialSecurityNumberViewModel
            {
                SocialSecurityNumber=  person.SocialSecurityNumber,
            };
        }

        public static OperatingIndustriesViewModel OperatingIndustriesResolver(Licence licence)
        {
            var vm = new OperatingIndustriesViewModel();

            if (licence.OperatingIndustries != null)
            {
                foreach (var industry in licence.OperatingIndustries)
                {
                    var match = vm.OperatingIndustries.Single(x => x.Id == industry.Id);
                    match.Checked = true;
                }
            }

            vm.OtherIndustry = licence.OtherOperatingIndustry;

            return vm;
        }

        public static bool DirectorsRequiredResolver(Licence licence)
        {
            return licence.LegalStatus.HasValue && (licence.LegalStatus == LegalStatusEnum.RegisteredCompany ||
                                                    licence.LegalStatus == LegalStatusEnum.Partnership);
        }

        public static List<SelectListItem> YesNoList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem {Text = "Yes", Value = "true"},
            new SelectListItem {Text = "No", Value = "false"}
        };

        public static PassportViewModel PassportViewModel(Person pa)
        {
            return new PassportViewModel
            {
                HasPassport = pa.HasPassport,
                YesNo = YesNoList
            };
        }

        public static RightToWorkViewModel RightToWorkResolver(Person pa)
        {
            return new RightToWorkViewModel
            {
                RequiresVisa = pa.RequiresVisa,
                VisaDescription = pa.VisaDescription
            };
        }

        public static RightToWorkViewModel RightToWorkResolver(NamedIndividual ni)
        {
            return new RightToWorkViewModel
            {
                RequiresVisa = ni.RequiresVisa,
                VisaDescription = ni.VisaDescription
            };
        }

        public static UndischargedBankruptViewModel UndischargedBankruptResolver(Person pa)
        {
            return new UndischargedBankruptViewModel
            {
                BankruptcyDate = new DateViewModel {Date = pa.BankruptcyDate},
                BankruptcyNumber = pa.BankruptcyNumber,
                IsUndischargedBankrupt = pa.IsUndischargedBankrupt,
                YesNo = YesNoList
            };
        }

        public static DisqualifiedDirectorViewModel DisqualifiedDirectorResolver(Person pa)
        {
            return new DisqualifiedDirectorViewModel
            {
                DisqualificationDetails = pa.DisqualificationDetails,
                IsDisqualifiedDirector = pa.IsDisqualifiedDirector,
                YesNo = YesNoList
            };
        }

        public static PreviousLicenceViewModel PreviousLicenceResolver(Person pa)
        {
            return new PreviousLicenceViewModel
            {
                HasPreviouslyHeldLicence = pa.HasPreviouslyHeldLicence,
                PreviousLicenceDescription = pa.PreviousLicenceDescription
            };
        }

        public static UndischargedBankruptViewModel UndischargedBankruptResolver(NamedIndividual pa)
        {
            return new UndischargedBankruptViewModel
            {
                BankruptcyDate = new DateViewModel {Date = pa.BankruptcyDate},
                BankruptcyNumber = pa.BankruptcyNumber,
                IsUndischargedBankrupt = pa.IsUndischargedBankrupt,
                YesNo = YesNoList
            };
        }

        public static DisqualifiedDirectorViewModel DisqualifiedDirectorResolver(NamedIndividual pa)
        {
            return new DisqualifiedDirectorViewModel
            {
                DisqualificationDetails = pa.DisqualificationDetails,
                IsDisqualifiedDirector = pa.IsDisqualifiedDirector,
                YesNo = YesNoList
            };
        }

        public static PreviousLicenceViewModel PreviousLicenceResolver(NamedIndividual pa)
        {
            return new PreviousLicenceViewModel
            {
                HasPreviouslyHeldLicence = pa.HasPreviouslyHeldLicence,
                PreviousLicenceDescription = pa.PreviousLicenceDescription
            };
        }

        public static ICollection<RestraintOrder> RestraintOrdersResolver(RestraintOrdersViewModel ro)
        {
            return ro.RestraintOrders.Select(r => new RestraintOrder
            {
                Id = r.Id,
                Description = r.Description,
                Date = r.Date.Date
            }) as ICollection<RestraintOrder>;
        }

        public static ICollection<Conviction> UnspentConvictionsResolver(UnspentConvictionsViewModel uc)
        {
            return uc.UnspentConvictions.Select(c => new Conviction
            {
                Id = c.Id,
                Description = c.Description,
                Date = c.Date.Date
            }) as ICollection<Conviction>;
        }

        public static ICollection<OffenceAwaitingTrial> OffencesAwaitingTrialResolver(OffencesAwaitingTrialViewModel uc)
        {
            return uc.OffencesAwaitingTrial.Select(o => new OffenceAwaitingTrial
            {
                Id = o.Id,
                Description = o.Description,
                Date = o.Date.Date
            }) as ICollection<OffenceAwaitingTrial>;
        }

        public static ICollection<PreviousTradingName> PreviousTradingNamesResolver(BusinessNameViewModel uc)
        {
            return uc.PreviousTradingNames.Select(p => new PreviousTradingName
            {
                Id = p.Id,
                BusinessName = p.BusinessName,
                Town = p.Town,
                Country = p.Country
            }) as ICollection<PreviousTradingName>;
        }
    }
}
