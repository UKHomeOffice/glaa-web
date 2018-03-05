using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.LicenceApplication
{
    public class PersonViewModel : Validatable, IPersonViewModel, INeedCountries, INeedCounties, IIsSubmitted
    {
        private IEnumerable<SelectListItem> countries;

        public PersonViewModel()
        {
            FullName = new FullNameViewModel();
            AlternativeName = new AlternativeFullNameViewModel();
            DateOfBirth = new DateOfBirthViewModel();
            BirthDetails = new BirthDetailsViewModel();
            JobTitle = new JobTitleViewModel();
            Address = new AddressViewModel();
            BusinessPhoneNumber = new BusinessPhoneNumberViewModel();
            BusinessExtension = new BusinessExtensionViewModel();
            PersonalEmailAddress = new PersonalEmailAddressViewModel();
            PersonalMobileNumber = new PersonalMobileNumberViewModel();
            Nationality = new NationalityViewModel();
            Passport = new PassportViewModel();
            UndischargedBankrupt = new UndischargedBankruptViewModel();
            DisqualifiedDirector = new DisqualifiedDirectorViewModel();
            RestraintOrders = new RestraintOrdersViewModel();
            UnspentConvictions = new UnspentConvictionsViewModel();
            OffencesAwaitingTrial = new OffencesAwaitingTrialViewModel();
            PreviousLicence = new PreviousLicenceViewModel();
            Address = new AddressViewModel();
        }

        public bool IsUk { get; set; }
        public FullNameViewModel FullName { get; set; }
        public AlternativeFullNameViewModel AlternativeName { get; set; }
        public DateOfBirthViewModel DateOfBirth { get; set; }
        public BirthDetailsViewModel BirthDetails { get; set; }
        public JobTitleViewModel JobTitle { get; set; }
        public AddressViewModel Address { get; set; }
        public BusinessPhoneNumberViewModel BusinessPhoneNumber { get; set; }
        public BusinessExtensionViewModel BusinessExtension { get; set; }
        public PersonalEmailAddressViewModel PersonalEmailAddress { get; set; }
        public PersonalMobileNumberViewModel PersonalMobileNumber { get; set; }
        public NationalityViewModel Nationality { get; set; }
        public PassportViewModel Passport { get; set; }
        public UndischargedBankruptViewModel UndischargedBankrupt { get; set; }
        public DisqualifiedDirectorViewModel DisqualifiedDirector { get; set; }
        public RestraintOrdersViewModel RestraintOrders { get; set; }
        public UnspentConvictionsViewModel UnspentConvictions { get; set; }
        public OffencesAwaitingTrialViewModel OffencesAwaitingTrial { get; set; }
        public PreviousLicenceViewModel PreviousLicence { get; set; }

        public IEnumerable<SelectListItem> Counties
        {
            set => Address.Counties = value;
            get => Address?.Counties ?? new List<SelectListItem>();
        }

        public IEnumerable<SelectListItem> Countries
        {
            get => countries;
            set
            {
                countries = value;
                Address.Countries = value;
                BirthDetails.CountryOfBirthViewModel.Countries = value;
            }
        }

        public bool IsSubmitted { get; set; }
    }
}
