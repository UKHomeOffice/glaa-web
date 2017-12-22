using GLAA.Domain;

namespace GLAA.ViewModels.LicenceApplication
{
    public class PersonViewModel : IPersonViewModel
    {
        public PersonViewModel()
        {
            FullName = new FullNameViewModel();
            AlternativeName = new AlternativeFullNameViewModel();
            DateOfBirth = new DateOfBirthViewModel();
            TownOfBirth = new TownOfBirthViewModel();
            CountryOfBirth = new CountryOfBirthViewModel();
            JobTitle = new JobTitleViewModel();
            Address = new AddressViewModel();
            BusinessPhoneNumber = new BusinessPhoneNumberViewModel();
            BusinessExtension = new BusinessExtensionViewModel();
            PersonalEmailAddress = new PersonalEmailAddressViewModel();
            PersonalMobileNumber = new PersonalMobileNumberViewModel();
            NationalInsuranceNumber = new NationalInsuranceNumberViewModel();
            Nationality = new NationalityViewModel();
            PassportViewModel = new PassportViewModel();
            UndischargedBankruptViewModel = new UndischargedBankruptViewModel();
            DisqualifiedDirectorViewModel = new DisqualifiedDirectorViewModel();
            RestraintOrdersViewModel = new RestraintOrdersViewModel();
            UnspentConvictionsViewModel = new UnspentConvictionsViewModel();
            OffencesAwaitingTrialViewModel = new OffencesAwaitingTrialViewModel();
            PreviousLicenceViewModel = new PreviousLicenceViewModel();
        }

        public FullNameViewModel FullName { get; set; }
        public AlternativeFullNameViewModel AlternativeName { get; set; }
        public DateOfBirthViewModel DateOfBirth { get; set; }
        public TownOfBirthViewModel TownOfBirth { get; set; }
        public CountryOfBirthViewModel CountryOfBirth { get; set; }
        public JobTitleViewModel JobTitle { get; set; }
        public AddressViewModel Address { get; set; }
        public BusinessPhoneNumberViewModel BusinessPhoneNumber { get; set; }
        public BusinessExtensionViewModel BusinessExtension { get; set; }
        public PersonalEmailAddressViewModel PersonalEmailAddress { get; set; }
        public PersonalMobileNumberViewModel PersonalMobileNumber { get; set; }
        public NationalInsuranceNumberViewModel NationalInsuranceNumber { get; set; }

        public NationalityViewModel Nationality { get; set; }
        public PassportViewModel PassportViewModel { get; set; }
        public UndischargedBankruptViewModel UndischargedBankruptViewModel { get; set; }
        public DisqualifiedDirectorViewModel DisqualifiedDirectorViewModel { get; set; }
        public RestraintOrdersViewModel RestraintOrdersViewModel { get; set; }
        public UnspentConvictionsViewModel UnspentConvictionsViewModel { get; set; }
        public OffencesAwaitingTrialViewModel OffencesAwaitingTrialViewModel { get; set; }
        public PreviousLicenceViewModel PreviousLicenceViewModel { get; set; }
    }
}
